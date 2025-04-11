using System.ComponentModel.DataAnnotations;

namespace TimePlannerAPI.DTOs
{
    /// <summary>
    /// Data Transfer object for Updating a schedule
    /// </summary>
    public class UpdateScheduleDto
    {

        ///<summary>
        ///Update name of Schedule
        /// </summary>
        ///<example>Updated Work Schedule</example>
        [StringLength(100,MinimumLength = 3)]
        public string? Name { get; set; }

        ///<summary>
        ///Update description
        /// </summary>
        /// <example>Updated weekly schedule</example>
        [StringLength(500)]
        public string? Description { get; set; }


        ///<summary>
        ///Updated description
        /// </summary>
        /// <example>fasle</example>
        public bool? IsActive { get; set; }




        ///<summary>
        ///Updated color code
        /// </summary>
        /// <example>#ff5733</example>
        [RegularExpression("^#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{3})$")]
        public string? color { get; set; }




        ///<summary>
        ///Updated tags list
        /// </summary>
        /// <example>["work","Weekly"]</example>
        public List<string>? Tags { get; set; }
    }
}
