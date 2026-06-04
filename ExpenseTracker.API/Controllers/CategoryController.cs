using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using ExpenseTracker.API.Data;
using ExpenseTracker.API.Dtos;
using ExpenseTracker.API.Models;
using ExpenseTracker.API.Services;

namespace ExpenseTracker.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _categoryService;
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public CategoryController(ICategoryService categoryService, AppDbContext context, IMapper mapper)
    {
        _categoryService = categoryService;
        _context = context;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var categories = await _categoryService.GetAllAsync();
        return Ok(categories.Select(c => _mapper.Map<CategoryDto>(c)));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var category = await _categoryService.GetByIdAsync(id);
        if (category == null) return NotFound();
        return Ok(_mapper.Map<CategoryDto>(category));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CategoryDto categoryDto)
    {
        if (string.IsNullOrWhiteSpace(categoryDto.Name))
            return BadRequest(new { message = "Category name is required" });

        var category = _mapper.Map<Category>(categoryDto);
        _context.Categories.Add(category);
        await _context.SaveChangesAsync();
        return Ok(_mapper.Map<CategoryDto>(category));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] CategoryDto categoryDto)
    {
        if (string.IsNullOrWhiteSpace(categoryDto.Name))
            return BadRequest(new { message = "Category name is required" });

        var existing = await _context.Categories.FindAsync(id);
        if (existing == null) return NotFound();

        _mapper.Map(categoryDto, existing);
        await _context.SaveChangesAsync();
        return Ok(_mapper.Map<CategoryDto>(existing));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _categoryService.DeleteAsync(id);
        if (!deleted) return NotFound();
        return Ok();
    }
}
