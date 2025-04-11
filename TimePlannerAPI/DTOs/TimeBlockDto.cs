using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TimePlannerAPI.DTOs
{


    /// <summary>
    /// Data Transfer Object for TimeBlock information
    /// </summary>
    public class TimeBlockDto
    {
        /// <summary>
        /// Unique identifier for the time block
        /// </summary>
        /// <example>3fa85f64-5717-4562-b3fc-2c963f66afa6</example>
        public Guid Id { get; set; }

            /// <summary>
            /// Title of the time block
            /// </summary>
            /// <example>Team Meeting</example>
            [Required]
            [StringLength(100, MinimumLength = 3, ErrorMessage = "Title must be between 3 and 100 characters")]
            public string Title { get; set; }

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
            [Required]
            [DataType(DataType.DateTime)]
            public DateTime StartTime { get; set; }

            /// <summary>
            /// End time of the time block
            /// </summary>
            /// <example>2023-08-15T10:00:00</example>
            [Required]
            [DataType(DataType.DateTime)]
            public DateTime EndTime { get; set; }

            /// <summary>
            /// Duration in minutes (calculated)
            /// </summary>
            /// <example>60</example>
            public double DurationMinutes => (EndTime - StartTime).TotalMinutes;

            /// <summary>
            /// Color code for UI representation (hex format)
            /// </summary>
            /// <example>#4287f5</example>
            [RegularExpression("^#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{3})$", ErrorMessage = "Invalid color hex code")]
            public string Color { get; set; } = "#4287f5";

            /// <summary>
            /// Indicates if this time block is completed
            /// </summary>
            /// <example>false</example>
            public bool IsCompleted { get; set; }

            /// <summary>
            /// Date and time when the time block was created
            /// </summary>
            /// <example>2023-08-14T14:30:00Z</example>
            [DataType(DataType.DateTime)]
            public DateTime CreatedAt { get; set; }

            /// <summary>
            /// Date and time when the time block was last modified
            /// </summary>
            /// <example>2023-08-14T15:45:00Z</example>
            [DataType(DataType.DateTime)]
            public DateTime? UpdatedAt { get; set; }

            /// <summary>
            /// ID of the schedule this time block belongs to
            /// </summary>
            /// <example>3fa85f64-5717-4562-b3fc-2c963f66afa6</example>
            [Required]
            public Guid ScheduleId { get; set; }

            /// <summary>
            /// ID of the user who created this time block
            /// </summary>
            /// <example>3fa85f64-5717-4562-b3fc-2c963f66afa6</example>
            [JsonIgnore] // Typically excluded from serialization
            public Guid UserId { get; set; }
        }
    }


