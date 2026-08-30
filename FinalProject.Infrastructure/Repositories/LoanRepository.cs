using FinalProject.Application.Interfaces;
using FinalProject.Domain.Entities;
using FinalProject.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Infrastructure.Repositories;

public class LoanRepository : ILoanRepository
{
    private readonly AppDbContext _context;

    public LoanRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Loan?> GetByIdAsync(int id) => await _context.Loans.FindAsync(id);

    public async Task<IEnumerable<Loan>> GetByUserIdAsync(int userId) =>
        await _context.Loans.Where(l => l.UserId == userId).ToListAsync();

    public async Task<IEnumerable<Loan>> GetAllAsync() => await _context.Loans.ToListAsync();

    public async Task AddAsync(Loan loan) => await _context.Loans.AddAsync(loan);

    public void Remove(Loan loan) => _context.Loans.Remove(loan);

    public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
}