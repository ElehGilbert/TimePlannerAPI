using TimePlannerAPI.Validator; // Add this namespace if LoginUserDtoValidator is defined here

using Microsoft.AspNetCore.Builder;
using TimePlannerAPI.Repositories;
using Microsoft.OpenApi.Models;
using TimePlannerAPI.Endpoint;
using FluentValidation;
using TimePlannerAPI.DTOs;
using TimePlannerAPI.Data;
using Microsoft.EntityFrameworkCore;
using TimePlannerAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;//Just Installed 14 April


var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<timePlannerDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));



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


// Add CreateTimeBlockDtoValidator to services configuration
builder.Services.AddValidatorsFromAssemblyContaining<CreateTimeBlockDtoValidator>();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();


//1.	IUserRepository // Add IUserRepository registration
builder.Services.AddScoped<IUserRepository, UserRepository>();


////2.	IValidator<UpdateUserDto>:
//builder.Services.AddScoped<IValidator<UpdateUserDto>, UpdateUserDtoValidator>();


////3.	IValidator<LoginUserDto>:
//builder.Services.AddScoped<IValidator<LoginUserDto>, LoginUserDtoValidator>();

// Add missing validators
builder.Services.AddValidatorsFromAssemblyContaining<UpdateUserDtoValidator>(); //Need to add code in the class!!!!!!!!!!!!!!!!!!!!!
builder.Services.AddValidatorsFromAssemblyContaining<LoginUserDtoValidator>(); //Need to add code in the class!!!!!!!!!!!!!!!!!!!!!





// Add to your services configuration
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IUserService, UserService>();

// Add JWT authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.ASCII.GetBytes(builder.Configuration["Jwt:Secret"])),
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    });

// Add authorization
builder.Services.AddAuthorization();

// Add validators
builder.Services.AddValidatorsFromAssemblyContaining<RegisterUserDtoValidator>();


/////Service End Here

var app = builder.Build();









// Configure the HTTP request pipeline.

//Middleware Zone - Begin

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwagger();
    app.MapOpenApi();
}
app.MapGroup("/schedules").MapSchedules().RequireAuthorization();


app.MapGroup("/schedules/{scheduleId:guid}/timeblocks")
    .MapTimeBlocks()
    .RequireAuthorization();

app.MapGroup("/activities")
    .MapActivities()
    .RequireAuthorization();

app.MapGroup("/users")
    .MapUsers()
    .RequireAuthorization();





app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
