namespace ExpenseTracker.API.Models;

public class ExpenseListItem
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public DateTime ExpenseDate { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public int CategoryId { get; set; }
}
