using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimePlannerAPI.Models
{
    public class Schedule
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        public DateTime CreatedDate { get; set; }=DateTime.UtcNow;

        [ForeignKey("UserId")]// Specifies the foriegn key relationship
        public Guid UserId { get; set; }

        //Navigation Property
        public User Users { get; set; }
        public ICollection<TimeBlock> TimeBlocks { get; set; }
    }


}
