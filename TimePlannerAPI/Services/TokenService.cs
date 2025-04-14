// Services/TokenService.cs
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
//using TimePlanner.Api.DTOs;
//using TimePlanner.Api.Entities;



using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using TimePlannerAPI.DTOs;
using TimePlannerAPI.Models;
using TimePlannerAPI.Repositories;

namespace TimePlannerAPI.Services
{
   // public class TokenService
   // {


        public interface ITokenService
        {
            Task<AuthResponseDto> GenerateTokens(User user);
        }

        public class TokenService : ITokenService
        {
            private readonly IConfiguration _configuration;
            private readonly IUserRepository _userRepository;

            public TokenService(IConfiguration configuration, IUserRepository userRepository)
            {
                _configuration = configuration;
                _userRepository = userRepository;
            }

            public async Task<AuthResponseDto> GenerateTokens(User user)
            {
                var token = GenerateJwtToken(user);
                var refreshToken = GenerateRefreshToken();

                user.RefreshToken = refreshToken;
                user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);
                await _userRepository.UpdateAsync(user);

                return new AuthResponseDto
                {
                    Token = token,
                    RefreshToken = refreshToken,
                    User = new UserDTO
                    {
                        Id = user.Id,
                        Email = user.Email,
                        FullName = user.FullName,
                        CreatedAt = user.CreatedAt,
                        UpdatedAt = user.UpdatedAt
                    }
                };
            }

            private string GenerateJwtToken(User user)
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Secret"]);

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[]
                    {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Name, user.FullName)
                }),
                    Expires = DateTime.UtcNow.AddMinutes(15),
                    SigningCredentials = new SigningCredentials(
                        new SymmetricSecurityKey(key),
                        SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                return tokenHandler.WriteToken(token);
            }

            private static string GenerateRefreshToken()
            {
                var randomNumber = new byte[32];
                using var rng = RandomNumberGenerator.Create();
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }
    }
//}
//}
