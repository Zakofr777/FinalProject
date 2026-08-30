namespace FinalProject.Application.DTOs;

public record UserRegisterDto(
    string FirstName,
    string LastName,
    string UserName,
    int Age,
    decimal Salary,
    string Password
);

public record UserLoginDto(string UserName, string Password);
public record AccountantLoginDto(string UserName, string Password);
public record AuthResponseDto(string Token, string UserName, string Role);