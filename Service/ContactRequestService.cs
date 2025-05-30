using Framework.Model;
using Framework.Service;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Service.DTOs.Request;
using Service.DTOs.Response;
using Service.Interface;

namespace Service
{
    public class ContactRequestService : BaseDatabaseService<IrisContext>, IContactRequestService
    {
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;

        public ContactRequestService(IrisContext context, IEmailService emailService, IConfiguration configuration) : base(context)
        {
            _emailService = emailService;
            _configuration = configuration;
        }
        public async Task<BaseResponse<Task>> AddAsync(ContactRequestAddDTO contactRequestAddDTO)
        {
            return await HandleVoidActionAsync(async () =>
            {
                var toEmail = _configuration["AdminEmail"];

                var htmlBody = $@"
                      <div style=""font-family: Arial, sans-serif; max-width:600px; margin:0 auto; padding:20px; border:1px solid #e0e0e0; border-radius:8px;"">
                        <h2 style=""color: #882839; margin-bottom:20px;"">New Contact Request</h2>
                        <table style=""width:100%; border-collapse:collapse;"">
                          <tr>
                            <td style=""padding:8px; font-weight:bold; width:120px;"">First Name:</td>
                            <td style=""padding:8px;"">{contactRequestAddDTO.FirstName}</td>
                          </tr>
                          <tr style=""background:#f9f9f9;"">
                            <td style=""padding:8px; font-weight:bold;"">Last Name:</td>
                            <td style=""padding:8px;"">{contactRequestAddDTO.LastName}</td>
                          </tr>
                          <tr>
                            <td style=""padding:8px; font-weight:bold;"">Email:</td>
                            <td style=""padding:8px;"">{contactRequestAddDTO.Email}</td>
                          </tr>
                          <tr style=""background:#f9f9f9;"">
                            <td style=""padding:8px; font-weight:bold;"">Phone No.:</td>
                            <td style=""padding:8px;"">{contactRequestAddDTO.PhoneNo}</td>
                          </tr>
                        </table>

                        <div style=""margin-top:20px;"">
                          <p style=""font-weight:bold; margin-bottom:8px;"">Message:</p>
                          <p style=""background:#f4f4f4; padding:12px; border-radius:4px; line-height:1.5;"">
                            {contactRequestAddDTO.Message}
                          </p>
                        </div>
                      </div>";

                await _emailService.SendEmailAsync(toEmail, "Contact Request", htmlBody);
            });
        }

        public async Task<BaseResponse<List<ContactRequestDTO>>> GetAllAsync(BaseRequest? request)
        {
            return await HandlePaginatedActionAsync(async () =>
            {
                var dbEntity = _context.ContactRequests
                    .AsNoTracking()
                    .Where(x => x.IsActive)
                    .AsQueryable();

                dbEntity = dbEntity.OrderByDescending(x => x.CreatedOn);

                return await GetPaginatedResultAsync(dbEntity, request.PageNumber, request.PageSize);
            }, entity => new ContactRequestDTO
            {
                Id = entity.Id,
                FullName = entity.FirstName + " " + entity.LastName,
                Email = entity.Email,
                PhoneNo = entity.PhoneNo,
                Message = entity.Message
            });
        }

        public async Task<BaseResponse<ContactRequestDTO?>> GetByIdAsync(int id)
        {
            return await HandleActionAsync(async () =>
            {
                var dbEntity = await _context.ContactRequests
                    .AsNoTracking()
                    .Where(x => x.IsActive && x.Id == id)
                    .FirstOrDefaultAsync();

                if(dbEntity == null)
                {
                    InitMessageResponse("NotFound");
                    return null;
                }

                return new ContactRequestDTO
                {
                    Id = dbEntity.Id,
                    FullName = dbEntity.FirstName + " " + dbEntity.LastName,
                    Email = dbEntity.Email,
                    PhoneNo = dbEntity.PhoneNo,
                    Message = dbEntity.Message
                };
            });
        }
    }
}
