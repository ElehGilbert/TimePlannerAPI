using TimePlannerAPI.DTOs;
using TimePlannerAPI.Models;

namespace TimePlannerAPI.Repositories
{
    public interface IActivityRepository
    {
        Task<Activity> CreateAsync(Activity activity);
        Task<bool> DeleteAsync(Guid id);
        Task<bool> ExistsAsync(Guid id);
        Task<IEnumerable<Activity>> GetAllByUserIdAsync(Guid userId);
        Task<Activity?> GetByIdAsync(Guid id);
        Task<IEnumerable<Activity>> GetByScheduleIdAsync(Guid scheduleId);
        Task<Activity?> UpdateAsync(Guid id, UpdateActivityDto updateDto);
    }
}