using System.ComponentModel.DataAnnotations;

namespace TimePlannerAPI.DTOs
{
    public class UpdateActivityDto
    {
        [MaxLength(100)]
        public string? Name { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }

        public TimeSpan? EstimatedDuration { get; set; }

        [Range(1, 5)]
        public int? Priority { get; set; }

        public DateTime? Deadline { get; set; }

        public Guid? ScheduleId { get; set; }
    }
}
