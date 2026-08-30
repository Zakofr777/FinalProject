namespace FinalProject.Application.DTOs;

public class UserDtos
{
    
    public record UserResponseDto(
        int Id, string FirstName,
        string LastName,
        string Username ,
        int Age,
        decimal Salary,
        bool IsBlocked
        );

    public record BlockUserDto(
        bool IsBlocked
    );
}