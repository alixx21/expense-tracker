using ExpenseTracker.API.Dtos;
using ExpenseTracker.API.Models;

namespace ExpenseTracker.API.Mappers;

public static class DtoMapper
{
    public static CategoryDto ToDto(this Category category) => new()
    {
        Id = category.Id,
        Name = category.Name
    };

    public static Category ToEntity(this CategoryDto dto) => new()
    {
        Id = dto.Id,
        Name = dto.Name
    };

    public static void UpdateFrom(this Category category, CategoryDto dto)
    {
        category.Name = dto.Name;
    }

    public static ExpenseResponseDto ToDto(this Expense expense) => new()
    {
        Id = expense.Id,
        Title = expense.Title,
        Amount = expense.Amount,
        ExpenseDate = expense.ExpenseDate,
        CategoryId = expense.CategoryId,
        CategoryName = expense.Category?.Name ?? string.Empty
    };

    public static Expense ToEntity(this ExpenseRequestDto dto) => new()
    {
        Title = dto.Title,
        Amount = dto.Amount,
        ExpenseDate = DateTime.SpecifyKind(dto.ExpenseDate, DateTimeKind.Utc),
        CategoryId = dto.CategoryId
    };

    public static void UpdateFrom(this Expense expense, ExpenseRequestDto dto)
    {
        expense.Title = dto.Title;
        expense.Amount = dto.Amount;
        expense.ExpenseDate = DateTime.SpecifyKind(dto.ExpenseDate, DateTimeKind.Utc);
        expense.CategoryId = dto.CategoryId;
    }
}
