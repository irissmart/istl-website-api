namespace Service.DTOs.Response
{
    public class JobCategoryDTO
    {
        public int Id { get; set; }

        public string JobCategoryName { get; set; } = null!;

        public string? ImagePath { get; set; }
    }
}
