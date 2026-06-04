using Microsoft.AspNetCore.Mvc;
using ExpenseTracker.API.Data;
using ExpenseTracker.API.Dtos;
using ExpenseTracker.API.Mappers;
using ExpenseTracker.API.Models;
using ExpenseTracker.API.Services;

namespace ExpenseTracker.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ExpenseController : ControllerBase
{
    private readonly IExpenseService _expenseService;
    private readonly AppDbContext _context;

    public ExpenseController(IExpenseService expenseService, AppDbContext context)
    {
        _expenseService = expenseService;
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string? sortBy = null, [FromQuery] string? sortDirection = null)
    {
        var expenses = await _expenseService.GetAllAsync(sortBy, sortDirection);
        return Ok(expenses.Select(e => e.ToDto()).ToList());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var expense = await _expenseService.GetByIdWithCategoryAsync(id);
        if (expense == null) return NotFound();
        return Ok(expense.ToDto());
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ExpenseRequestDto expenseDto)
    {
        if (string.IsNullOrWhiteSpace(expenseDto.Title))
            return BadRequest(new { message = "Title is required" });
        
        if (expenseDto.Amount <= 0)
            return BadRequest(new { message = "Amount must be greater than 0" });

        var expense = expenseDto.ToEntity();
        _context.Expenses.Add(expense);
        await _context.SaveChangesAsync();
        return Ok(expense.ToDto());
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] ExpenseRequestDto expenseDto)
    {
        if (string.IsNullOrWhiteSpace(expenseDto.Title))
            return BadRequest(new { message = "Title is required" });
        
        if (expenseDto.Amount <= 0)
            return BadRequest(new { message = "Amount must be greater than 0" });

        var existing = await _context.Expenses.FindAsync(id);
        if (existing == null) return NotFound();

        existing.UpdateFrom(expenseDto);
        await _context.SaveChangesAsync();
        return Ok(existing.ToDto());
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _expenseService.DeleteAsync(id);
        if (!deleted) return NotFound();
        return Ok();
    }
}
