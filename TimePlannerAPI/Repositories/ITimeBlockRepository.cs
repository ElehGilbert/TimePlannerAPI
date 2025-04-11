using TimePlannerAPI.Models;

namespace TimePlannerAPI.Repositories
{
    public interface ITimeBlockRepository
    {

        Task<Guid> CreateAsync(TimeBlock timeBlock);
        Task DeleteAsync(Guid id);
        Task<bool> ExistsAsync(Guid id);
        Task<TimeBlock?> GetByIdAsync(Guid id);
        Task<IEnumerable<TimeBlock>> GetByScheduleIdAsync(Guid scheduleId);
        Task UpdateAsync(TimeBlock timeBlock);
    }
}