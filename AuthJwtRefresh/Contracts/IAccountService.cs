using AuthJwtRefresh.DTO;

namespace AuthJwtRefresh.Contracts
{
    public interface IAccountService
    {
        Task<LoginResponse> AuthenticateAsync(LoginRequest request);
        Task<Guid> RegisterAsync(RegisterRequest request);
    }
}
