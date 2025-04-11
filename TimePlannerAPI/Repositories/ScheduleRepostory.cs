using Microsoft.EntityFrameworkCore;
using TimePlannerAPI.Data;
using TimePlannerAPI.Models;

namespace TimePlannerAPI.Repositories
{
    public class ScheduleRepostory : IScheduleRepostory
    {
        private readonly timePlannerDbContext _context;

        //Dependency injection of the DbContext
        public ScheduleRepostory(timePlannerDbContext context)
        {
            _context = context;
        }


        public async Task<Schedule> GetScheduleByIdAsync(Guid id)
        {
            //Include related TimeBlocks when fetching a schedule

            return await _context.Schedules
                .Include(s => s.TimeBlocks)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task AddScheduleAsync(Schedule schedule)
        {
            //Add a new schedule to the database(context)
            await _context.Schedules.AddAsync(schedule);
            //Save changes to the database
            await _context.SaveChangesAsync();
        }



        public async Task UpdateScheduleAsync(Schedule schedule)
        {
            //Update an existing schedule in the database
            _context.Schedules.Update(schedule);
            //Save changes to the database
            await _context.SaveChangesAsync();
        }



        public async Task DeleteScheduleAsync(Guid id)
        {
            //Find the schedule by ID
            var schedule = await _context.Schedules.FindAsync(id);
            if (schedule != null)
            {
                //Remove the schedule from the database
                _context.Schedules.Remove(schedule);
                //Save changes to the database
                await _context.SaveChangesAsync();
            }
        }

        public Task<Schedule> GetScheduleById(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Schedule>> GetUserSchedulesAsync(Guid userId)
        {
            throw new NotImplementedException();
        }
    }
}
