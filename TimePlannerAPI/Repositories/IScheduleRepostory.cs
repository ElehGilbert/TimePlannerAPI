using TimePlannerAPI.Models;

namespace TimePlannerAPI.Repositories
{
    public interface IScheduleRepostory
    {
        Task<Schedule> GetScheduleById(Guid id);
        Task<IEnumerable<Schedule>> GetUserSchedulesAsync(Guid userId);
        Task AddScheduleAsync(Schedule schedule);
        Task UpdateScheduleAsync(Schedule schedule);
        Task DeleteScheduleAsync(Guid id);
    }






    //Repository 
}
