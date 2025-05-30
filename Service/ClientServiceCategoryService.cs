using Framework.Interface;
using Framework.Model;
using Framework.Service;
using Infrastructure;
using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Service.DTOs.Request;
using Service.DTOs.Response;
using Service.Interface;

namespace Service
{
    public class ClientServiceCategoryService : BaseDatabaseService<IrisContext>, IClientServiceCategoryService
    {
        private readonly IConfiguration _configuration;
        private readonly IFileService _fileService;
        private readonly string? _uploadPath;

        public ClientServiceCategoryService(IrisContext context
            , IConfiguration configuration
            , IFileService fileService) : base(context)
        {
            _configuration = configuration;
            _fileService = fileService;
            _uploadPath = _configuration["UploadPath"];
        }

        public async Task<BaseResponse<List<ClientServiceCategoryDTO>>> GetAllAsync(BaseRequest request)
        {
            return await HandlePaginatedActionAsync(async () =>
            {
                var query = _context.ClientServiceCategories
                    .AsQueryable();

                query = query.OrderByDescending(x => x.UpdatedOn ?? x.CreatedOn);

                if (!string.IsNullOrEmpty(request.Text))
                {
                    query = query.Where(x => x.ServiceCategoryName.Contains(request.Text));
                }

                return await GetPaginatedResultAsync(query, request.PageNumber, request.PageSize);
            }, entity => new ClientServiceCategoryDTO
            {
                Id = entity.Id,
                ServiceCategoryName = entity.ServiceCategoryName,
                Description = entity.Description,
                ImagePath = entity.CategoryImagePath != null ? _configuration["ImageUrl"] + entity.CategoryImagePath : null
            });
        }

        public async Task<BaseResponse<ClientServiceCategoryDTO?>> GetByIdAsync(int id)
        {
            return await HandleActionAsync(async () =>
            {
                var dbEntity = await _context.ClientServiceCategories
                    .AsNoTracking()
                    .Where(x => x.Id == id && x.IsActive)
                    .FirstOrDefaultAsync();

                if (dbEntity == null)
                {
                    InitMessageResponse("NotFound");
                    return null;
                }

                return new ClientServiceCategoryDTO
                {
                    Id = dbEntity.Id,
                    ServiceCategoryName = dbEntity.ServiceCategoryName,
                    Description = dbEntity.Description,
                    ImagePath = dbEntity.CategoryImagePath != null ? _configuration["ImageUrl"] + dbEntity.CategoryImagePath : null
                };
            });
        }

        public async Task<BaseResponse<Task>> AddAsync(int userId, ClientServiceCategoryAddDTO request)
        {
            return await HandleVoidActionAsync(async () =>
            {
                if (await IsDuplicateAsync<ClientServiceCategory>(x =>
                x.ServiceCategoryName == request.ServiceCategoryName)) return;

                var dbEntity = new ClientServiceCategory
                {
                    ServiceCategoryName = request.ServiceCategoryName,
                    Description = request.Description,
                };

                if (request.Image != null)
                {
                    var image = await _fileService.UploadAsync(_uploadPath, request.Image);
                    dbEntity.CategoryImagePath = image;
                }

                await _context.ClientServiceCategories.AddAsync(dbEntity);
                await _context.SaveChangesAsync(userId);
            });
        }

        public async Task<BaseResponse<Task>> UpdateAsync(int userId, ClientServiceCategoryUpdateDTO request)
        {
            return await HandleVoidActionAsync(async () =>
            {
                var (doesNotExist, dbEntity) = await 
                    DoesNotExistAsync<ClientServiceCategory>(x => x.Id == request.Id);
                
                if (doesNotExist) return;

                dbEntity.ServiceCategoryName = request.ServiceCategoryName ?? dbEntity.ServiceCategoryName;
                dbEntity.Description = request.Description ?? dbEntity.Description;

                if (request.Image != null)
                {
                    if (!string.IsNullOrEmpty(dbEntity.CategoryImagePath))
                    {
                        _fileService.DeleteFile(_uploadPath, dbEntity.CategoryImagePath);
                    }

                    var image = await _fileService.UploadAsync(_uploadPath, request.Image);

                    dbEntity.CategoryImagePath = image;
                }

                _context.ClientServiceCategories.Update(dbEntity);
                await _context.SaveChangesAsync(userId);
            });
        }

        public async Task<BaseResponse<Task>> DeleteAsync(int userId, int id)
        {
            return await HandleVoidActionAsync(async () =>
            {
                var dbEntity = await _context.ClientServiceCategories
                    .Where(x => x.IsActive && x.Id == id)
                    .Include(x => x.ClientServices)
                    .FirstOrDefaultAsync();

                if (dbEntity == null)
                {
                    InitMessageResponse("NotFound");
                    return;
                }

                if(dbEntity.ClientServices.Count() > 0)
                {
                    foreach (var service in dbEntity.ClientServices)
                    {
                        if (service.ServiceImagePath != null)
                        {
                            _fileService.DeleteFile(_uploadPath, service.ServiceImagePath);
                        }

                        service.IsActive = false;
                        service.ServiceImagePath = "";

                        _context.ClientServices.Update(service);
                    }
                }

                if (dbEntity.CategoryImagePath != null)
                {
                    _fileService.DeleteFile(_uploadPath, dbEntity.CategoryImagePath);
                }

                dbEntity.IsActive = false;
                dbEntity.CategoryImagePath = null;

                _context.ClientServiceCategories.Update(dbEntity);
                await _context.SaveChangesAsync(userId);
            });
        }
    }
}
