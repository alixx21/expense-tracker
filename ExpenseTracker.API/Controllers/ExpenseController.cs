using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ExpenseTracker.API.Data;
using ExpenseTracker.API.Models;
 
namespace ExpenseTracker.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExpenseController : ControllerBase
    {
        private readonly AppDbContext _context;
 
        public ExpenseController(AppDbContext context)
        {
            _context = context;
        }
 
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var expenses = await _context.Expenses
                .Include(e => e.Category)
                .Select(e => new {
                    e.Id,
                    e.Title,
                    e.Amount,
                    e.ExpenseDate,
                    CategoryName = e.Category != null ? e.Category.Name : ""
                })
                .ToListAsync();
            return Ok(expenses);
        }
 
        [HttpPost]
        public async Task<IActionResult> Create(Expense expense)
        {
            expense.ExpenseDate = DateTime.SpecifyKind(expense.ExpenseDate, DateTimeKind.Utc);
            _context.Expenses.Add(expense);
            await _context.SaveChangesAsync();
            return Ok(expense);
        }
 
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var expense = await _context.Expenses.FindAsync(id);
            if (expense == null) return NotFound();
            _context.Expenses.Remove(expense);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}