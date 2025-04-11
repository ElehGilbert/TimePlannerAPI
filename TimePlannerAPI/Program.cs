using Microsoft.AspNetCore.Builder;
using TimePlannerAPI.Repositories;
using Microsoft.OpenApi.Models;
using TimePlannerAPI.Endpoint;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddScoped<IScheduleRepostory, ScheduleRepostory>();

builder.Services.AddControllers();

//For Activity Repository
builder.Services.AddScoped<IActivityRepository, ActivityRepository>();


//Adding ITimeBlockRepository
builder.Services.AddScoped<ITimeBlockRepository, TimeBlockRepository>();

//This is for Swagger
builder.Services.AddEndpointsApiExplorer(); //Required for Swagger
builder.Services.AddSwaggerGen(); //Adds Swagger Generator

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();









// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwagger();
    app.MapOpenApi();
}
app.MapGroup("/api/schedules").MapSchedules().RequireAuthorization();


app.MapGroup("/api/schedules/{scheduleId:guid}/timeblocks")
    .MapTimeBlocks()
    .RequireAuthorization();

app.MapGroup("/api/activities")
    .MapActivities()
    .RequireAuthorization();





app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
