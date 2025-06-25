using System.ComponentModel.DataAnnotations;

namespace SmartHome.Application.DTOs.Category
{
    public class CategoryCreateDto
    {
        [Required(ErrorMessage = "Category name is required.")]
        [MaxLength(50, ErrorMessage = "Category name cannot exceed 50 characters.")]
        public string Name { get; set; }

        [MaxLength(255, ErrorMessage = "Description cannot exceed 255 characters.")]
        public string Description { get; set; }
    }
}
