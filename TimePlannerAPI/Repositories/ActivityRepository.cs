using Microsoft.EntityFrameworkCore;
//using System.Diagnostics;
using TimePlannerAPI.Data;
using TimePlannerAPI.DTOs;
using TimePlannerAPI.Models;

namespace TimePlannerAPI.Repositories
{
    public class ActivityRepository : IActivityRepository
    {


        // Repositories/ActivityRepository.cs


        //public class ActivityRepository : IActivityRepository
        //{

        private readonly timePlannerDbContext _context;
        private readonly ILogger<ActivityRepository> _logger;
        public ActivityRepository(
            timePlannerDbContext context,
            ILogger<ActivityRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Activity?> GetByIdAsync(Guid id)
        {
            return await _context.Activities
                .Include(a => a.User)
                .Include(a => a.Schedule)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<IEnumerable<Activity>> GetAllByUserIdAsync(Guid userId)
        {
            return await _context.Activities
                .Where(a => a.UserId == userId)
                .OrderByDescending(a => a.Priority)
                .ThenBy(a => a.Deadline)
                .ToListAsync();
        }

        public async Task<IEnumerable<Activity>> GetByScheduleIdAsync(Guid scheduleId)
        {
            return await _context.Activities
                .Where(a => a.ScheduleId == scheduleId)
                .OrderBy(a => a.CreatedAt)
                .ToListAsync();
        }

        public async Task<Activity> CreateAsync(Activity activity)
        {
            await _context.Activities.AddAsync(activity);
            await _context.SaveChangesAsync();
            return activity;
        }

        public async Task<Activity?> UpdateAsync(Guid id, UpdateActivityDto updateDto)
        {
            var existingActivity = await _context.Activities.FindAsync(id);
            if (existingActivity == null)
            {
                return null;
            }

            // Update properties if they're provided in the DTO
            if (!string.IsNullOrEmpty(updateDto.Name))
            {
                existingActivity.Name = updateDto.Name;
            }

            if (updateDto.Description != null)
            {
                existingActivity.Description = updateDto.Description;
            }

            if (updateDto.EstimatedDuration.HasValue)
            {
                existingActivity.EstimatedDuration = updateDto.EstimatedDuration.Value;
            }

            if (updateDto.Priority.HasValue)
            {
                existingActivity.Priority = updateDto.Priority.Value;
            }

            if (updateDto.Deadline.HasValue)
            {
                existingActivity.Deadline = updateDto.Deadline.Value;
            }

            if (updateDto.ScheduleId.HasValue)
            {
                existingActivity.ScheduleId = updateDto.ScheduleId.Value;
            }

            existingActivity.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return existingActivity;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var activity = await _context.Activities.FindAsync(id);
            if (activity == null)
            {
                return false;
            }

            _context.Activities.Remove(activity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _context.Activities.AnyAsync(a => a.Id == id);
        }
    }
}

