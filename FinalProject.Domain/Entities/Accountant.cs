namespace FinalProject.Domain.Entities;

public class Accountant
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
}