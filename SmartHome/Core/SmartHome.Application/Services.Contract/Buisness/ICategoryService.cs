using SmartHome.Application.DTOs.Category;
using SmartHome.Application.DTOs.SensorData;

namespace SmartHome.Application.Services.Contract.Buisness
{
    public interface ICategoryService
    {
        Task<List<CategoryGetDto>> GetAllAsync();
        Task CreateAsync(CategoryCreateDto entity);
        Task DeleteAsync(int id);
    }
}
