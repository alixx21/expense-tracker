using AutoMapper;
using ExpenseTracker.API.Dtos;
using ExpenseTracker.API.Models;

namespace ExpenseTracker.API.Mappers;

public class CategoryProfile : Profile
{
    public CategoryProfile()
    {
        CreateMap<Category, CategoryDto>();
        CreateMap<CategoryDto, Category>()
            .ForMember(dest => dest.Id, opt => opt.Condition(src => src.Id != 0));
    }
}
