using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimePlannerAPI.Models
{
    public class TimeBlock
    {

        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Title
        { get; set; }


        public string Description { get; set; }


        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        public DateTime EndTime { get; set; }

        [ForeignKey("ScheduleId")] // Specifies the foreign key relationship
        public Guid ScheduleId { get; set; }

        //Navigation Property
            public Schedule Schedule { get; set; }
    }

}
