using FinalProject.Application.DTOs;
using FluentValidation;

namespace FinalProject.Application.Validators;

public class CreateLoanDtoValidator : AbstractValidator<CreateLoanDto>
{
    public CreateLoanDtoValidator()
    {
        RuleFor(x => x.Amount).GreaterThan(0).WithMessage("Loan amount must be greater than zero.");
        RuleFor(x => x.Currency).NotEmpty().Length(3).WithMessage("Currency must be a 3-letter ISO code (e.g., USD, GEL).");
        RuleFor(x => x.LoanPeriod).GreaterThan(0).WithMessage("Loan period must be at least 1 month.");
        RuleFor(x => x.LoanType).IsInEnum().WithMessage("Invalid loan type.");
    }
}