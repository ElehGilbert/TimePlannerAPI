using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TimePlannerAPI.Models
{
  
        // Entities/Activity.cs

        public class Activity
        {
            [Key]
            public Guid Id { get; set; }

            [Required]
            [MaxLength(100)]
            public string Name { get; set; }

            [MaxLength(500)]
            public string? Description { get; set; }

            [Required]
            public TimeSpan EstimatedDuration { get; set; }

            [Range(1, 5)]
            public int Priority { get; set; } = 1;

            public DateTime? Deadline { get; set; }

            [Required]
            [ForeignKey("User")]
            public Guid UserId { get; set; }
            public User User { get; set; }

            [ForeignKey("Schedule")]
            public Guid? ScheduleId { get; set; }
            public Schedule? Schedule { get; set; }

            public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
            public DateTime? UpdatedAt { get; set; }
        }
    }

