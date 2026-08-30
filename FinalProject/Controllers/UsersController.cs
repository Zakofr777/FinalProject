using FinalProject.Application.DTOs;
using FinalProject.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinalProject.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    IUserService _userService;
    
    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(int id)
    {
        var user = _userService.GetUserByIdAsync(id).Result;
        if(user == null) return NotFound(new { message = $"User with ID {id} not found." });
        return Ok(user);
    }
    
    [Authorize(Roles = "Accountant")]
    [HttpPut("{id:int}/block")]
    public async Task<IActionResult> BlockUser(int id, [FromBody] UserDtos.BlockUserDto dto)
    {
        var success = await _userService.BlockUserAsync(id, dto.IsBlocked);
        if (!success) return NotFound(new { message = $"User with ID {id} not found." });
        return Ok(new { message = $"User blocked status updated to {dto.IsBlocked}." });
    }
}