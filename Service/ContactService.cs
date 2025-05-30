using Framework.Model;
using Framework.Service;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Service.DTOs.Response;
using Service.Interface;

namespace Service
{
    public class ContactService : BaseDatabaseService<IrisContext>, IContactService
    {
        private readonly IConfiguration _configuration;

        public ContactService(IrisContext context, IConfiguration configuration) : base(context)
        {
            _configuration = configuration;
        }

        public async Task<BaseResponse<ContactDTO?>> GetAsync()
        {
            return await HandleActionAsync(async () =>
            {
                var dbEntity = await _context.ContactInformations
                    .AsNoTracking()
                    .Include(x => x.SocialLinks.Where(x => x.IsActive))
                    .Where(x => x.IsActive)
                    .FirstOrDefaultAsync();

                var section = await _context.Sections
                    .AsNoTracking()
                    .Where(x => x.IsActive && x.Id == 23)
                    .FirstOrDefaultAsync();

                if (dbEntity == null)
                {
                    InitMessageResponse("NotFound");
                    return null;
                }

                return new ContactDTO
                {
                    Id = dbEntity.Id,
                    PhoneNo = dbEntity.PhoneNo,
                    Email = dbEntity.Email,
                    Address = dbEntity.Address,
                    BannerImage = section.BackgroundImageRelativePath != null ? _configuration["ImageUrl"] + section.BackgroundImageRelativePath : null,
                    SocialLinks = dbEntity.SocialLinks
                    .Select(x => new SocialLink
                    {
                        Id = x.Id,
                        PlatformName = x.PlatformName,
                        Url = x.Url
                    })
                };
            });
        }

        public async Task<BaseResponse<Task>> UpdateAsync(int userId, ContactDTO request)
        {
            return await HandleVoidActionAsync(async () =>
            {
                var dbEntity = await _context.ContactInformations
                    .Where(x => x.IsActive)
                    .Include(x => x.SocialLinks.Where(x => x.IsActive))
                    .AsSplitQuery()
                    .FirstOrDefaultAsync();

                if (dbEntity == null)
                {
                    InitMessageResponse("NotFound");
                    return;
                }

                dbEntity.PhoneNo = request.PhoneNo ?? dbEntity.PhoneNo;
                dbEntity.Email = request.Email ?? dbEntity.Email;
                dbEntity.Address = request.Address ?? dbEntity.Address;

                foreach (var socialLink in request.SocialLinks)
                {
                    var dbSocialLink = dbEntity.SocialLinks.FirstOrDefault(s => s.Id == socialLink.Id);
                    
                    if (dbSocialLink == null)
                    {
                        InitMessageResponse("BadRequest");
                        return;
                    }

                    dbSocialLink.PlatformName = socialLink.PlatformName;
                    dbSocialLink.Url = socialLink.Url;

                    _context.SocialLinks.Update(dbSocialLink);
                }

                await _context.SaveChangesAsync(userId);
            });
        }
    }
}
