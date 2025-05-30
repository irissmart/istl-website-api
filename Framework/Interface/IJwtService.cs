using System.Security.Claims;

namespace Framework.Interface
{
    public interface IJwtService
    {
        string GenerateJwtToken(IEnumerable<Claim> claims);

        string GenerateRefreshToken();
    }
}