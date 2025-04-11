
using TimePlannerAPI.DTOs;
using TimePlannerAPI.Repositories;
using FluentValidation;

namespace TimePlannerAPI.DTOs
{
    public class UpdateTimeBlockDtoValidator : AbstractValidator<UpdateTimeBlockDto>
        {
            public UpdateTimeBlockDtoValidator() //(UpdateTimeBlockDto dto)
        {
                // Title validation (when provided)
                When(x => x.Title != null, () =>
                {
                    RuleFor(x => x.Title)
                        .NotEmpty()
                        .Length(3, 100);
                });

                // Description validation (when provided)
                When(x => x.Description != null, () =>
                {
                    RuleFor(x => x.Description)
                        .MaximumLength(500);
                });

                // Time validation rules !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!Come back and adjust this Validation
                //RuleFor(x => x)
                //    .Must(dto => dto.StartTime == null, dto.EndTime == null,
                //   dto.StartTime < dto.EndTime)

                //    .WithMessage("End time must be after start time")
                //    .OverridePropertyName("TimeRange");

                // Color validation (when provided)
                When(x => x.Color != null, () =>
                {
                    RuleFor(x => x.Color)
                        .Matches("^#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{3})$");
                });
            }
        }
    }
//}
//}
