﻿using FluentValidation;

namespace TimePlannerAPI.DTOs
{
    public class RegisterUserDtoValidator : AbstractValidator<RegisterUserDto>
    {

 
       
        
            public RegisterUserDtoValidator()
            {
                RuleFor(x => x.Email)
                    .NotEmpty().WithMessage("Email is required")
                    .EmailAddress().WithMessage("Invalid email format")
                    .MaximumLength(100).WithMessage("Email cannot exceed 100 characters");

                RuleFor(x => x.Password)
                    .NotEmpty().WithMessage("Password is required")
                    .MinimumLength(8).WithMessage("Password must be at least 8 characters")
                    .MaximumLength(100).WithMessage("Password cannot exceed 100 characters")
                    .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter")
                    .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter")
                    .Matches("[0-9]").WithMessage("Password must contain at least one number")
                    .Matches("[^a-zA-Z0-9]").WithMessage("Password must contain at least one special character");

                RuleFor(x => x.FullName)
                    .NotEmpty().WithMessage("Full name is required")
                    .MaximumLength(100).WithMessage("Full name cannot exceed 100 characters");
            }
        }
    }

