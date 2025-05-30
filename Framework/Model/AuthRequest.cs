namespace Framework.Model
{
    public class AuthRequest
    {
        public string AuthId { get; set; }
        public string Password { get; set; }
        public int RoleId { get; set; }
    }
}