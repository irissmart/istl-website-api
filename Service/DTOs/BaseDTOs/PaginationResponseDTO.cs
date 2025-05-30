namespace Service.DTOs.BaseDTOs
{
    public class PaginationResponseDTO<T>
    {
        public T? Data { get; set; }
        public int TotalPages { get; set; }
    }
}