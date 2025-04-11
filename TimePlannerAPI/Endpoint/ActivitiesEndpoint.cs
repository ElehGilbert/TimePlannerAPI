﻿using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OutputCaching;
using System.Security.Claims;
using TimePlannerAPI.DTOs;
using TimePlannerAPI.Models;
using TimePlannerAPI.Repositories;


namespace TimePlannerAPI.Endpoint
{
    public static class ActivitiesEndpoint
    {
      

            public static RouteGroupBuilder MapActivities(this RouteGroupBuilder group)
            {
                // Endpoint to get all activities
                group.MapGet("/", GetAll)
                    .CacheOutput(c => c.Expire(TimeSpan.FromSeconds(60)).Tag("activities-get"));

                // Endpoint to get activity by ID
                group.MapGet("/{id:guid}", GetById);

                // Endpoint to create new activity
                group.MapPost("/", Create);

                // Endpoint to update activity
                group.MapPut("/{id:guid}", Update);

                // Endpoint to delete activity
                group.MapDelete("/{id:guid}", Delete);

                return group;
            }

            static async Task<Ok<List<ActivityDto>>> GetAll(
                IActivityRepository repository,
                IMapper mapper,
                ClaimsPrincipal user)
            {
                var userId = GetUserId(user);
                var activities = await repository.GetByUserId(userId);
                var activityDtos = mapper.Map<List<ActivityDto>>(activities);
                return TypedResults.Ok(activityDtos);
            }

            static async Task<Results<Ok<ActivityDto>, NotFound>> GetById(
                Guid id,
                IActivityRepository repository,
                IMapper mapper,
                ClaimsPrincipal user)
            {
                var userId = GetUserId(user);
                var activity = await repository.GetByIdAsync(id);

                if (activity is null || activity.UserId != userId)
                {
                    return TypedResults.NotFound();
                }

                var activityDto = mapper.Map<ActivityDto>(activity);
                return TypedResults.Ok(activityDto);
            }

            static async Task<Created<ActivityDto>> Create(
                CreateActivityDto createActivityDto,
                IActivityRepository repository,
                IOutputCacheStore outputCacheStore,
                IMapper mapper,
                ClaimsPrincipal user)
            {
                var activity = mapper.Map<Activity>(createActivityDto);
                activity.UserId = GetUserId(user);

                await repository.Create(activity);
                await outputCacheStore.EvictByTagAsync("activities-get", default);

                var activityDto = mapper.Map<ActivityDto>(activity);
                return TypedResults.Created($"/activities/{activity.Id}", activityDto);
            }

            static async Task<Results<NotFound, NoContent>> UpdateAsync(
                Guid id,
                UpdateActivityDto updateActivityDto,
                IActivityRepository repository,
                IOutputCacheStore outputCacheStore,
                IMapper mapper,
                ClaimsPrincipal user)
            {
                var userId = GetUserId(user);
                var existingActivity = await repository.GetUserById(id);

                if (existingActivity is null || existingActivity.UserId != userId)
                {
                    return TypedResults.NotFound();
                }

                mapper.Map(updateActivityDto, existingActivity);
                await repository.UpdateAsync(existingActivity);
                await outputCacheStore.EvictByTagAsync("activities-get", default);

                return TypedResults.NoContent();
            }

        static async Task<Results<NotFound, NoContent>> Delete(
            Guid id,
            IActivityRepository repository,
            IOutputCacheStore outputCacheStore,
            ClaimsPrincipal user)
        {
            var userId = GetUserId(user);
            var activity = await repository.GetById(id);

            if (activity is null || activity.UserId != userId)
            {
                return TypedResults.NotFound();
            }

            await repository.Delete(id);
            await outputCacheStore.EvictByTagAsync("activities-get", default);

            return TypedResults.NoContent();
        }
        private static Guid GetUserId(ClaimsPrincipal user)
        {
            return Guid.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier));
        }
    }
}
