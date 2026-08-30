using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinalProject.Application.DTOs;
using FinalProject.Application.Interfaces;
using FinalProject.Application.Services;
using FinalProject.Domain.Entities;
using FinalProject.Domain.enums;
using Moq;
using Xunit;

namespace FinalProject.Tests.Services;

public class LoanServiceTests
{
    private readonly Mock<ILoanRepository> _loanRepoMock;
    private readonly Mock<IUserRepository> _userRepoMock;
    private readonly LoanService _loanService;

    public LoanServiceTests()
    {
        _loanRepoMock = new Mock<ILoanRepository>();
        _userRepoMock = new Mock<IUserRepository>();

        _loanService = new LoanService(
            _loanRepoMock.Object,
            _userRepoMock.Object
        );
    }

    #region Create Loan Tests

    [Fact]
    public async Task CreateLoan_UserExists_CreatesAndSavesLoan()
    {
        // Arrange
        int userId = 1;
        var createDto = new CreateLoanDto(LoanType.AutoLoan, 20000, "EUR", 2);
        var user = new User { Id = userId, UserName = "john_doe" };

        _userRepoMock.Setup(r => r.GetByIdAsync(userId))
                     .Returns(Task.FromResult<User?>(user));

        _loanRepoMock.Setup(r => r.AddAsync(It.IsAny<Loan>()))
                     .Returns(Task.CompletedTask);

        _loanRepoMock.Setup(r => r.SaveChangesAsync())
                     .Returns(Task.CompletedTask);
        
        var result = await _loanService.CreateLoanAsync(userId, createDto);
        Assert.NotNull(result);
        _loanRepoMock.Verify(r => r.AddAsync(It.Is<Loan>(l => 
            l.UserId == userId && 
            l.Amount == createDto.Amount
        )), Times.Once);

        _loanRepoMock.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task CreateLoan_UserNotFound_ThrowsKeyNotFoundException()
    {
        int userId = 99;
        var createDto = new CreateLoanDto(LoanType.QuickLoan, 10000, "GEL", 4);

        _userRepoMock.Setup(r => r.GetByIdAsync(userId))
                     .Returns(Task.FromResult<User?>(null));

        await Assert.ThrowsAsync<KeyNotFoundException>(() => _loanService.CreateLoanAsync(userId, createDto));
        _loanRepoMock.Verify(r => r.AddAsync(It.IsAny<Loan>()), Times.Never);
        _loanRepoMock.Verify(r => r.SaveChangesAsync(), Times.Never);
    }

    #endregion

    #region Read Loan Tests

    [Fact]
    public async Task GetUserLoans_ReturnsListOfLoans()
    {
        int userId = 1;
        var userLoans = new List<Loan>
        {
            new Loan { Id = 10, UserId = userId, Amount = 500m },
            new Loan { Id = 11, UserId = userId, Amount = 1200m }
        };

        _loanRepoMock.Setup(r => r.GetByUserIdAsync(userId))
                     .Returns(Task.FromResult<IEnumerable<Loan>>(userLoans));

        var result = await _loanService.GetUserLoansAsync(userId);

        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        _loanRepoMock.Verify(r => r.GetByUserIdAsync(userId), Times.Once);
    }

    [Fact]
    public async Task GetAllLoans_ReturnsAllLoansInDb()
    {
        var allLoans = new List<Loan>
        {
            new Loan { Id = 1, UserId = 1, Amount = 500m },
            new Loan { Id = 2, UserId = 2, Amount = 2000m }
        };

        _loanRepoMock.Setup(r => r.GetAllAsync())
                     .Returns(Task.FromResult<IEnumerable<Loan>>(allLoans));

        var result = await _loanService.GetAllLoansAsync();

        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        _loanRepoMock.Verify(r => r.GetAllAsync(), Times.Once);
    }

    #endregion

    #region Status / Delete Tests

   

    [Fact]
    public async Task DeleteUserLoanAsync_ValidUserAndLoanInProcess_RemovesAndSaves()
    {
        int userId = 1;
        int loanId = 10;
        var existingLoan = new Loan 
        { 
            Id = loanId, 
            UserId = userId, 
            Amount = 500m, 
            Status = LoanStatus.InProcess 
        };

        _loanRepoMock.Setup(r => r.GetByIdAsync(loanId))
            .Returns(Task.FromResult<Loan?>(existingLoan));

        _loanRepoMock.Setup(r => r.SaveChangesAsync())
            .Returns(Task.CompletedTask);

        var result = await _loanService.DeleteUserLoanAsync(userId, loanId);

        Assert.True(result);
        _loanRepoMock.Verify(r => r.Remove(existingLoan), Times.Once);
        _loanRepoMock.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    #endregion
}