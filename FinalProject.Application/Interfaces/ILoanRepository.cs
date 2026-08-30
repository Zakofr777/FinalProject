using FinalProject.Domain.Entities;

namespace FinalProject.Application.Interfaces;

public interface ILoanRepository
{
    Task<Loan?> GetByIdAsync(int id);
    Task<IEnumerable<Loan>> GetByUserIdAsync(int userId);
    Task<IEnumerable<Loan>> GetAllAsync();
    Task AddAsync(Loan loan);
    void Remove(Loan loan);
    Task SaveChangesAsync();
}