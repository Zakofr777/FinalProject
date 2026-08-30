using System.Security.Claims;
using FinalProject.Application.DTOs;
using FinalProject.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinalProject.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class LoansController : ControllerBase
{
    private readonly ILoanService _loanService;

    public LoansController(ILoanService loanService)
    {
        _loanService = loanService;
    }

    private int CurrentUserId => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    [Authorize(Roles = "User")]
    [HttpPost]
    public async Task<IActionResult> CreateLoan([FromBody] CreateLoanDto dto)
    {
        var loan = await _loanService.CreateLoanAsync(CurrentUserId, dto);
        return CreatedAtAction(nameof(GetUserLoans), new { id = loan.Id }, loan);
    }

    [Authorize(Roles = "User")]
    [HttpGet("my-loans")]
    public async Task<IActionResult> GetUserLoans()
    {
        var loans = await _loanService.GetUserLoansAsync(CurrentUserId);
        return Ok(loans);
    }

    [Authorize(Roles = "User")]
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateUserLoan(int id, [FromBody] UpdateLoanDto dto)
    {
        var loan = await _loanService.UpdateUserLoanAsync(CurrentUserId, id, dto);
        if (loan == null) return NotFound(new { message = "Loan not found or unauthorized access." });
        return Ok(loan);
    }

    [Authorize(Roles = "User")]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteUserLoan(int id)
    {
        var deleted = await _loanService.DeleteUserLoanAsync(CurrentUserId, id);
        if (!deleted) return NotFound(new { message = "Loan not found or unauthorized access." });
        return NoContent();
    }

    [Authorize(Roles = "Accountant")]
    [HttpGet("all")]
    public async Task<IActionResult> GetAllLoans()
    {
        var loans = await _loanService.GetAllLoansAsync();
        return Ok(loans);
    }

    [Authorize(Roles = "Accountant")]
    [HttpPut("accountant/{id:int}")]
    public async Task<IActionResult> UpdateLoanByAccountant(int id, [FromBody] UpdateLoanDto dto)
    {
        var loan = await _loanService.UpdateLoanByAccountantAsync(id, dto);
        if (loan == null) return NotFound(new { message = $"Loan {id} not found." });
        return Ok(loan);
    }

    [Authorize(Roles = "Accountant")]
    [HttpDelete("accountant/{id:int}")]
    public async Task<IActionResult> DeleteLoanByAccountant(int id)
    {
        var deleted = await _loanService.DeleteLoanByAccountantAsync(id);
        if (!deleted) return NotFound(new { message = $"Loan {id} not found." });
        return NoContent();
    }

    [Authorize(Roles = "Accountant")]
    [HttpPatch("{id:int}/status")]
    public async Task<IActionResult> UpdateLoanStatus(int id, [FromBody] UpdateLoanStatusDto dto)
    {
        var updated = await _loanService.UpdateLoanStatusAsync(id, dto);
        if (!updated) return NotFound(new { message = $"Loan {id} not found." });
        return Ok(new { message = "Loan status updated successfully." });
    }
}