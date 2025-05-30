namespace Service.DTOs.BaseDTO
{
    public class PagedResponse<T>
    {
        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        public int TotalItems { get; set; }

        public int TotalPages { get; set; }

        public bool Success { get; set; }

        public string Code { get; set; }

        public string Message { get; set; }

        public T? Data { get; set; }
    }
}