using System.ComponentModel.DataAnnotations;

namespace TimePlannerAPI.Models
{
    public class User
    {
        [Key] //Marks this as the primary key
        public Guid Id { get; set; }

        [Required]//Ensure this field never null in the database
        [MaxLength(100)]//Limit the size of the database column
        public string Email { get; set; }

        [Required]
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }

        [MaxLength(100)]
        public string FullName { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
        //Navigation property for related schedules
        public ICollection<Schedule> Schedules { get; set; }



        public string RefreshToken { get; set; } // Added property these properties are necessary for managing refresh tokens
        public DateTime RefreshTokenExpiry { get; set; }// Added property these properties are necessary for managing refresh tokens
        

        }
}
