using AuthJwtRefresh.Models;

namespace AuthJwtRefresh.Contracts
{
    public interface IAccountRepository
    {
        Task<Account> FindByEmailAsync(string email);
        Task<Guid> AddAsync(Account account);
    }
}
