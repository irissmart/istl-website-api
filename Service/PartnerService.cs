using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.Interface;
using Framework.Model;
using Framework.Service;
using Infrastructure;
using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Service.DTOs.Request;
using Service.DTOs.Response;
using Service.Interface;

namespace Service
{
    public class PartnerService : BaseDatabaseService<IrisContext>, IPartnerService
    {
        private readonly IConfiguration _configuration;
        private readonly IFileService _fileService;
        private readonly string? _uploadPath;

        public PartnerService(IrisContext context, IConfiguration configuration, IFileService fileService) : base(context)
        {
            _configuration = configuration;
            _fileService = fileService;
            _uploadPath = _configuration["UploadPath"];
        }

        public async Task<BaseResponse<List<PartnerResponseDTO>>> GetAllAsync(BaseRequest request)
        {
            return await HandlePaginatedActionAsync(async () =>
            {
                var query = _context.Partners
                    .AsQueryable();

                if (!string.IsNullOrEmpty(request.Text))
                {
                    query = query.Where(x =>
                        x.FirstName.Contains(request.Text) ||
                        x.LastName.Contains(request.Text) ||
                        (x.MailUrl ?? string.Empty).Contains(request.Text));
                }

                query = query.OrderByDescending(x => x.UpdatedOn ?? x.CreatedOn);

                return await GetPaginatedResultAsync(query, request.PageNumber, request.PageSize);
            }, entity => new PartnerResponseDTO
            {
                ID = entity.Id,
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                Title = entity.Title,
                Description = entity.Description,
                Image = entity.Image != null ? _configuration["ImageUrl"] + entity.Image : null,
                TwitterUrl = entity.TwitterUrl,
                TiktokUrl = entity.TiktokUrl,
                LinkedinUrl = entity.LinkedinUrl,
                MailUrl = entity.MailUrl,
                WebsiteUrl = entity.WebsiteUrl
            });
        }

        public async Task<BaseResponse<PartnerResponseDTO?>> GetByIdAsync(int id)
        {
            return await HandleActionAsync(async () =>
            {
                var dbEntity = await _context.Partners
                    .AsNoTracking()
                    .Where(x => x.Id == id && x.IsActive)
                    .FirstOrDefaultAsync();

                if (dbEntity == null)
                {
                    InitMessageResponse("NotFound");
                    return null;
                }

                return new PartnerResponseDTO
                {
                    ID = dbEntity.Id,
                    FirstName = dbEntity.FirstName,
                    LastName = dbEntity.LastName,
                    Title = dbEntity.Title,
                    Description = dbEntity.Description,
                    Image = dbEntity.Image != null ? _configuration["ImageUrl"] + dbEntity.Image : null,
                    TwitterUrl = dbEntity.TwitterUrl,
                    TiktokUrl = dbEntity.TiktokUrl,
                    LinkedinUrl = dbEntity.LinkedinUrl,
                    MailUrl = dbEntity.MailUrl,
                    WebsiteUrl = dbEntity.WebsiteUrl
                };
            });
        }

        public async Task<BaseResponse<Task>> AddAsync(int userId, PartnerAddDTO request)
        {
            return await HandleVoidActionAsync(async () =>
            {
                var dbEntity = new Partner
                {
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Title = request.Title,
                    Description = request.Description,
                    TwitterUrl = request.TwitterUrl,
                    TiktokUrl = request.TiktokUrl,
                    LinkedinUrl = request.LinkedinUrl,
                    MailUrl = request.MailUrl,
                    WebsiteUrl = request.WebsiteUrl
                };

                if(request.Image != null)
                {
                    var imageName = await _fileService.UploadAsync(_uploadPath, request.Image);
                    dbEntity.Image = imageName;
                }

                await _context.Partners.AddAsync(dbEntity);
                await _context.SaveChangesAsync(userId);
            });
        }

        public async Task<BaseResponse<Task>> UpdateAsync(int userId, PartnerUpdateDTO request)
        {
            return await HandleVoidActionAsync(async () =>
            {
                var (doesNotExist, dbEntity) = await DoesNotExistAsync<Partner>(x => x.Id == request.Id);

                if (doesNotExist) return;

                dbEntity.FirstName = request.FirstName;
                dbEntity.LastName = request.LastName;
                dbEntity.Title = request.Title;
                dbEntity.Description = request.Description;
                dbEntity.TwitterUrl = request.TwitterUrl;
                dbEntity.TiktokUrl = request.TiktokUrl;
                dbEntity.LinkedinUrl = request.LinkedinUrl;
                dbEntity.MailUrl = request.MailUrl;
                dbEntity.WebsiteUrl = request.WebsiteUrl;

                if(request.Image != null)
                {
                    if(dbEntity.Image != null)
                    {
                        _fileService.DeleteFile(_uploadPath, dbEntity.Image);
                    }

                    var imageName = await _fileService.UploadAsync(_uploadPath, request.Image);
                    dbEntity.Image = imageName;
                }

                _context.Partners.Update(dbEntity);
                await _context.SaveChangesAsync(userId);
            });
        }

        public async Task<BaseResponse<Task>> DeleteAsync(int userId, int id)
        {
            return await HandleVoidActionAsync(async () =>
            {
                var (doesNotExist, dbEntity) = await DoesNotExistAsync<Partner>(x => x.Id == id);

                if (doesNotExist) return;

                if (dbEntity.Image != null)
                {
                    _fileService.DeleteFile(_uploadPath, dbEntity.Image);
                }

                dbEntity.IsActive = false;
                dbEntity.Image = null;

                _context.Partners.Update(dbEntity);
                await _context.SaveChangesAsync(userId);
            });
        }
    }
}
