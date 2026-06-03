using ExpenseTracker.API.Data;
using ExpenseTracker.API.Models;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.API.Services;

public interface IExpenseService
{
    Task<IEnumerable<Expense>> GetAllAsync(string? sortBy = null, string? sortDirection = null);
    Task<Expense?> GetByIdAsync(int id);
    Task<Expense?> GetByIdWithCategoryAsync(int id);
    Task<bool> DeleteAsync(int id);
}

public class ExpenseService : IExpenseService
{
    private readonly AppDbContext _context;

    public ExpenseService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Expense>> GetAllAsync(string? sortBy = null, string? sortDirection = null)
    {
        var query = _context.Expenses.Include(e => e.Category).AsQueryable();

        if (!string.IsNullOrEmpty(sortBy))
        {
            var isDescending = sortDirection?.ToLower() == "desc";
            query = sortBy.ToLower() switch
            {
                "title" => isDescending ? query.OrderByDescending(e => e.Title) : query.OrderBy(e => e.Title),
                "amount" => isDescending ? query.OrderByDescending(e => e.Amount) : query.OrderBy(e => e.Amount),
                "date" => isDescending ? query.OrderByDescending(e => e.ExpenseDate) : query.OrderBy(e => e.ExpenseDate),
                "expensedate" => isDescending ? query.OrderByDescending(e => e.ExpenseDate) : query.OrderBy(e => e.ExpenseDate),
                "category" => isDescending ? query.OrderByDescending(e => e.Category != null ? e.Category.Name : "") : query.OrderBy(e => e.Category != null ? e.Category.Name : ""),
                "categoryname" => isDescending ? query.OrderByDescending(e => e.Category != null ? e.Category.Name : "") : query.OrderBy(e => e.Category != null ? e.Category.Name : ""),
                _ => query
            };
        }

        return await query.ToListAsync();
    }

    public async Task<Expense?> GetByIdAsync(int id)
    {
        return await _context.Expenses.FindAsync(id);
    }

    public async Task<Expense?> GetByIdWithCategoryAsync(int id)
    {
        return await _context.Expenses
            .Include(e => e.Category)
            .FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var expense = await _context.Expenses.FindAsync(id);
        if (expense == null) return false;

        _context.Expenses.Remove(expense);
        await _context.SaveChangesAsync();
        return true;
    }
}
