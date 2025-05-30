namespace Service.DTOs.Response
{
    public class ClientServiceCategoryDTO
    {
        public int Id { get; set; }
        public string ServiceCategoryName { get; set; } = null!;

        public string Description { get; set; } = null!;

        public string? ImagePath { get; set; }
    }
}
