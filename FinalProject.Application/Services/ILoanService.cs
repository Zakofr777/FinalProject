using FinalProject.Application.DTOs;
using FinalProject.Application.Interfaces;
using FinalProject.Domain.Entities;
using FinalProject.Domain.enums;

namespace FinalProject.Application.Services;

public interface ILoanService
{
    Task<LoanResponseDto> CreateLoanAsync(int userId, CreateLoanDto dto);
    Task<IEnumerable<LoanResponseDto>> GetUserLoansAsync(int userId);
    Task<LoanResponseDto?> UpdateUserLoanAsync(int userId, int loanId, UpdateLoanDto dto);
    Task<bool> DeleteUserLoanAsync(int userId, int loanId);
    
    Task<IEnumerable<LoanResponseDto>> GetAllLoansAsync();
    Task<LoanResponseDto?> UpdateLoanByAccountantAsync(int loanId, UpdateLoanDto dto);
    Task<bool> DeleteLoanByAccountantAsync(int loanId);
    Task<bool> UpdateLoanStatusAsync(int loanId, UpdateLoanStatusDto dto);
}

public class LoanService : ILoanService
{
    private readonly ILoanRepository _loanRepository;
    private readonly IUserRepository _userRepository;

    public LoanService(ILoanRepository loanRepository, IUserRepository userRepository)
    {
        _loanRepository = loanRepository;
        _userRepository = userRepository;
    }

    public async Task<LoanResponseDto> CreateLoanAsync(int userId, CreateLoanDto dto)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null) throw new KeyNotFoundException("User not found.");

        if (user.IsBlocked)
            throw new InvalidOperationException("Blocked users cannot apply for loans.");

        var loan = new Loan
        {
            UserId = userId,
            LoanType = dto.LoanType,
            Amount = dto.Amount,
            Currency = dto.Currency,
            LoanPeriod = dto.LoanPeriod,
            Status = LoanStatus.InProcess
        };

        await _loanRepository.AddAsync(loan);
        await _loanRepository.SaveChangesAsync();

        return MapToDto(loan);
    }

    public async Task<IEnumerable<LoanResponseDto>> GetUserLoansAsync(int userId)
    {
        var loans = await _loanRepository.GetByUserIdAsync(userId);
        return loans.Select(MapToDto);
    }

    public async Task<LoanResponseDto?> UpdateUserLoanAsync(int userId, int loanId, UpdateLoanDto dto)
    {
        var loan = await _loanRepository.GetByIdAsync(loanId);
        if (loan == null || loan.UserId != userId) return null;

        if (loan.Status != LoanStatus.InProcess)
            throw new InvalidOperationException("Loans can only be modified while in 'InProcess' status.");

        loan.LoanType = dto.LoanType;
        loan.Amount = dto.Amount;
        loan.Currency = dto.Currency;
        loan.LoanPeriod = dto.LoanPeriod;

        await _loanRepository.SaveChangesAsync();
        return MapToDto(loan);
    }

    public async Task<bool> DeleteUserLoanAsync(int userId, int loanId)
    {
        var loan = await _loanRepository.GetByIdAsync(loanId);
        if (loan == null || loan.UserId != userId) return false;

        if (loan.Status != LoanStatus.InProcess)
            throw new InvalidOperationException("Loans can only be deleted while in 'InProcess' status.");

        _loanRepository.Remove(loan);
        await _loanRepository.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<LoanResponseDto>> GetAllLoansAsync()
    {
        var loans = await _loanRepository.GetAllAsync();
        return loans.Select(MapToDto);
    }

    public async Task<LoanResponseDto?> UpdateLoanByAccountantAsync(int loanId, UpdateLoanDto dto)
    {
        var loan = await _loanRepository.GetByIdAsync(loanId);
        if (loan == null) return null;

        loan.LoanType = dto.LoanType;
        loan.Amount = dto.Amount;
        loan.Currency = dto.Currency;
        loan.LoanPeriod = dto.LoanPeriod;

        await _loanRepository.SaveChangesAsync();
        return MapToDto(loan);
    }

    public async Task<bool> DeleteLoanByAccountantAsync(int loanId)
    {
        var loan = await _loanRepository.GetByIdAsync(loanId);
        if (loan == null) return false;

        _loanRepository.Remove(loan);
        await _loanRepository.SaveChangesAsync();
        return true;
    }

    public async Task<bool> UpdateLoanStatusAsync(int loanId, UpdateLoanStatusDto dto)
    {
        var loan = await _loanRepository.GetByIdAsync(loanId);
        if (loan == null) return false;

        loan.Status = dto.Status;
        await _loanRepository.SaveChangesAsync();
        return true;
    }

    private static LoanResponseDto MapToDto(Loan loan) =>
        new(loan.Id, loan.UserId, loan.LoanType, loan.Amount, loan.Currency, loan.LoanPeriod, loan.Status);
}