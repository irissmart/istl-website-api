using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Azure.Core;
using Framework.Configuration;
using Framework.Interface;
using Framework.Model;
using Framework.Service;
using Infrastructure;
using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Service.Constants;
using Service.DTOs;
using Service.Interface;

namespace Service
{
    public class AuthService : BaseDatabaseService<IrisContext>, IAuthService
    {
        protected readonly IJwtService _jwtService;
        protected readonly JwtConfig _jwtConfig;

        public AuthService(IrisContext context, IJwtService jwtService, JwtConfig jwtConfig)
            : base(context)
        {
            _jwtService = jwtService;
            _jwtConfig = jwtConfig;
        }

        public async Task<(AuthResponse, int)> AuthenticateAsync(AuthRequest request)
        {
            var query = _context
                .Users.AsNoTracking()
                .Where(x => x.Email == request.AuthId && x.IsActive);

            if (request.RoleId != 0)
            {
                query = query.Where(x => x.UserRoleId == request.RoleId);
            }

            var user = await query.FirstOrDefaultAsync();

            if (user == null || !VerifyPassword(request.Password, user.Password, user.SaltHash))
            {
                return (null, 404)!;
            }

            if (!user.IsVerified)
            {
                return (null, 403)!;
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, GetAuthRole((Enums.UserRole)user.UserRoleId)),
            };

            var jwtToken = _jwtService.GenerateJwtToken(claims);
            var refreshToken = GenerateRefreshToken(user.Id);

            await _context.RefreshTokens.AddAsync(refreshToken);
            await _context.SaveChangesAsync(user.Id);

            return (
                new AuthResponse
                {
                    JwtToken = jwtToken,
                    RefreshToken = refreshToken.Token,
                    JwtTokenExpires = DateTime.UtcNow.AddMinutes(
                        _jwtConfig.AccessTokenExpirationMinutes
                    ),
                    RefreshTokenExpires = refreshToken.Expires,
                },
                200
            );
        }

        private string GetAuthRole(Enums.UserRole userRole)
        {
            switch (userRole)
            {
                case Enums.UserRole.Admin:
                    return AuthRole.Admin;
                default:
                    return "";
            }
        }

        private RefreshToken GenerateRefreshToken(int userId)
        {
            var refreshToken = new RefreshToken
            {
                UserId = userId,
                Token = _jwtService.GenerateRefreshToken(),
                Expires = DateTime.UtcNow.AddDays(_jwtConfig.RefreshTokenExpirationDays),
                IsUsed = false,
                IsRevoked = false,
                Created = DateTime.UtcNow,
            };
            return refreshToken;
        }

        public async Task<AuthResponse> RefreshTokenAsync(RefreshTokenDTO request)
        {
            var refreshToken = await _context
                .RefreshTokens.Include(rt => rt.User)
                .SingleOrDefaultAsync(rt => rt.Token == request.Token);

            if (
                refreshToken == null
                || refreshToken.IsUsed
                || refreshToken.IsRevoked
                || refreshToken.Expires <= DateTime.UtcNow
            )
                throw new UnauthorizedAccessException(
                    ValidationMessage.InvalidRefreshToken
                );

            refreshToken.IsUsed = true;
            refreshToken.Used = DateTime.UtcNow;

            var user = refreshToken.User;
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            };

            var newJwtToken = _jwtService.GenerateJwtToken(claims);
            var newRefreshToken = GenerateRefreshToken(refreshToken.UserId);

            await _context.RefreshTokens.AddAsync(newRefreshToken);
            await _context.SaveChangesAsync(user.Id);

            return new AuthResponse
            {
                JwtToken = newJwtToken,
                RefreshToken = newRefreshToken.Token,
                JwtTokenExpires = DateTime.UtcNow.AddMinutes(
                    _jwtConfig.AccessTokenExpirationMinutes
                ),
                RefreshTokenExpires = newRefreshToken.Expires,
            };
        }

        private bool VerifyPassword(string requestPassword, string dbPassword, string saltHash)
        {
            var hashedPassword = GetHash_SHA256(requestPassword + saltHash);
            return (hashedPassword == dbPassword);
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
    }
}
