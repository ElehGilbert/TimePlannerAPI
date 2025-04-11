using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TimePlannerAPI.DTOs
{
    /// <summary>
    /// Data Transfer Object for Schedule  information
    /// </summary>
    public class ScheduleDto
    {
        /// <summary>
        /// Unique identifier for the schedule
        /// 
        /// </summary>
        /// <example>3fa85f64-5717-4562-b3fc-2c963f66afa6</example>
        
        public Guid Id { get; set; }



        /// <summary>
        /// Name of the Schedule
        /// </summary>
        /// <example>My Schedule</example>
        [Required]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Ops Schedule name must be between 3 and 100 characters")]
        public string Name { get; set; } = string.Empty;// 




        /// <summary>
        /// Description of the Schedule
        /// </summary>
        /// <example>My week work Schedule including meetings and tasks</example>
        [StringLength(500, ErrorMessage = "Ops Schedule description must be less than 500 characters")]
        public string Description { get; set; }



        /// <summary>
        /// Date and time when the schedule was created
        /// </summary>
        /// <example>2025-04-10T10:30:002</example>
        [DataType(DataType.DateTime)]
        public DateTime Date { get; set; }



        /// <summary>
        /// Date and time when the schedule was last modified
        /// </summary>
        /// <example>2025-04-10T10:30:002</example>

        [DataType(DataType.DateTime)]
        public DateTime? Time { get; set; }





        ///<summary>
        ///Indicates if this schedule is currently active
        /// </summary>
        /// <example>true</example>
        public bool IsActive { get; set; } = true;





        /// <summary>
        /// The total number of time blocks in this schedule
        /// </summary>
        /// <example>5</example>
        public double TotalDurationMinutes { get; set; }




        ///<summary>
        ///Color code for UI representation (hex format)
        ///</summary>
        ///<example>#4287f5</example>
        [RegularExpression("^#([0-9A-Fa-f]{6}|[0-9A-Fa-f]{3})$", ErrorMessage = "Invalid color code format")]
        public string Color { get; set; } = "#4287f5";





        ///<summary>
        ///List of time blocks associated with this schedule
        /// </summary>
        //[ValidateComplexType]//validates nested objects
        public List<TimeBlockDto> TimeBlocks { get; set; } = new List<TimeBlockDto>();





        ///<summary>
        ///Tags for Categorizing schedules
        /// </summary>
        ///<example>["Work", "Weekly"]</example>
        public List<string> Tags { get; set; } = new();




        ///<summary>
        ///The Owners user ID
        /// </summary>
        ///<example>3fa85f64-5717-4562-b3fc-2c963f66afa6</example>
        [JsonIgnore]//Typically exclude this from serialization
        public Guid UserId { get; set; }



    }
}
