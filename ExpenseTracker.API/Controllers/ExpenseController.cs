using Microsoft.AspNetCore.Mvc;
using ExpenseTracker.API.Data;
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
        var expenseList = expenses.Select(e => new ExpenseListItem
        {
            Id = e.Id,
            Title = e.Title,
            Amount = e.Amount,
            ExpenseDate = e.ExpenseDate,
            CategoryName = e.Category != null ? e.Category.Name : "",
            CategoryId = e.CategoryId
        }).ToList();
        return Ok(expenseList);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var expense = await _expenseService.GetByIdWithCategoryAsync(id);
        if (expense == null) return NotFound();
        
        var expenseItem = new ExpenseListItem
        {
            Id = expense.Id,
            Title = expense.Title,
            Amount = expense.Amount,
            ExpenseDate = expense.ExpenseDate,
            CategoryName = expense.Category != null ? expense.Category.Name : "",
            CategoryId = expense.CategoryId
        };
        return Ok(expenseItem);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Expense expense)
    {
        if (string.IsNullOrWhiteSpace(expense.Title))
            return BadRequest(new { message = "Title is required" });
        
        if (expense.Amount <= 0)
            return BadRequest(new { message = "Amount must be greater than 0" });

        expense.ExpenseDate = DateTime.SpecifyKind(expense.ExpenseDate, DateTimeKind.Utc);
        _context.Expenses.Add(expense);
        await _context.SaveChangesAsync();
        return Ok(expense);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] Expense expense)
    {
        if (string.IsNullOrWhiteSpace(expense.Title))
            return BadRequest(new { message = "Title is required" });
        
        if (expense.Amount <= 0)
            return BadRequest(new { message = "Amount must be greater than 0" });

        var existing = await _context.Expenses.FindAsync(id);
        if (existing == null) return NotFound();

        existing.Title = expense.Title;
        existing.Amount = expense.Amount;
        existing.ExpenseDate = DateTime.SpecifyKind(expense.ExpenseDate, DateTimeKind.Utc);
        existing.CategoryId = expense.CategoryId;
        await _context.SaveChangesAsync();
        return Ok(existing);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _expenseService.DeleteAsync(id);
        if (!deleted) return NotFound();
        return Ok();
    }
}
