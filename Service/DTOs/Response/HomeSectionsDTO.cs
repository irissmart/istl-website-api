namespace Service.DTOs.Response
{
    public class HomeSectionsDTO
    {
        public int PageId { get; set; }
        public IEnumerable<HomeSectionDTO> Sections { get; set; } = new List<HomeSectionDTO>();
    }

    public class HomeSectionDTO
    {
        public int Id { get; set; }
        public string SectionName { get; set; } = string.Empty;
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? ButtonText { get; set; }
        public string? BackgroundImagePath { get; set; }
        public string? SectionImagePath { get; set; }
        public IEnumerable<AnalyticDTO> Analytics { get; set; } = new List<AnalyticDTO>();
        public IEnumerable<ServiceDTO> Services { get; set; } = new List<ServiceDTO>();
        public IEnumerable<DetailedServiceDTO> DetailedServices { get; set; } = new List<DetailedServiceDTO>();
        public IEnumerable<StepDTO> Steps { get; set; } = new List<StepDTO>();
    }

    public class AnalyticDTO
    {
        public int Id { get; set; }
        public string Number { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
    }

    public class ServiceDTO
    {
        public int Id { get; set; }
        public int SectionId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? LogoPath { get; set; }
    }

    public class DetailedServiceDTO
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? IconPath { get; set; }
        public string? Description { get; set; }
    }

    public class StepDTO
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}
