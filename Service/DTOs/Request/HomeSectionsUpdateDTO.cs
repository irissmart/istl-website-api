using Microsoft.AspNetCore.Http;

namespace Service.DTOs
{
    public class HomeSectionsUpdateDTO
    {
        public int PageId { get; set; }
        public IEnumerable<HomeSectionUpdateDTO> Sections { get; set; } = new List<HomeSectionUpdateDTO>();
    }

    public class HomeSectionUpdateDTO
    {
        public int Id { get; set; }
        public string SectionName { get; set; } = string.Empty;
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? ButtonText { get; set; }

        public IEnumerable<AnalyticUpdateDTO> Analytics { get; set; } = new List<AnalyticUpdateDTO>();
        public IEnumerable<ServiceUpdateDTO> Services { get; set; } = new List<ServiceUpdateDTO>();
        public IEnumerable<DetailedServiceUpdateDTO> DetailedServices { get; set; } = new List<DetailedServiceUpdateDTO>();
        public IEnumerable<StepUpdateDTO> Steps { get; set; } = new List<StepUpdateDTO>();
    }

    public class AnalyticUpdateDTO
    {
        public int Id { get; set; }
        public string Number { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
    }

    public class ServiceUpdateDTO
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
    }

    public class DetailedServiceUpdateDTO
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
    }

    public class StepUpdateDTO
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}
