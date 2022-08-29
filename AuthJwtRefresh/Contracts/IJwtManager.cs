using System.Security.Claims;

namespace AuthJwtRefresh.Contracts
{
    public interface IJwtManager
    {
        Task<LoginResponse> GenerateTokensAsync(Claim[] claims);
        Task<LoginResponse> RefreshTokenAsync(string refrshToken, string accessToken, DateTime time);
    }
}
