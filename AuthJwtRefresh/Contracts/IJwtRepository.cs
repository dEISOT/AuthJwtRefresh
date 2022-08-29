using AuthJwtRefresh.Models;

namespace AuthJwtRefresh.Contracts
{
    public interface IJwtRepository
    {
        Task AddAsync(RefreshToken refreshToken);
        Task<RefreshToken> TryGetTokenAsync(string refreshToken);

    }
}
