using FinalProject.Domain.Entities;

namespace FinalProject.Application.Interfaces;

public interface IAccountantRepository
{
    Task<Accountant?> GetByUserNameAsync(string userName);
}