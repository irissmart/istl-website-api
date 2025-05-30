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
using Microsoft.Extensions.Configuration;
using Service.DTOs.Request;
using Service.DTOs.Response;
using Service.Interface;

namespace Service
{
    public class ClientService : BaseDatabaseService<IrisContext>, IClientService
    {
        private readonly IConfiguration _configuration;
        private readonly IFileService _fileService;
        private readonly string? _uploadPath;

        public ClientService(IrisContext context, IConfiguration configuration, IFileService fileService) : base(context)
        {
            _configuration = configuration;
            _fileService = fileService;
            _uploadPath = _configuration["UploadPath"];
        }

        public async Task<BaseResponse<List<ClientResponseDTO>>> GetAllAsync(BaseRequest request)
        {
            return await HandlePaginatedActionAsync(async () =>
            {
                var query = _context.Clients
                    .AsQueryable();

                if (!string.IsNullOrEmpty(request.Text))
                {
                    query = query.Where(x => x.Name.Contains(request.Text));
                }

                query = query.OrderByDescending(x => x.UpdatedOn ?? x.CreatedOn);

                return await GetPaginatedResultAsync(query, request.PageNumber, request.PageSize);
            }, entity => new ClientResponseDTO
            {
                Id = entity.Id,
                Name = entity.Name,
                Image = entity.Image != null ? _configuration["ImageUrl"] + entity.Image : null,
                Url = entity.Url
            });
        }

        public async Task<BaseResponse<ClientResponseDTO?>> GetByIdAsync(int id)
        {
            return await HandleActionAsync(async () =>
            {
                var dbEntity = await _context.Clients
                    .AsNoTracking()
                    .Where(x => x.Id == id && x.IsActive)
                    .FirstOrDefaultAsync();

                if (dbEntity == null)
                {
                    InitMessageResponse("NotFound");
                    return null;
                }

                return new ClientResponseDTO
                {
                    Id = dbEntity.Id,
                    Name = dbEntity.Name,
                    Image = dbEntity.Image != null ? _configuration["ImageUrl"] + dbEntity.Image : null,
                    Url = dbEntity.Url
                };
            });
        }

        public async Task<BaseResponse<Task>> AddAsync(int userId, ClientAddDTO request)
        {
            return await HandleVoidActionAsync(async () =>
            {
                var dbEntity = new Client
                {
                    Name = request.Name,
                    Url = request.Url
                };

                if (request.Image != null)
                {
                    var imageName = await _fileService.UploadAsync(_uploadPath, request.Image);
                    dbEntity.Image = imageName;
                }

                await _context.Clients.AddAsync(dbEntity);
                await _context.SaveChangesAsync(userId);
            });
        }

        public async Task<BaseResponse<Task>> UpdateAsync(int userId, ClientUpdateDTO request)
        {
            return await HandleVoidActionAsync(async () =>
            {

                var (doesNotExist, dbEntity) = await DoesNotExistAsync<Client>(x => x.Id == request.Id);

                dbEntity.Name = request.Name;
                dbEntity.Url = request.Url;

                if (request.Image != null)
                {
                    if (dbEntity.Image != null)
                    {
                        _fileService.DeleteFile(_uploadPath, dbEntity.Image);
                    }

                    var ImageName = await _fileService.UploadAsync(_uploadPath, request.Image);
                    dbEntity.Image = ImageName;
                }

                _context.Clients.Update(dbEntity);
                await _context.SaveChangesAsync(userId);

            });
        }

        public async Task<BaseResponse<Task>> DeleteAsync(int userId, int id)
        {
            return await HandleVoidActionAsync(async () =>
            {

                var (doesNotExist, dbEntity) = await DoesNotExistAsync<Client>(x => x.Id == id);

                if (doesNotExist) return;

                if (dbEntity.Image != null)
                {
                    _fileService.DeleteFile(_uploadPath, dbEntity.Image);
                }

                dbEntity.IsActive = false;
                dbEntity.Image = "";

                _context.Clients.Update(dbEntity);
                await _context.SaveChangesAsync(userId);
            });
        }
    }
}
