using ExpenseTracker.API.Models;

namespace ExpenseTracker.API.Services;

public interface IExpenseService
{
    Task<IEnumerable<ExpenseListItem>> GetAllAsync();
    Task<ExpenseListItem?> GetByIdAsync(int id);
    Task<Expense> CreateAsync(Expense expense);
    Task<Expense?> UpdateAsync(int id, Expense expense);
    Task<bool> DeleteAsync(int id);
}
