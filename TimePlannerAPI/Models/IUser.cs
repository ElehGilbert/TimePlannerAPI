
namespace TimePlannerAPI.Models
{
    public interface IUser
    {
        string Email { get; set; }
        string FullName { get; set; }
        Guid Id { get; set; }
        byte[] PasswordHash { get; set; }
        byte[] PasswordSalt { get; set; }
        ICollection<Schedule> Schedules { get; set; }
    }
}