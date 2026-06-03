namespace ExpenseTracker.API.Dtos;

public class ExpenseRequestDto
{
    public string Title { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public DateTime ExpenseDate { get; set; }
    public int CategoryId { get; set; }
}
