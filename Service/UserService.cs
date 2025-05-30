using Framework.Interface;
using Framework.Model;
using Framework.Service;
using Infrastructure;
using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using Service.DTOs.Request;
using Service.DTOs.Response;
using Service.Enums;
using Service.Interface;
using System.Security.Cryptography;
using System.Text;

namespace Service
{
    public class UserService : BaseDatabaseService<IrisContext>, IUserService
    {
        private readonly IConfiguration _configuration;
        private readonly IFileService _fileService;
        private readonly string? _uploadPath;
        private readonly IEmailService _emailService;

        public UserService(IrisContext context
            , IConfiguration configuration
            , IFileService fileService
            , IEmailService emailService) : base(context)
        {
            _configuration = configuration;
            _fileService = fileService;
            _uploadPath = _configuration["UploadPath"];
            _emailService = emailService;
        }

        public async Task<BaseResponse<UserDTO?>> GetByIdAsync(int id)
        {
            return await HandleActionAsync(async () =>
            {
                var dbEntity = await _context.Users
                    .AsNoTracking()
                    .Where(x => x.Id == id && x.IsActive)
                    .FirstOrDefaultAsync();

                if (dbEntity == null)
                {
                    InitMessageResponse("NotFound");
                    return null;
                }

                return new UserDTO
                {
                    Id = dbEntity.Id,
                    FirstName = dbEntity.FirstName, 
                    LastName = dbEntity.LastName, 
                    Email = dbEntity.Email
                };
            });
        }

        public async Task<BaseResponse<Task>> UpdateAsync(int userId, UserUpdateDTO request)
        {
            return await HandleVoidActionAsync(async () =>
            {
                var (doesNotExist, dbEntity) = await DoesNotExistAsync<User>(x =>
                    x.Id == userId && x.IsVerified);

                if (doesNotExist)
                    return;

                dbEntity.FirstName = request.FirstName != null ? request.FirstName : dbEntity.FirstName;
                dbEntity.LastName = request.LastName != null ? request.LastName : dbEntity.LastName;

                if (request.Image != null)
                {
                    var uploadPath = _configuration["UploadPath"];

                    if (!string.IsNullOrEmpty(dbEntity.ProfileImageRelativePath))
                    {
                        _fileService.DeleteFile(uploadPath, dbEntity.ProfileImageRelativePath);
                    }

                    var image = await _fileService.UploadAsync(uploadPath, request.Image);

                    dbEntity.ProfileImageRelativePath = image;
                }


                _context.Users.Update(dbEntity);
                await _context.SaveChangesAsync(userId);
            });
        }

        public async Task<BaseResponse<Task>> ForgetPasswordAsync(string email)
        {
            return await HandleVoidActionAsync(async () =>
            {
                if (string.IsNullOrEmpty(email))
                {
                    InitMessageResponse("BadRequest", "Email is required.");
                    return;
                }

                var (doesNotExist, dbEntity) = await DoesNotExistAsync<User>(x =>
                    x.Email == email && x.IsVerified);

                if (doesNotExist)
                    return;

                if(dbEntity.TokenExpiry <= DateTime.UtcNow)
                {
                    dbEntity.Token = GenerateUserIdentifier(dbEntity.SaltHash);
                    dbEntity.TokenExpiry = DateTime.UtcNow.AddMinutes(30);

                    _context.Users.Update(dbEntity);
                    await _context.SaveChangesAsync(dbEntity.Id);
                }

                var verificationUrl = _configuration["BaseUrl"] + "api/user/verify-token?token=" + dbEntity.Token;

                var htmlBody = $@"
                        <div style='font-family: Arial, sans-serif; text-align: center;'>
                            <h2 style='color: #882839;'>Reset Password</h2>
                            <p>Click the button below to reset your password:</p>
                            <a href='{verificationUrl}' style='display: inline-block; padding: 10px 20px; font-size: 16px; color: white; background-color: #882839; text-decoration: none; border-radius: 5px;'>Reset Password</a>
                        </div>";

                await _emailService.SendEmailAsync(email, "Reset Password", htmlBody);
            });
        }

        public async Task<BaseResponse<Task>> VerifyTokenAsync(string token)
        {
            return await HandleVoidActionAsync(async () =>
            {
                if (string.IsNullOrEmpty(token))
                {
                    InitMessageResponse("BadRequest", "Token is required.");
                    return;
                }

                var (doesNotExist, dbEntity) = await DoesNotExistAsync<User>(x =>
                    x.Token == token && x.IsVerified, "Invalid Token.");

                if (doesNotExist)
                    return;

                if (dbEntity.TokenExpiry <= DateTime.UtcNow)
                {
                    InitMessageResponse("BadRequest", "Token Expired.");
                    return;
                }
            });
        }

