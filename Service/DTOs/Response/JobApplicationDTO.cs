namespace Service.DTOs.Response
{
    public class JobApplicationDTO
    {
        public int Id { get; set; }
        public int JobId { get; set; }
        public string JobName { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Contact { get; set; } = null!;
        public DateTime CreatedOn { get; set; }
    }
}
