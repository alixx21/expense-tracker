using ExpenseTracker.API.Data;
using ExpenseTracker.API.Models;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.API.Services;

public class ExpenseService : IExpenseService
{
    private readonly AppDbContext _context;

    public ExpenseService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ExpenseListItem>> GetAllAsync()
    {
        return await _context.Expenses
            .Include(e => e.Category)
            .Select(e => new ExpenseListItem
            {
                Id = e.Id,
                Title = e.Title,
                Amount = e.Amount,
                ExpenseDate = e.ExpenseDate,
                CategoryName = e.Category != null ? e.Category.Name : "",
                CategoryId = e.CategoryId
            })
            .ToListAsync();
    }

    public async Task<ExpenseListItem?> GetByIdAsync(int id)
    {
        return await _context.Expenses
            .Include(e => e.Category)
            .Where(e => e.Id == id)
            .Select(e => new ExpenseListItem
            {
                Id = e.Id,
                Title = e.Title,
                Amount = e.Amount,
                ExpenseDate = e.ExpenseDate,
                CategoryName = e.Category != null ? e.Category.Name : "",
                CategoryId = e.CategoryId
            })
            .FirstOrDefaultAsync();
    }

    public async Task<Expense> CreateAsync(Expense expense)
    {
        expense.ExpenseDate = DateTime.SpecifyKind(expense.ExpenseDate, DateTimeKind.Utc);
        _context.Expenses.Add(expense);
        await _context.SaveChangesAsync();
        return expense;
    }

    public async Task<Expense?> UpdateAsync(int id, Expense expense)
    {
        var existing = await _context.Expenses.FindAsync(id);
        if (existing == null) return null;

        existing.Title = expense.Title;
        existing.Amount = expense.Amount;
        existing.ExpenseDate = DateTime.SpecifyKind(expense.ExpenseDate, DateTimeKind.Utc);
        existing.CategoryId = expense.CategoryId;
        await _context.SaveChangesAsync();
        return existing;
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
