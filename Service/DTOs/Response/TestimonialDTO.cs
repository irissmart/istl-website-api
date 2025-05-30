namespace Service.DTOs.Response
{
    public class TestimonialDTO
    {
        public int Id { get; set; }
        public string Comment { get; set; } = null!;

        public string ClientName { get; set; } = null!;

        public string ClientOccupation { get; set; } = null!;

        public string? ImagePath { get; set; }
    }
}
