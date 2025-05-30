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
    public class ClientServiceService : BaseDatabaseService<IrisContext>, IClientServiceService
    {
        private readonly IConfiguration _configuration;
        private readonly IFileService _fileService;
        private readonly string? _uploadPath;

        public ClientServiceService(IrisContext context
            , IConfiguration configuration
            , IFileService fileService) : base(context)
        {
            _configuration = configuration;
            _fileService = fileService;
            _uploadPath = _configuration["UploadPath"];
        }

        public async Task<BaseResponse<ClientServiceDTO?>> GetByIdAsync(int id)
        {
            return await HandleActionAsync(async () =>
            {
                var dbEntity = await _context.ClientServices
                    .AsNoTracking()
                    .Where(x => x.Id == id && x.IsActive)
                    .FirstOrDefaultAsync();

                if (dbEntity == null)
                {
                    InitMessageResponse("NotFound");
                    return null;
                }

                return new ClientServiceDTO
                {
                    Id = dbEntity.Id,
                    ClientServiceCategoryId = dbEntity.ClientServiceCategoryId,
                    ServiceName = dbEntity.ServiceName,
                    Description = dbEntity.Description,
                    ImagePath = dbEntity.ServiceImagePath != null ? _configuration["ImageUrl"] + dbEntity.ServiceImagePath : null
                };
            });
        }

        public async Task<BaseResponse<List<ClientServiceDTO>>> GetAllFilteredAsync(int? categoryId, BaseRequest request)
        {
            return await HandlePaginatedActionAsync(async () =>
            {
                var query = _context.ClientServices
                    .AsQueryable();

                if (categoryId != null)
                {
                    query = query.Where(x => x.ClientServiceCategoryId == categoryId);
                }

                if (!string.IsNullOrEmpty(request.Text))
                {
                    query = query.Where(x => x.ServiceName.Contains(request.Text));
                }

                query = query.OrderByDescending(x => x.UpdatedOn ?? x.CreatedOn);

                return await GetPaginatedResultAsync(query, request.PageNumber, request.PageSize);
            }, entity => new ClientServiceDTO
            {
                Id = entity.Id,
                ClientServiceCategoryId = entity.ClientServiceCategoryId,
                ServiceName = entity.ServiceName,
                Description = entity.Description,
                ImagePath = entity.ServiceImagePath != null ? _configuration["ImageUrl"] + entity.ServiceImagePath : null
            });
        }

        public async Task<BaseResponse<List<ClientServiceDTO>>> GetAllRelatedAsync(int id, string serviceName, BaseRequest request)
        {
            return await HandlePaginatedActionAsync(async () =>
            {
                var query = _context.ClientServices
                    .Where(x => x.Id != id)
                    .AsQueryable();

                if (!string.IsNullOrWhiteSpace(serviceName))
                {
                    query = query.Where(x => x.ServiceName.Contains(serviceName) ||
                                             x.ServiceName.StartsWith(serviceName) ||
                                             x.ServiceName.EndsWith(serviceName));
                }

                if (!query.Any())
                {
                    query = _context.ClientServices
                         .Where(x => x.Id != id)
                         .AsQueryable();
                }

                return await GetPaginatedResultAsync(query, request.PageNumber, request.PageSize);
            }, entity => new ClientServiceDTO
            {
                Id = entity.Id,
                ClientServiceCategoryId = entity.ClientServiceCategoryId,
                ServiceName = entity.ServiceName,
                Description = entity.Description,
                ImagePath = entity.ServiceImagePath != null ? _configuration["ImageUrl"] + entity.ServiceImagePath : null
            });

        }

        public async Task<BaseResponse<Task>> AddAsync(int userId, ClientServiceAddDTO request)
        {
            return await HandleVoidActionAsync(async () =>
            {
                if (await IsDuplicateAsync<Infrastructure.Entities.ClientService>(x =>
                x.ServiceName == request.ServiceName)) return;

                var dbEntity = new Infrastructure.Entities.ClientService
                {
                    ClientServiceCategoryId = request.ClientServiceCategoryId,
                    ServiceName = request.ServiceName,
                    Description = request.Description,
                };

                if (request.Image != null)
                {
                    var image = await _fileService.UploadAsync(_uploadPath, request.Image);
                    dbEntity.ServiceImagePath = image;
                }

                await _context.ClientServices.AddAsync(dbEntity);
                await _context.SaveChangesAsync(userId);
            });
        }

        public async Task<BaseResponse<Task>> UpdateAsync(int userId, ClientServiceUpdateDTO request)
        {
            return await HandleVoidActionAsync(async () =>
            {
                var (doesNotExist, dbEntity) = await 
                    DoesNotExistAsync<Infrastructure.Entities.ClientService>(x => x.Id == request.Id);
                
                if (doesNotExist) return;

                dbEntity.ClientServiceCategoryId = request.ClientServiceCategoryId != 0 ? request.ClientServiceCategoryId : dbEntity.ClientServiceCategoryId;
                dbEntity.ServiceName = request.ServiceName ?? dbEntity.ServiceName;
                dbEntity.Description = request.Description ?? dbEntity.Description;

                if (request.Image != null)
                {
                    if (!string.IsNullOrEmpty(dbEntity.ServiceImagePath))
                    {
                        _fileService.DeleteFile(_uploadPath, dbEntity.ServiceImagePath);
                    }

                    var image = await _fileService.UploadAsync(_uploadPath, request.Image);

                    dbEntity.ServiceImagePath = image;
                }

                _context.ClientServices.Update(dbEntity);
                await _context.SaveChangesAsync(userId);
            });
        }

        public async Task<BaseResponse<Task>> DeleteAsync(int userId, int id)
        {
            return await HandleVoidActionAsync(async () =>
            {
                var (doesNotExist, dbEntity) = await DoesNotExistAsync<Infrastructure.Entities.ClientService>(x => x.Id == id);

                if (doesNotExist) return;

                if (dbEntity.ServiceImagePath != null)
                {
                    _fileService.DeleteFile(_uploadPath, dbEntity.ServiceImagePath);
                }

                dbEntity.IsActive = false;
                dbEntity.ServiceImagePath = "";

                _context.ClientServices.Update(dbEntity);
                await _context.SaveChangesAsync(userId);
            });
        }
    }
}
