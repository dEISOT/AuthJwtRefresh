using AuthJwtRefresh.Context;
using AuthJwtRefresh.Contracts;
using AuthJwtRefresh.Models;
using Microsoft.EntityFrameworkCore;

namespace AuthJwtRefresh.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly AppDbContext _db;
        public AccountRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<Guid> AddAsync(Account account)
        {
            await _db.Accounts.AddAsync(account);
            await _db.SaveChangesAsync();
            return account.Id;
        }

        public Task<Account> FindByEmailAsync(string email)
        {
            var response = _db.Accounts.FirstOrDefaultAsync(a => string.Equals(a.Email, email));
            return response;
        }
    }
}
