using AuthJwtRefresh.Contracts;
using AuthJwtRefresh.DTO;
using AuthJwtRefresh.Models;
using AutoMapper;
using System.Security.Claims;

namespace AuthJwtRefresh.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IJwtManager _jwtManager;
        private readonly IMapper _mapper;

        public AccountService(IAccountRepository accountRepository, IJwtManager jwtManager, IMapper mapper)
        {
            _accountRepository = accountRepository;
            _jwtManager = jwtManager;
            _mapper = mapper;
        }

        public async Task<LoginResponse> AuthenticateAsync(LoginRequest request)
        {
            var model = await _accountRepository.FindByEmailAsync(request.Email);
            if (model == null)
            {
                throw new Exception("Invalid Email or password");
            }
            var check = BCrypt.Net.BCrypt.Verify(request.Password, model.PasswordHash); 

            

            //claims (Name + role)
            var role = model.Role;
            var claims = new[]
            {
                new Claim("Email", model.Email),
                new Claim(ClaimTypes.Role, role),
                new Claim("Id", model.Id.ToString())
            };
            var result = await _jwtManager.GenerateTokensAsync(claims);
            return result;
        }

        public async Task<Guid> RegisterAsync(RegisterRequest request)
        {
            var model = await _accountRepository.FindByEmailAsync(request.Email);
            if(model != null)
            {
                throw new Exception("Account with this Email is already exist");
            }

            var account = _mapper.Map<Account>(request);
            account.Role = "User";
            account.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

            var result = await _accountRepository.AddAsync(account);

            return result;
        }
    }
}
