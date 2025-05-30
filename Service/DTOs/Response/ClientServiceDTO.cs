namespace Service.DTOs.Response
{
    public class ClientServiceDTO
    {
        public int Id { get; set; }

        public int ClientServiceCategoryId { get; set; }

        public string ServiceName { get; set; } = null!;

        public string Description { get; set; } = null!;

        public string? ImagePath { get; set; }
    }
}
