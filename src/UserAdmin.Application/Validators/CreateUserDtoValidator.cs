using System;
using Application.Dtos.UserDtos;
using FluentValidation;

namespace Application.Validators;

public class RequestUserDtoValidator : AbstractValidator<RequestUserDto>
{
    public RequestUserDtoValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required.")
            .MaximumLength(100).WithMessage("First name cannot exceed 100 characters.");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required.")
            .MaximumLength(100).WithMessage("Last name cannot exceed 100 characters.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email format.");
        
        //RuleFor(x => x.UserId)
        //    .NotEmpty().WithMessage("UserId is required.");
    }
}
