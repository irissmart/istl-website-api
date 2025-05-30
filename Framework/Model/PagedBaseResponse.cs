namespace Framework.Model
{
    public class PagedBaseResponse<T> : BaseResponse<T>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }

        public PagedBaseResponse(bool success, string code, string message, T data, int pageNumber, int pageSize, int totalItems, int totalPages)
            : base(success, code, message, data)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalItems = totalItems;
            TotalPages = totalPages;
        }
    }
}