using FinalProject.Domain.Entities;

namespace FinalProject.Application.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(int id);
    Task<User?> GetByUserNameAsync(string userName);
    Task<bool> ExistsByUserNameAsync(string userName);
    Task AddAsync(User user);
    Task SaveChangesAsync();
}