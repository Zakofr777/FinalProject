using FinalProject.Application.DTOs;
using FinalProject.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace FinalProject.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserService _userService;
    
    public AuthController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserRegisterDto userRegisterDto)
    {
        var result = await _userService.RegisterUserAsync(userRegisterDto);
        return Ok(result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserLoginDto userLoginDto)
    {
        var result = await _userService.LoginUserAsync(userLoginDto);
        return Ok(result);
    }
    
    [HttpPost("login/accountant")]
    public async Task<IActionResult> LoginAccountant([FromBody] AccountantLoginDto dto)
    {
        var result = await _userService.LoginAccountantAsync(dto);
        return Ok(result);
    }    
    
}