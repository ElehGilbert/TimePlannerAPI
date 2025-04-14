using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TimePlannerAPI.DTOs;
using TimePlannerAPI.Services;

// Endpoints/UsersEndpoint.cs
//using Microsoft.AspNetCore.Mvc;
////using TimePlanner.Api.DTOs;
////using TimePlanner.Api.Services;
//using FluentValidation;
//using Microsoft.AspNetCore.Authorization;

namespace TimePlannerAPI.Endpoint
{
    public static class UsersEndpoint
    {

            public static RouteGroupBuilder MapUsers(this RouteGroupBuilder group) //User MapGroup
            {
                group.MapPost("/register", Register)
                    .WithName("RegisterUser")
                    .WithSummary("Register a new user")
                    .WithDescription("Creates a new user account")
                    .Produces<AuthResponseDto>(StatusCodes.Status201Created)
                    .ProducesValidationProblem();

                group.MapPost("/login", Login)
                    .WithName("LoginUser")
                    .WithSummary("Authenticate a user")
                    .WithDescription("Returns JWT token for authentication")
                    .Produces<AuthResponseDto>()
                    .ProducesValidationProblem()
                    .Produces(StatusCodes.Status401Unauthorized);

                group.MapPost("/refresh-token", RefreshToken)
                    .WithName("RefreshToken")
                    .WithSummary("Refresh access token")
                    .WithDescription("Returns new JWT token using refresh token")
                    .Produces<AuthResponseDto>()
                    .Produces(StatusCodes.Status401Unauthorized);

                group.MapGet("/me", GetCurrentUser)
                    .RequireAuthorization()
                    .WithName("GetCurrentUser")
                    .WithSummary("Get current user info")
                    .WithDescription("Returns information about the currently authenticated user")
                    .Produces<UserDTO>()
                    .Produces(StatusCodes.Status401Unauthorized);

                group.MapPut("/me", UpdateCurrentUser)
                    .RequireAuthorization()
                    .WithName("UpdateCurrentUser")
                    .WithSummary("Update current user")
                    .WithDescription("Updates information for the currently authenticated user")
                    .Produces<UserDTO>()
                    .ProducesValidationProblem()
                    .Produces(StatusCodes.Status401Unauthorized);

                return group;
            }



        //Method for the Follwoing EndPoints
        //Error Corrected and Implemented the Mapper for solve the ASP.NET cor runtime error

            private static async Task<IResult> Register(
                [FromBody] RegisterUserDto registerDto,
                [FromServices] IAuthService authService,
                [FromServices] IValidator<RegisterUserDto> validator, IMapper mapper)
            {
                var validationResult = await validator.ValidateAsync(registerDto);
                if (!validationResult.IsValid)
                {
                    return Results.ValidationProblem(validationResult.ToDictionary());
                }

                var result = await authService.RegisterAsync(registerDto);

            if (!result.Success)
                {
                    return Results.BadRequest(result.Message);
                }

                return Results.Created($"/users/me", result.AuthResponse);
            }

            private static async Task<IResult> Login(
                [FromBody] LoginUserDto loginDto,
                [FromServices] IAuthService authService,
                [FromServices] IValidator<LoginUserDto> validator, IMapper mapper)
            {
                var validationResult = await validator.ValidateAsync(loginDto);
                if (!validationResult.IsValid)
                {
                    return Results.ValidationProblem(validationResult.ToDictionary());
                }
            

            var result = await authService.LoginAsync(loginDto);
            if (!result.Success)
            {
                return Results.Unauthorized();
            }
            //if (!result.Success)
            //{
            //    return Results.BadRequest(result.Message);
            //}

            return Results.Ok(result.AuthResponse);
            }

            private static async Task<IResult> RefreshToken(
                [FromBody] RefreshTokenDto refreshTokenDto,
                IMapper mapper,
                [FromServices] IAuthService authService)
            {
                var result = await authService.RefreshTokenAsync(refreshTokenDto.RefreshToken);
                if (!result.Success)
                {
                    return Results.Unauthorized();
                }
                return Results.Ok(result.AuthResponse);
            }

            [Authorize]
            private static async Task<IResult> GetCurrentUser(IMapper mapper,
                [FromServices] IUserService userService,
                ClaimsPrincipal user)
            {
                var userId = Guid.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier));
                var currentUser = await userService.GetByIdAsync(userId);
                return Results.Ok(currentUser);
            }

            [Authorize]
            private static async Task<IResult> UpdateCurrentUser(IMapper mapper,
                [FromBody] UpdateUserDto updateDto,
                [FromServices] IUserService userService,
                [FromServices] IValidator<UpdateUserDto> validator,
                ClaimsPrincipal user)
            {
                var validationResult = await validator.ValidateAsync(updateDto);
                if (!validationResult.IsValid)
                {
                    return Results.ValidationProblem(validationResult.ToDictionary());
                }

                var userId = Guid.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier));
                var updatedUser = await userService.UpdateAsync(userId, updateDto);
                return Results.Ok(updatedUser);
            }
        }

  
}

