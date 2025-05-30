using System;
using System.Collections.Generic;
using System.Data.Common;
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
    public class UserPermission : BaseDatabaseService<IrisContext>, IUserPermission
    {
        private readonly IConfiguration _configuration;

        public UserPermission(IrisContext context, IConfiguration configuration) : base(context)
        {
            _configuration = configuration;
        }

        public async Task<BaseResponse<List<dynamic>>> GetAllAsync(BaseRequest request)
        {
            return await HandlePaginatedActionAsync(async () =>
            {
                var query = _context.UserPermissions
                    .AsQueryable();

                return await GetPaginatedResultAsync(query, request.PageNumber, request.PageSize);
            }, entity => (dynamic)new
            {
                ID = entity.Id,
                UserId = entity.UserId,
                ModuleName = entity.ModuleName,
                CanView = entity.CanView,
                CanCreate = entity.CanCreate,
                CanUpdate = entity.CanUpdate,
                CanDelete = entity.CanDelete
            });
        }

        public async Task<BaseResponse<List<dynamic>>> GetAllByUserIdAsync(int userId, BaseRequest request)
        {
            return await HandlePaginatedActionAsync(async () =>
            {
                var query = _context.UserPermissions
                    .Where(x => x.UserId == userId)
                    .AsQueryable();

                return await GetPaginatedResultAsync(query, request.PageNumber, request.PageSize);
            }, entity => (dynamic)new
            {
                ID = entity.Id,
                UserId = entity.UserId,
                ModuleName = entity.ModuleName,
                CanView = entity.CanView,
                CanCreate = entity.CanCreate,
                CanUpdate = entity.CanUpdate,
                CanDelete = entity.CanDelete
            });
        }

    }
}
