namespace FinalProject.Domain.Entities;

public class User
{
    public int Id { get; set; }
    public string name { get; set; } = string.Empty;
    public string lastname { get; set; } = string.Empty;
    public string username  { get; set; } = string.Empty;
    public int age { get; set; }
    public decimal Salary { get; set; }
    public bool IsBlocked { get; set; } = false; // Default: false
    public string PasswordHash { get; set; } = string.Empty;
    
    public ICollection<Loan> Loans { get; set; } = new List<Loan>();
    
}