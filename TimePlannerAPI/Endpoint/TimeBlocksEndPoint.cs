using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OutputCaching;
using System.Security.Claims;
using TimePlannerAPI.DTOs;
using TimePlannerAPI.Models;
using TimePlannerAPI.Repositories;

namespace TimePlannerAPI.Endpoint
{
    public static class TimeBlocksEndPoint 
    {

        public static RouteGroupBuilder MapTimeBlocks(this RouteGroupBuilder group)
        {
            // Endpoint to get all timeblocks for a schedule
            group.MapGet("/", GetAll)
                .CacheOutput(c => c.Expire(TimeSpan.FromSeconds(30)).Tag("timeblocks-get"));

            // Endpoint to get timeblock by ID
            group.MapGet("/{id:guid}", GetByIdAsync);

            // Endpoint to create new timeblock
            group.MapPost("/", CreateAsync);

            // Endpoint to update timeblock
            group.MapPut("/{id:guid}", UpdateAsync);

            // Endpoint to delete timeblock
            group.MapDelete("/{id:guid}", DeleteAsync);

            return group;
        }

        static async Task<Ok<List<TimeBlockDto>>> GetAll(
            Guid scheduleId,
            ITimeBlockRepository repository,
            IScheduleRepostory scheduleRepository,
            IMapper mapper,
            ClaimsPrincipal user)
        {
            var userId = GetUserId(user);
            var schedule = await scheduleRepository.GetScheduleById(scheduleId);

            if (schedule is null || schedule.UserId != userId)
            {
                return TypedResults.Ok(new List<TimeBlockDto>());
            }

            var timeBlocks = await repository.GetByScheduleIdAsync(scheduleId);
            var timeBlockDtos = mapper.Map<List<TimeBlockDto>>(timeBlocks);
            return TypedResults.Ok(timeBlockDtos);
        }


        // Getting By ID
        static async Task<Results<Ok<TimeBlockDto>, NotFound>> GetByIdAsync(
            Guid id,
            ITimeBlockRepository repository,
            IScheduleRepostory scheduleRepository,
            IMapper mapper,
            ClaimsPrincipal user)
        {
            var timeBlock = await repository.GetByIdAsync(id);
            if (timeBlock is null)
            {
                return TypedResults.NotFound();
            }

            var userId = GetUserId(user);
            var schedule = await scheduleRepository.GetScheduleById(timeBlock.ScheduleId);

            if (schedule is null || schedule.UserId != userId)
            {
                return TypedResults.NotFound();
            }

            var timeBlockDto = mapper.Map<TimeBlockDto>(timeBlock);
            return TypedResults.Ok(timeBlockDto);
        }

        static async Task<Created<TimeBlockDto>> CreateAsync(
            Guid scheduleId,
            CreateTimeBlockDto createTimeBlockDto,
            ITimeBlockRepository repository,
            IScheduleRepostory scheduleRepository,
            IOutputCacheStore outputCacheStore,
            IMapper mapper,
            ClaimsPrincipal user)
        {
            var userId = GetUserId(user);
            var schedule = await scheduleRepository.GetScheduleById(scheduleId);

            if (schedule is null || schedule.UserId != userId)
            {
                return TypedResults.Created<TimeBlockDto?>("", null);
            }

            var timeBlock = mapper.Map<TimeBlock>(createTimeBlockDto);
            timeBlock.ScheduleId = scheduleId;

            await repository.CreateAsync(timeBlock);
            await outputCacheStore.EvictByTagAsync("timeblocks-get", default);

            var timeBlockDto = mapper.Map<TimeBlockDto>(timeBlock);
            return TypedResults.Created($"/schedules/{scheduleId}/timeblocks/{timeBlock.Id}", timeBlockDto);
        }

        static async Task<Results<NotFound, NoContent>> UpdateAsync(
            Guid id,
            UpdateTimeBlockDto updateTimeBlockDto,
            ITimeBlockRepository repository,
     IScheduleRepostory scheduleRepository,
            IOutputCacheStore outputCacheStore,
            IMapper mapper,
            ClaimsPrincipal user)
        {
            var timeBlock = await repository.GetByIdAsync(id);
            if (timeBlock is null)
            {
                return TypedResults.NotFound();
            }

            var userId = GetUserId(user);
            var schedule = await scheduleRepository.GetScheduleById(timeBlock.ScheduleId);

            if (schedule is null || schedule.UserId != userId)
            {
                return TypedResults.NotFound();
            }

            mapper.Map(updateTimeBlockDto, timeBlock);
            await repository.UpdateAsync(timeBlock);
            await outputCacheStore.EvictByTagAsync("timeblocks-get", default);

            return TypedResults.NoContent();
        }

        static async Task<Results<NotFound, NoContent>> DeleteAsync(
            Guid id,
            ITimeBlockRepository repository,
            IScheduleRepostory scheduleRepository,
            IOutputCacheStore outputCacheStore,
            ClaimsPrincipal user)
        {
            var timeBlock = await repository.GetByIdAsync(id);
            if (timeBlock is null)
            {
                return TypedResults.NotFound();
            }

            var userId = GetUserId(user);
            var schedule = await scheduleRepository.GetScheduleById(timeBlock.ScheduleId);

            if (schedule is null || schedule.UserId != userId)
            {
                return TypedResults.NotFound();
            }

            await repository.DeleteAsync(id);
            await outputCacheStore.EvictByTagAsync("timeblocks-get", default);

            return TypedResults.NoContent();
        }

        private static Guid GetUserId(ClaimsPrincipal user)
        {
            return Guid.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier));
        }
    }
}
