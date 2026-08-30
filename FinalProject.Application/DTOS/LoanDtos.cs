using FinalProject.Domain.enums;

namespace FinalProject.Application.DTOs;

public record CreateLoanDto(
    LoanType LoanType,
    decimal Amount,
    string Currency,
    int LoanPeriod
);

public record UpdateLoanDto(
    LoanType LoanType,
    decimal Amount,
    string Currency,
    int LoanPeriod
);

public record UpdateLoanStatusDto(LoanStatus Status);

public record LoanResponseDto(
    int Id,
    int UserId,
    LoanType LoanType,
    decimal Amount,
    string Currency,
    int LoanPeriod,
    LoanStatus Status
);