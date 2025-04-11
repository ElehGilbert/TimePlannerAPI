using System.ComponentModel.DataAnnotations;

namespace TimePlannerAPI.DTOs
{


    /// <summary>
    /// Data Transfer Object for updating a TimeBlock
    /// </summary>
    public class UpdateTimeBlockDto
    {
        /// <summary>
        /// Title of the time block
        /// </summary>
        /// <example>Team Meeting</example>
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Title must be between 3 and 100 characters")]
            public string? Title { get; set; }

            /// <summary>
            /// Description of the time block (optional)
            /// </summary>
            /// <example>Weekly sprint planning meeting</example>
            [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
            public string? Description { get; set; }

            /// <summary>
            /// Start time of the time block
            /// </summary>
            /// <example>2023-08-15T09:00:00</example>
            [DataType(DataType.DateTime)]
            public DateTime? StartTime { get; set; }

            /// <summary>
            /// End time of the time block
            /// </summary>
            /// <example>2023-08-15T10:00:00</example>
            [DataType(DataType.DateTime)]
            public DateTime? EndTime { get; set; }

            /// <summary>
            /// Color code for UI representation (hex format)
            /// </summary>
            /// <example>#4287f5</example>
            [RegularExpression("^#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{3})$", ErrorMessage = "Invalid color hex code")]
            public string? Color { get; set; }

            /// <summary>
            /// Indicates if this time block is completed
            /// </summary>
            /// <example>false</example>
            public bool? IsCompleted { get; set; }
        }
    }
