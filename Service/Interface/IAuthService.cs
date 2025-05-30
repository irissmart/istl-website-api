using Framework.Model;
using Service.DTOs;

namespace Service.Interface
{
    public interface IAuthService
    {
        Task<(AuthResponse, int)> AuthenticateAsync(AuthRequest request);

        Task<AuthResponse> RefreshTokenAsync(RefreshTokenDTO request);
    }
}
