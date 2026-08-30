using FinalProject.Application.Interfaces;
using FinalProject.Domain.Entities;
using FinalProject.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Infrastructure.Repositories;

public class AccountantRepository : IAccountantRepository
{
    private readonly AppDbContext _context;

    public AccountantRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Accountant?> GetByUserNameAsync(string userName) =>
        await _context.Accountants.FirstOrDefaultAsync(a => a.UserName == userName);
}