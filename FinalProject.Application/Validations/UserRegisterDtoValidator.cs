using FinalProject.Application.DTOs;
using FluentValidation;

namespace FinalProject.Application.Validators;

public class UserRegisterDtoValidator : AbstractValidator<UserRegisterDto>
{
    public UserRegisterDtoValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty().MaximumLength(50);
        RuleFor(x => x.LastName).NotEmpty().MaximumLength(50);
        RuleFor(x => x.UserName).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Age).InclusiveBetween(18, 100).WithMessage("Age must be between 18 and 100.");
        RuleFor(x => x.Salary).GreaterThan(0).WithMessage("Salary must be greater than zero.");
        RuleFor(x => x.Password).NotEmpty().MinimumLength(6).WithMessage("Password must be at least 6 characters long.");
    }
}