using AmazeCareWebApi.Models;

namespace AmazeCareWebApi.Services.Interface
{
    public interface IJwtTokenService
    {
        string GenerateToken(User user, out DateTime expiresAt);
    }
}
