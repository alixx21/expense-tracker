using Microsoft.AspNetCore.Mvc;
using ExpenseTracker.API.Data;
using ExpenseTracker.API.Models;
using ExpenseTracker.API.Services;

namespace ExpenseTracker.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _categoryService;
    private readonly AppDbContext _context;

    public CategoryController(ICategoryService categoryService, AppDbContext context)
    {
        _categoryService = categoryService;
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var categories = await _categoryService.GetAllAsync();
        return Ok(categories);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var category = await _categoryService.GetByIdAsync(id);
        if (category == null) return NotFound();
        return Ok(category);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Category category)
    {
        if (string.IsNullOrWhiteSpace(category.Name))
            return BadRequest(new { message = "Category name is required" });

        _context.Categories.Add(category);
        await _context.SaveChangesAsync();
        return Ok(category);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] Category category)
    {
        if (string.IsNullOrWhiteSpace(category.Name))
            return BadRequest(new { message = "Category name is required" });

        var existing = await _context.Categories.FindAsync(id);
        if (existing == null) return NotFound();

        existing.Name = category.Name;
        await _context.SaveChangesAsync();
        return Ok(existing);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _categoryService.DeleteAsync(id);
        if (!deleted) return NotFound();
        return Ok();
    }
}
