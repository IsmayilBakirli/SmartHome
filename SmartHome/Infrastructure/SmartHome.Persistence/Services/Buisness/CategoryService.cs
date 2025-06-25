using Microsoft.EntityFrameworkCore;
using SmartHome.Application.Exceptions;
using SmartHome.Application.DTOs.Category;
using SmartHome.Application.MappingProfile;
using SmartHome.Application.Repositories.Contract;
using SmartHome.Application.Services.Contract.Buisness;

namespace SmartHome.Persistence.Services.Buisness
{
    public class CategoryService : ICategoryService
    {
        private readonly IRepositoryManager _repositoryManager;
        public CategoryService(IRepositoryManager repositoryManager)
        {
            _repositoryManager = repositoryManager;
        }
        public async Task CreateAsync(CategoryCreateDto entity)
        {

            await _repositoryManager.CategoryRepository.CreateAsync(entity.MapToCategory());
        }

        public async Task DeleteAsync(int id)
        {
            var data = await _repositoryManager.CategoryRepository.FindByIdAsync(id);

            if (data == null)
                throw new NotFoundException($"Category with ID {id} not found.");

            await _repositoryManager.CategoryRepository.DeleteAsync(data);
        }

        public async Task<List<CategoryGetDto>> GetAllAsync()
        {
            var data = await _repositoryManager.CategoryRepository.GetAll().MapToCategoryGetDtos().ToListAsync();
            if (data == null)
            {
                throw new NotFoundException("No categories found.");
            }
            return data;
        }

   
    }
}
