using FluentValidation;
using TimePlannerAPI.DTOs;

namespace TimePlannerAPI.Validator
{
    public class UpdateUserDtoValidator : AbstractValidator<UpdateUserDto>
    {
   
        
        
            public UpdateUserDtoValidator()
            {
                RuleFor(x => x.FullName)
                    .MaximumLength(100)
                    .WithMessage("Full name must not exceed 100 characters.");

                RuleFor(x => x.Email)
                    .EmailAddress()
                    .When(x => !string.IsNullOrEmpty(x.Email))
                    .WithMessage("Invalid email format.");

                RuleFor(x => x.Password)
                    .MinimumLength(6)
                    .MaximumLength(100)
                    .When(x => !string.IsNullOrEmpty(x.Password))
                    .WithMessage("Password must be between 6 and 100 characters.");
            }
        }
    }