        public async Task<BaseResponse<Task>> ResetPasswordAsync(string password, string token)
        {
            return await HandleVoidActionAsync(async () =>
            {
                if (string.IsNullOrEmpty(password))
                {
                    InitMessageResponse("BadRequest", "Password is required.");
                    return;
                }

                if (string.IsNullOrEmpty(token))
                {
                    InitMessageResponse("BadRequest", "Token is required.");
                    return;
                }

                var (doesNotExist, dbEntity) = await DoesNotExistAsync<User>(x =>
                    x.Token == token && x.IsVerified, "Invalid Token.");

                if (doesNotExist)
                    return;

                if (dbEntity.TokenExpiry <= DateTime.UtcNow)
                {
                    InitMessageResponse("BadRequest", "Token Expired.");
                    return;
                }

                var passwordStrength = CheckPasswordStrength(password);

                if (passwordStrength == (int)PasswordStrength.Strong)
                {
                    var saltHash = GetSaltString(4);

                    dbEntity.SaltHash = saltHash;
                    dbEntity.Password = GetHash_SHA256(password + saltHash);
                    dbEntity.TokenExpiry = DateTime.UtcNow.AddMinutes(-60);

                    _context.Users.Update(dbEntity);
                    await _context.SaveChangesAsync(dbEntity.Id);
                }
                else
                {
                    InitMessageResponse("BadRequest", $"Your password strength is {Enum.GetName(typeof(PasswordStrength), passwordStrength)},  Enter a strong password!");
                }
            });
        }

        public async Task<BaseResponse<Task>> ChangePasswordAsync(int id, ChangePasswordDTO request)
        {
            return await HandleVoidActionAsync(async () =>
            {
                var dbEntity = await _context.Users
                    .FirstOrDefaultAsync(x => x.Id == id && x.IsActive && x.IsVerified);

                if (dbEntity == null)
                {
                    InitMessageResponse("Unauthorized");
                    return;
                }

                if (GetHash_SHA256(request.OldPassword + dbEntity.SaltHash) != dbEntity.Password)
                {
                    InitMessageResponse("BadRequest", "Invalid Password");
                    return;
                }

                var passwordStrength = CheckPasswordStrength(request.NewPassword);

                if (passwordStrength == (int)PasswordStrength.Strong)
                {
                    var saltHash = GetSaltString(5);

                    dbEntity.SaltHash = saltHash;
                    dbEntity.Password = GetHash_SHA256(request.NewPassword + dbEntity.SaltHash);

                    _context.Users.Update(dbEntity);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    InitMessageResponse("BadRequest", $"Your password strength is {Enum.GetName(typeof(PasswordStrength), passwordStrength)}, Enter a strong password!");
                }
            });
        }

        public string GenerateUserIdentifier(string saltHash)
        {
            byte[] randomBytes = new byte[32];

            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomBytes);
            }

            string rawData = saltHash + DateTime.UtcNow.Ticks + Convert.ToBase64String(randomBytes);
            string hashBytes = GetHash_SHA256(rawData);

            return hashBytes;
        }

        private string GetHash_SHA256(string stringToHash)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(stringToHash));

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }

                return builder.ToString().ToUpper();
            }
        }

        private string GetSaltString(int length)
        {
            Random random = new Random();

            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public int CheckPasswordStrength(string password)
        {
            HashSet<char> specialCharacters = new HashSet<char>(new char[] { '!', '@', '#', '$', '%', '^', '&', '*', '(', ')', '-', '+', '?', '/' });
            bool hasLower = false, hasUpper = false, hasDigit = false, hasSpecial = false;

            foreach (char c in password)
            {
                if (char.IsLower(c)) hasLower = true;
                if (char.IsUpper(c)) hasUpper = true;
                if (char.IsDigit(c)) hasDigit = true;
                if (specialCharacters.Contains(c)) hasSpecial = true;
            }

            if (password.Length >= 8 && hasLower && hasUpper && hasDigit && hasSpecial)
            {
                return (int)PasswordStrength.Strong;
            }
            else if (password.Length >= 6 && (hasLower || hasUpper || hasSpecial))
            {
                return (int)PasswordStrength.Moderate;
            }
            else
            {
                return (int)PasswordStrength.Weak;
            }
        }
    }
}
