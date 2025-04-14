using FluentValidation;
using TimePlannerAPI.DTOs;

namespace TimePlannerAPI.Validator
{
    public class LoginUserDtoValidator : AbstractValidator<LoginUserDto>
    {

        
            public LoginUserDtoValidator()
            {
                RuleFor(x => x.Email)
                    .NotEmpty()
                    .WithMessage("Email is required.")
                    .EmailAddress()
                    .WithMessage("Invalid email format.");

                RuleFor(x => x.Password)
                    .NotEmpty()
                    .WithMessage("Password is required.");
            }
        }
    }

