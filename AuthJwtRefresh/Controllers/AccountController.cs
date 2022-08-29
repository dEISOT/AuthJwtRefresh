using AuthJwtRefresh.Contracts;
using AuthJwtRefresh.DTO;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthJwtRefresh.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IJwtManager _jwtManager;

        public AccountController(IAccountService accountService, IJwtManager jwtManager)
        {
            _accountService = accountService;
            _jwtManager = jwtManager;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            //validation for credentials

            var result = await _accountService.AuthenticateAsync(request);
            return Ok(result);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "User, Admin")]
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                var accessToken = await HttpContext.GetTokenAsync("Bearer", "access_token");
                var jwtResult = await _jwtManager.RefreshTokenAsync(request.RefreshToken, accessToken, DateTime.Now);

                return Ok(jwtResult);
            }
            catch (Exception)
            {

                throw;
            }


        }
        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var accountId = await _accountService.RegisterAsync(request);

            return Ok(accountId);
            
        }
    }
}
