using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TimePlannerAPI.DTOs
{
    // DTOs/UserDto.cs
    public class UserDTO
    {
        //public string Email { get; set; } = null!;


     


            public Guid Id { get; set; }

            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [StringLength(100)]
            public string FullName { get; set; }

            [JsonIgnore] // Never return password hash in DTO
            public string PasswordHash { get; set; }

            public DateTime CreatedAt { get; set; }
            public DateTime? UpdatedAt { get; set; }
        }

        public class RegisterUserDto
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [StringLength(100, MinimumLength = 6)]
            public string Password { get; set; }

            [Required]
            [StringLength(100)]
            public string FullName { get; set; }
        }

        public class LoginUserDto
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            public string Password { get; set; }
        }

        public class UpdateUserDto
        {
            [StringLength(100)]
            public string? FullName { get; set; }

            [EmailAddress]
            public string? Email { get; set; }

            [StringLength(100, MinimumLength = 6)]
            public string? Password { get; set; }
        }

        public class AuthResponseDto
        {
            public string Token { get; set; }
            public string RefreshToken { get; set; }
            public UserDTO User { get; set; }
        }
    }

