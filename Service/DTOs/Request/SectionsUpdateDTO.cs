using Microsoft.AspNetCore.Http;

namespace Service.DTOs
{
    public class SectionsUpdateDTO
    {
        public int PageId { get; set; }
        public IEnumerable<SectionUpdateDTO> Sections { get; set; } = new List<SectionUpdateDTO>();
    }

    public class SectionUpdateDTO
    {
        public int Id { get; set; }
        public string SectionName { get; set; } = string.Empty;
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? ButtonText { get; set; }

        // Validate proper image.
        public IFormFile? BackgroundImage { get; set; }
        public IFormFile? SectionImage { get; set; }
    }
}
