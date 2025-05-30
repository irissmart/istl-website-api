namespace Service.DTOs.Response
{
    public class SectionsDTO
    {
        public int PageId { get; set; }
        public IEnumerable<SectionDTO> Sections { get; set; } = new List<SectionDTO>();
    }

    public class SectionDTO
    {
        public int Id { get; set; }
        public string SectionName { get; set; } = string.Empty;
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? ButtonText { get; set; }
        public string? BackgroundImagePath { get; set; }
        public string? SectionImagePath { get; set; }
    }
}
