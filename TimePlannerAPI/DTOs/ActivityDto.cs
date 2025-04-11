namespace TimePlannerAPI.DTOs
{
    public class ActivityDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public TimeSpan EstimatedDuration { get; set; }
        public int Priority { get; set; }
        public DateTime? Deadline { get; set; }
        public Guid UserId { get; set; }
        public Guid? ScheduleId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
