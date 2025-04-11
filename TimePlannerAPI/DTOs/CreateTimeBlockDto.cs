

using TimePlannerAPI.Models;
using TimePlannerAPI.Repositories;
using System.ComponentModel.DataAnnotations;

namespace TimePlannerAPI.DTOs
{


    /// <summary>
    /// Data Transfer Object for creating a new TimeBlock
    /// </summary>
    public class CreateTimeBlockDto
    {
        /// <summary>
        /// Title of the time block (required)
        /// </summary>
        /// <example>Team Meeting</example>
        [Required(ErrorMessage = "Title is required")]
            [StringLength(100, MinimumLength = 3, ErrorMessage = "Title must be between 3 and 100 characters")]
            public string Title { get; set; }

            /// <summary>
            /// Description of the time block (optional)
            /// </summary>
            /// <example>Weekly sprint planning meeting</example>
            [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
            public string? Description { get; set; }

            /// <summary>
            /// Start time of the time block (required)
            /// </summary>
            /// <example>2023-08-15T09:00:00</example>
            [Required(ErrorMessage = "Start time is required")]
            [DataType(DataType.DateTime)]
            public DateTime StartTime { get; set; }

            /// <summary>
            /// End time of the time block (required)
            /// </summary>
            /// <example>2023-08-15T10:00:00</example>
            [Required(ErrorMessage = "End time is required")]
            [DataType(DataType.DateTime)]
            public DateTime EndTime { get; set; }

            /// <summary>
            /// Color code for UI representation (hex format)
            /// </summary>
            /// <example>#4287f5</example>
            [RegularExpression("^#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{3})$", ErrorMessage = "Invalid color hex code")]
            public string Color { get; set; } = "#4287f5"; // Default color

            /// <summary>
            /// Indicates if this time block is completed (default: false)
            /// </summary>
            /// <example>false</example>
            public bool IsCompleted { get; set; } = false;

            /// <summary>
            /// ID of the schedule this time block belongs to (required)
            /// </summary>
            /// <example>3fa85f64-5717-4562-b3fc-2c963f66afa6</example>
            [Required(ErrorMessage = "Schedule ID is required")]
            public Guid ScheduleId { get; set; }
        }
    }



