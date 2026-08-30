using FinalProject.Application.DTOs;
using FinalProject.Application.Interfaces;
using FinalProject.Domain.Entities;

namespace FinalProject.Application.Services;

public interface IUserService
{
    Task<AuthResponseDto> RegisterUserAsync(UserRegisterDto dto);
    Task<AuthResponseDto> LoginUserAsync(UserLoginDto dto);
    Task<AuthResponseDto> LoginAccountantAsync(AccountantLoginDto dto);
    Task<UserDtos.UserResponseDto?> GetUserByIdAsync(int id);
    Task<bool> BlockUserAsync(int userId, bool isBlocked);
}

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IAccountantRepository _accountantRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public UserService(
        IUserRepository userRepository,
        IAccountantRepository accountantRepository,
        IPasswordHasher passwordHasher,
        IJwtTokenGenerator jwtTokenGenerator)
    {
        _userRepository = userRepository;
        _accountantRepository = accountantRepository;
        _passwordHasher = passwordHasher;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<AuthResponseDto> RegisterUserAsync(UserRegisterDto dto)
    {
        if (await _userRepository.ExistsByUserNameAsync(dto.UserName))
            throw new InvalidOperationException("Username already exists.");

        var user = new User
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            UserName = dto.UserName,
            Age = dto.Age,
            Salary = dto.Salary,
            PasswordHash = _passwordHasher.HashPassword(dto.Password),
            IsBlocked = false
        };

        await _userRepository.AddAsync(user);
        await _userRepository.SaveChangesAsync();

        var token = _jwtTokenGenerator.GenerateToken(user.Id, user.UserName, "User");
        return new AuthResponseDto(token, user.UserName, "User");
    }

    public async Task<AuthResponseDto> LoginUserAsync(UserLoginDto dto)
    {
        var user = await _userRepository.GetByUserNameAsync(dto.UserName);
        if (user == null || !_passwordHasher.VerifyPassword(dto.Password, user.PasswordHash))
            throw new UnauthorizedAccessException("Invalid credentials.");

        var token = _jwtTokenGenerator.GenerateToken(user.Id, user.UserName, "User");
        return new AuthResponseDto(token, user.UserName, "User");
    }

    public async Task<AuthResponseDto> LoginAccountantAsync(AccountantLoginDto dto)
    {
        var accountant = await _accountantRepository.GetByUserNameAsync(dto.UserName);
        if (accountant == null || !_passwordHasher.VerifyPassword(dto.Password, accountant.PasswordHash))
            throw new UnauthorizedAccessException("Invalid credentials.");

        var token = _jwtTokenGenerator.GenerateToken(accountant.Id, accountant.UserName, "Accountant");
        return new AuthResponseDto(token, accountant.UserName, "Accountant");
    }

    public async Task<UserDtos.UserResponseDto?> GetUserByIdAsync(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null) return null;

        return new UserDtos.UserResponseDto(user.Id, user.FirstName, user.LastName, user.UserName, user.Age, user.Salary, user.IsBlocked);
    }

    public async Task<bool> BlockUserAsync(int userId, bool isBlocked)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null) return false;

        user.IsBlocked = isBlocked;
        await _userRepository.SaveChangesAsync();
        return true;
    }
}
