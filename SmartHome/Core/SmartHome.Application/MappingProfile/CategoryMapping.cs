using SmartHome.Application.DTOs.Category;
using SmartHome.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome.Application.MappingProfile
{
    public static class CategoryMapping
    {
        public static IQueryable<CategoryGetDto> MapToCategoryGetDtos(this IQueryable<Category> categories)
        {
            return categories.Select((category) =>
            new CategoryGetDto
            {
                Id=category.Id,
                Name=category.Name,
                Description=category.Description
            });
        }
        public static Category MapToCategory(this CategoryCreateDto categoryCreateDto)
        {
            return new Category
            {
                Name = categoryCreateDto.Name,
                Description = categoryCreateDto.Description
            };
        }
    }
}
