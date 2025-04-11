using System.ComponentModel.DataAnnotations;

namespace TimePlannerAPI.DTOs
{
    /// <summary>
    /// Data Transfer Object for creating a new schedule
    /// </summary>
    public class CreateScheduleDto
    {
        /// <summary>
        /// Name of the Schedule
        /// </summary>
        /// <example>My Schedule</example>
        [Required]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Ops Schedule name must be between 3 and 100 characters")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Description of the Schedule
        /// </summary>
        /// <example>My week work Schedule including meetings and tasks</example>
        [StringLength(500, ErrorMessage = "Ops Schedule description must be less than 500 characters")]
        public string? Description { get; set; }




        /// <summary>
        /// Color code for UI representation(hex format)
        /// </summary>
        /// <example>#FF5733</example>
        [RegularExpression("^#([0-9A-Fa-f]{6}|[0-9A-Fa-f]{3})$", ErrorMessage = "Invalid color code format")]
        public string Color { get; set; } = "#FF5733";




        ///<summary>
        ///Initial tags for the schedule
        /// </summary>
        /// <example>["work","weekly"]</example>
        public List<string> Tags { get; set; } = new();
    }
}
