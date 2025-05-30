namespace Framework.Model
{
    public class AuthResponse
    {
        public string JwtToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime JwtTokenExpires { get; set; }
        public DateTime RefreshTokenExpires { get; set; }
    }
}