using Framework.Model;
using Framework.Service;
using Infrastructure;
using Service.DTOs.Response;
using Service.Interface;

namespace Service
{
    public class PageService : BaseDatabaseService<IrisContext>, IPageService
    {
        public PageService(IrisContext context) : base(context)
        {
        }

        public async Task<BaseResponse<List<PageResponseDTO>>> GetAllAsync(BaseRequest dto)
        {
            return await HandlePaginatedActionAsync(async () =>
            {
                var query = _context.Pages
                    .Where(x => x.IsActive)
                    .OrderByDescending(x => x.CreatedOn);

                return await GetPaginatedResultAsync(query, dto.PageNumber, dto.PageSize);
            }, page => new PageResponseDTO
            {
                Id = page.Id,
                PageName = page.PageName
            });
        }
    }
}
