using AuthJwtRefresh.Context;
using AuthJwtRefresh.Contracts;
using AuthJwtRefresh.Models;
using Microsoft.EntityFrameworkCore;

namespace AuthJwtRefresh.Repositories
{
    public class JwtRepository : IJwtRepository
    {
        private readonly AppDbContext _db;

        public JwtRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task AddAsync(RefreshToken refreshToken)
        {
            _db.RefreshTokens.Add(refreshToken);
            await _db.SaveChangesAsync();
        }

        public async Task<RefreshToken> TryGetTokenAsync(string refreshToken)
        {
            var result = await _db.RefreshTokens.FirstOrDefaultAsync(t => refreshToken.Equals(t.Token));
            return result;
        }
    }
}
