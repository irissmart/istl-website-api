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
    public class TagService : BaseDatabaseService<IrisContext>, ITagService
    {
        public TagService(IrisContext context) : base(context)
        {
        }

        public async Task<BaseResponse<List<TagDTO>>> GetAllAsync(BaseRequest request)
        {
            return await HandlePaginatedActionAsync(async () =>
            {
                var query = _context.Tags
                    .AsQueryable();

                query = query.OrderByDescending(x => x.TagName);

                return await GetPaginatedResultAsync(query, request.PageNumber, request.PageSize);
            }, entity => new TagDTO
            {
                Id = entity.Id,
                TagName = entity.TagName
            });
        }

        public async Task<BaseResponse<Task>> AddAsync(int userId, string tagName)
        {
            return await HandleVoidActionAsync(async () =>
            {
                if (await IsDuplicateAsync<Tag>(x => x.TagName == tagName)) return;

                var dbEntity = new Tag
                {
                    TagName = tagName,
                };

                await _context.Tags.AddAsync(dbEntity);
                await _context.SaveChangesAsync(userId);
            });
        }

        public async Task<BaseResponse<Task>> UpdateAsync(int userId, TagUpdateDTO request)
        {
            return await HandleVoidActionAsync(async () =>
            {
                var (doesNotExist, dbEntity) = await 
                    DoesNotExistAsync<Tag>(x => x.Id == request.Id);
                
                if (doesNotExist) return;

                dbEntity.TagName = request.TagName ?? dbEntity.TagName;
                //dbEntity. = request.ClientName ?? dbEntity.ClientName;

                _context.Tags.Update(dbEntity);
                await _context.SaveChangesAsync(userId);
            });
        }
    }
}
