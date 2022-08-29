using AuthJwtRefresh.Contracts;
using AuthJwtRefresh.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace AuthJwtRefresh.Services
{
    public class JwtManager : IJwtManager
    {
        private readonly IConfiguration _configuration;
        private readonly IJwtRepository _jwtRepository;
        private readonly IAccountRepository _accountRepository;

        public JwtManager(IConfiguration configuration, IJwtRepository jwtRepository, IAccountRepository accountRepository)
        {
            _configuration = configuration;
            _jwtRepository = jwtRepository;
            _accountRepository = accountRepository;
        }

        public async Task<LoginResponse> GenerateTokensAsync(Claim[] claims)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            // Get secret phrase from configuration
            var secret = _configuration.GetValue<string>("JWT:Secret");
            // Get expiration minutes from configuration
            var JwtTokenExpiration = double.Parse(_configuration.GetValue<string>("JWT:JwtTokenExpiration"));
            // Encode secrets
            var key = Encoding.UTF8.GetBytes(secret);
            // Set JWT description using user id and user role
            var JwtToken = new JwtSecurityToken(
                _configuration.GetValue<string>("JWT:Issuer"),
                _configuration.GetValue<string>("JWT:Audience"),
                claims,
                expires: DateTime.UtcNow.AddMinutes(JwtTokenExpiration),
                signingCredentials : new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
                );
            var accessToken = new JwtSecurityTokenHandler().WriteToken(JwtToken);
            var value = claims.FirstOrDefault(c => c.Type == "Id").Value;
            var accountId = new Guid(value);
            var refreshToken = new RefreshToken
            {
                Token = GenerateRefreshTokenString(),
                ExpireAt = DateTime.UtcNow.AddDays(double.Parse(_configuration.GetValue<string>("JWT:RefreshTokenExpiration"))),
                AccountId = accountId
            };  
            await _jwtRepository.AddAsync(refreshToken);

            var response = new LoginResponse()
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken.Token,
            };
            return response;
        }

        private static string GenerateRefreshTokenString()
        {
            var randomNumber = new byte[32];
            using var randomNumberGenerator = RandomNumberGenerator.Create();
            randomNumberGenerator.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        private (ClaimsPrincipal, JwtSecurityToken) DecodeJwtToken(string token)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(token))
                {
                    throw new SecurityTokenException("Invalid token");
                }
                var key = Encoding.UTF8.GetBytes(_configuration.GetValue<string>("JWT:Secret"));
                var tokenHandler = new JwtSecurityTokenHandler();
                SecurityToken jwtToken;
                var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters()
                {
                    ValidateActor = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = _configuration["Jwt:Issuer"],
                    ValidAudience = _configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Secret"]))
                }, out jwtToken);
                return (principal, (JwtSecurityToken)jwtToken);
            }
            catch (Exception ex)
            {
                throw;
            }


        }

        public async Task<LoginResponse> RefreshTokenAsync(string refreshToken, string accessToken, DateTime time)
        {
            var (principal, jwtToken) = DecodeJwtToken(accessToken);
            var claims = principal.Claims.ToArray();
            RefreshToken existingRefreshToken = await _jwtRepository.TryGetTokenAsync(refreshToken);
            if(existingRefreshToken == null)
            {
                throw new SecurityTokenException("Invalid token");
            }
            //if (existingRefreshToken.)
            //{

            //}
            //if emails equals and expire time right
            var result = await GenerateTokensAsync(claims);

            return result;
        }
    }
}

































