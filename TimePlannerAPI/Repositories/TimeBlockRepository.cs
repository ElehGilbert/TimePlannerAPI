using Microsoft.EntityFrameworkCore;
using TimePlannerAPI.Data;
using TimePlannerAPI.Models;

namespace TimePlannerAPI.Repositories
{
    public class TimeBlockRepository : ITimeBlockRepository
    {

        // TimeBlockRepository.cs

        private readonly timePlannerDbContext _context;
        private readonly ILogger<TimeBlockRepository> _logger;

        public TimeBlockRepository(
            timePlannerDbContext context,
            ILogger<TimeBlockRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<TimeBlock?> GetByIdAsync(Guid id)
        {
            try
            {
                return await _context.TimeBlocks
                    .AsNoTracking()//
                    .FirstOrDefaultAsync(tb => tb.Id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting time block by ID: {Id}", id);
                throw;
            }
        }

        public async Task<IEnumerable<TimeBlock>> GetByScheduleIdAsync(Guid scheduleId)
        {
            try
            {
                return await _context.TimeBlocks
                    .AsNoTracking()
                    .Where(tb => tb.ScheduleId == scheduleId)
                    .OrderBy(tb => tb.StartTime)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting time blocks for schedule: {ScheduleId}", scheduleId);
                throw;
            }
        }

        public async Task<Guid> CreateAsync(TimeBlock timeBlock)
        {
            try
            {
                await _context.TimeBlocks.AddAsync(timeBlock);
                await _context.SaveChangesAsync();
                return timeBlock.Id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating time block");
                throw;
            }
        }

        public async Task UpdateAsync(TimeBlock timeBlock)
        {
            try
            {
                _context.TimeBlocks.Update(timeBlock);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating time block: {Id}", timeBlock.Id);
                throw;
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            try
            {
                var timeBlock = await _context.TimeBlocks.FindAsync(id);
                if (timeBlock != null)
                {
                    _context.TimeBlocks.Remove(timeBlock);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting time block: {Id}", id);
                throw;
            }
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            try
            {
                return await _context.TimeBlocks
                    .AnyAsync(tb => tb.Id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking if time block exists: {Id}", id);
                throw;
            }
        }
    }
}



