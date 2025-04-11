using Microsoft.AspNetCore.Http.HttpResults;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using AutoMapper;
using Microsoft.AspNetCore.OutputCaching;
using TimePlannerAPI.Repositories;
using TimePlannerAPI.DTOs;
using System.Security.Claims;
using TimePlannerAPI.Models;

namespace TimePlannerAPI.Endpoint
{
   
        public static class ScheduleEndPoint 
        {
            public static RouteGroupBuilder MapSchedules(this RouteGroupBuilder group)
            {
                // Endpoint to get all schedules for current user
                group.MapGet("/", GetAll)
                    .CacheOutput(c => c.Expire(TimeSpan.FromSeconds(60)).Tag("schedules-get"));

                // Endpoint to get schedule by ID
                group.MapGet("/{id:guid}", GetById);

                // Endpoint to create new schedule
                group.MapPost("/", Create);

                // Endpoint to update schedule
                group.MapPut("/{id:guid}", Update);

                // Endpoint to delete schedule
                group.MapDelete("/{id:guid}", Delete);

                return group;
            }

            static async Task<Ok<List<ScheduleDto>>> GetAll(
                IScheduleRepostory repository,
                IMapper mapper,
                ClaimsPrincipal user)
            {
                var userId = GetUserId(user);
                var schedules = await repository.GetUserSchedulesAsync(userId);
                var scheduleDtos = mapper.Map<List<ScheduleDto>>(schedules);
                return TypedResults.Ok(scheduleDtos);
            }

            static async Task<Results<Ok<ScheduleDto>, NotFound>> GetById(
                Guid id,
                IScheduleRepostory repository,
                IMapper mapper,
                ClaimsPrincipal user)
            {
                var userId = GetUserId(user);
                var schedule = await repository.GetScheduleById(id);

                if (schedule is null || schedule.UserId != userId)
                {
                    return TypedResults.NotFound();
                }

                var scheduleDto = mapper.Map<ScheduleDto>(schedule);
                return TypedResults.Ok(scheduleDto);
            }

            static async Task<Created<ScheduleDto>> Create(
                CreateScheduleDto createScheduleDto,
                IScheduleRepostory repository,
                IOutputCacheStore outputCacheStore,
                IMapper mapper,
                ClaimsPrincipal user)
            {
                var schedule = mapper.Map<Schedule>(createScheduleDto);
                schedule.UserId = GetUserId(user);
                schedule.CreatedDate = DateTime.UtcNow;

                await repository.AddScheduleAsync(schedule);// To create schedule
                await outputCacheStore.EvictByTagAsync("schedules-get", default);

                var scheduleDto = mapper.Map<ScheduleDto>(schedule);
                return TypedResults.Created($"/schedules/{schedule.Id}", scheduleDto);
            }





            static async Task<Results<NotFound, NoContent>> Update(
                Guid id,
                UpdateScheduleDto updateScheduleDto,
                IScheduleRepostory repository,
                IOutputCacheStore outputCacheStore,
                IMapper mapper,
                ClaimsPrincipal user)
            {
                var userId = GetUserId(user);
                var existingSchedule = await repository.GetScheduleById(id);

                if (existingSchedule is null || existingSchedule.UserId != userId)
                {
                    return TypedResults.NotFound();
                }

                mapper.Map(updateScheduleDto, existingSchedule);
                existingSchedule.CreatedDate = DateTime.UtcNow;

                await repository.UpdateScheduleAsync(existingSchedule);
                await outputCacheStore.EvictByTagAsync("schedules-get", default);

                return TypedResults.NoContent();
            }

            static async Task<Results<NotFound, NoContent>> Delete(
                Guid id,
                IScheduleRepostory repository,
                IOutputCacheStore outputCacheStore,
                ClaimsPrincipal user)
            {
                var userId = GetUserId(user);
                var schedule = await repository.GetScheduleById(id);

                if (schedule is null || schedule.UserId != userId)
                {
                    return TypedResults.NotFound();
                }
            
        await repository.DeleteScheduleAsync(id);
        await outputCacheStore.EvictByTagAsync("schedules-get", default);

            return TypedResults.NoContent();
        }

        private static Guid GetUserId(ClaimsPrincipal user)
        {
            return Guid.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier));
        }
    }
}


