namespace Service.DTOs.Response
{
    public class ContactDTO
    {
        public int Id { get; set; }

        public string PhoneNo { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Address { get; set; } = null!;

        public string? BannerImage { get; set; }

        public IEnumerable<SocialLink> SocialLinks { get; set; } = new List<SocialLink>();
    }

    public class SocialLink
    {
        public int Id { get; set; }

        public string PlatformName { get; set; } = null!;

        public string Url { get; set; } = null!;
    }
}
