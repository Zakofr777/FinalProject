using FinalProject.Domain.enums;

namespace FinalProject.Domain.Entities;

public class Loan
{
    public int Id { get; set; }
    public LoanType LoanType { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "GEL";
    public int LoanPeriod { get; set; } 
    public LoanStatus Status { get; set; } = LoanStatus.InProcess; 
    public int UserId { get; set; }
    public User User { get; set; } = null!;
}