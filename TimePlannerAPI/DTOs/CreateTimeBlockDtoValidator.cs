using FluentValidation;

namespace TimePlannerAPI.DTOs
{
    public class CreateTimeBlockDtoValidator : AbstractValidator<CreateTimeBlockDto>
    {
       
        
            public CreateTimeBlockDtoValidator()
            {
                RuleFor(x => x.Title)
                    .NotEmpty()
                    .Length(3, 100);

                RuleFor(x => x.Description)
                    .MaximumLength(500)
                    .When(x => !string.IsNullOrEmpty(x.Description));

                RuleFor(x => x.StartTime)
                    .NotEmpty()
                    .GreaterThanOrEqualTo(DateTime.Now.Date)
                    .WithMessage("Start time cannot be in the past");

                RuleFor(x => x.EndTime)
                    .NotEmpty()
                    .GreaterThan(x => x.StartTime)
                    .WithMessage("End time must be after start time");

                RuleFor(x => x.Color)
                    .Matches("^#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{3})$")
                    .When(x => !string.IsNullOrEmpty(x.Color));

                RuleFor(x => x.ScheduleId)
                    .NotEmpty()
                    .WithMessage("Schedule ID is required");
            }
        }
    }

