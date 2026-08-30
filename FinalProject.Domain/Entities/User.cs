namespace FinalProject.Domain.Entities;

public class User
{
    public int Id { get; set; }
    
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string UserName  { get; set; } = string.Empty;
    public int Age { get; set; }
    public decimal Salary { get; set; }
    public bool IsBlocked { get; set; } = false; // Default: false
    public string PasswordHash { get; set; } = string.Empty;
    
    public ICollection<Loan> Loans { get; set; } = new List<Loan>();
    
}