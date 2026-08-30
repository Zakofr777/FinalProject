using System;
using System.Threading.Tasks;
using FinalProject.Application.DTOs;
using FinalProject.Application.Interfaces;
using FinalProject.Application.Services;
using FinalProject.Domain.Entities;
using Moq;
using Xunit;

namespace FinalProject.Tests.Services;

public class UserServiceTests
{
    private readonly Mock<IUserRepository> _userRepoMock;
    private readonly Mock<IAccountantRepository> _accountantRepoMock;
    private readonly Mock<IPasswordHasher> _passwordHasherMock;
    private readonly Mock<IJwtTokenGenerator> _tokenGeneratorMock;
    private readonly UserService _userService;

    public UserServiceTests()
    {
        _userRepoMock = new Mock<IUserRepository>();
        _accountantRepoMock = new Mock<IAccountantRepository>();
        _passwordHasherMock = new Mock<IPasswordHasher>();
        _tokenGeneratorMock = new Mock<IJwtTokenGenerator>();

        _userService = new UserService(
            _userRepoMock.Object,
            _accountantRepoMock.Object,
            _passwordHasherMock.Object,
            _tokenGeneratorMock.Object
        );
    }

    #region User Login Tests

    [Fact]
    public async Task validUserLogin()
    {
        var request = new UserLoginDto("john_doe", "password123");
        var user = new User { Id = 1, UserName = "john_doe", PasswordHash = "hashed_pw" };
        var expectedToken = "fake.jwt.token";

        _userRepoMock.Setup(r => r.GetByUserNameAsync(request.UserName))
                     .ReturnsAsync(user);

        _passwordHasherMock.Setup(h => h.VerifyPassword(request.Password, user.PasswordHash))
                           .Returns(true);

        _tokenGeneratorMock.Setup(g => g.GenerateToken(user.Id, user.UserName, "User"))
                           .Returns(expectedToken);

        var result = await _userService.LoginUserAsync(request);

        Assert.NotNull(result);
        Assert.Equal(expectedToken, result.Token);
    }

    [Fact]
    public async Task loginUserNotFound()
    {
        var request = new UserLoginDto("nonexistent", "password123");

        _userRepoMock.Setup(r => r.GetByUserNameAsync(request.UserName))
                     .ReturnsAsync((User?)null);

        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _userService.LoginUserAsync(request));
    }

    [Fact]
    public async Task loginWrongPassword()
    {
        var request = new UserLoginDto("john_doe", "wrong_password");
        var user = new User { Id = 1, UserName = "john_doe", PasswordHash = "hashed_pw" };

        _userRepoMock.Setup(r => r.GetByUserNameAsync(request.UserName))
                     .ReturnsAsync(user);

        _passwordHasherMock.Setup(h => h.VerifyPassword(request.Password, user.PasswordHash))
                           .Returns(false);

        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _userService.LoginUserAsync(request));
    }

    #endregion

    #region Accountant Login Tests

    [Fact]
    public async Task validLoginAccountant()
    {
        var request = new AccountantLoginDto("yifo", "yifoyifo123");
        var accountant = new Accountant { Id = 1, UserName = "yifo", PasswordHash = "hashed_yifo_pw" };
        var expectedToken = "fake.accountant.jwt.token";

        _accountantRepoMock.Setup(r => r.GetByUserNameAsync(request.UserName))
                           .ReturnsAsync(accountant);

        _passwordHasherMock.Setup(h => h.VerifyPassword(request.Password, accountant.PasswordHash))
                           .Returns(true);

        _tokenGeneratorMock.Setup(g => g.GenerateToken(accountant.Id, accountant.UserName, "Accountant"))
                           .Returns(expectedToken);

        var result = await _userService.LoginAccountantAsync(request);

        Assert.NotNull(result);
        Assert.Equal(expectedToken, result.Token);
    }

    #endregion
}