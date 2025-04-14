
// Services/AuthService.cs
using System.Security.Cryptography;
using System.Text;
using TimePlannerAPI.DTOs;
using TimePlannerAPI.Models;
using TimePlannerAPI.Repositories;

namespace TimePlannerAPI.Services
{
   
        public interface IAuthService
        {
            Task<AuthResult> RegisterAsync(RegisterUserDto registerDto);
            Task<AuthResult> LoginAsync(LoginUserDto loginDto);
            Task<AuthResult> RefreshTokenAsync(string refreshToken);
        }

        public class AuthService : IAuthService
        {
            private readonly IUserRepository _userRepository;
            private readonly IConfiguration _configuration;
            private readonly ITokenService _tokenService;

            public AuthService(
                IUserRepository userRepository,
                IConfiguration configuration,
                ITokenService tokenService)
            {
                _userRepository = userRepository;
                _configuration = configuration;
                _tokenService = tokenService;
            }

            public async Task<AuthResult> RegisterAsync(RegisterUserDto registerDto)
            {
                if (await _userRepository.GetByEmailAsync(registerDto.Email) != null)
                {
                    return AuthResult.Fail("Email already in use");
                }

                CreatePasswordHash(registerDto.Password, out var passwordHash, out var passwordSalt);

                var user = new User
                {
                    Email = registerDto.Email,
                    FullName = registerDto.FullName,
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt,
                    CreatedAt = DateTime.UtcNow
                };

                await _userRepository.CreateAsync(user);

                var authResponse = await _tokenService.GenerateTokens(user);
                return AuthResult.success(authResponse);
            }

            public async Task<AuthResult> LoginAsync(LoginUserDto loginDto)
            {
                var user = await _userRepository.GetByEmailAsync(loginDto.Email);
                if (user == null)
                {
                    return AuthResult.Fail("User not found");
                }

                if (!VerifyPasswordHash(loginDto.Password, user.PasswordHash, user.PasswordSalt))
                {
                    return AuthResult.Fail("Invalid credentials");
                }

                var authResponse = await _tokenService.GenerateTokens(user);
                return AuthResult.success(authResponse);
            }

            public async Task<AuthResult> RefreshTokenAsync(string refreshToken)
            {
                var user = await _userRepository.GetByRefreshTokenAsync(refreshToken);
                if (user == null)
                {
                    return AuthResult.Fail("Invalid refresh token");
                }

                if (user.RefreshTokenExpiry < DateTime.UtcNow)
                {
                    return AuthResult.Fail("Refresh token expired");
                }

                var authResponse = await _tokenService.GenerateTokens(user);
                return AuthResult.success(authResponse);
            }

        // 
            private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
            {
                using var hmac = new HMACSHA512();
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }

            private static bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
            {
                using var hmac = new HMACSHA512(passwordSalt);
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }

    public class AuthResult
    {
        public bool Success { get; set; } //Ensure this is a property with both getter  and setter
        public string Message { get; set; }
        public AuthResponseDto AuthResponse { get; set; }

        public static AuthResult success(AuthResponseDto authResponse) //change this to small caps so Success isn't read as a method
        {
            

            return new AuthResult { Success = true, AuthResponse = authResponse };
        }
        public static AuthResult Fail(string message)
        {
            return new AuthResult { Success = false, Message = message };
        }
    }
}
        
