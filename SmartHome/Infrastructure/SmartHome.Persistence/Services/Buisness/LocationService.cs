using Microsoft.EntityFrameworkCore;
using SmartHome.Application.DTOs.Location;
using SmartHome.Application.Exceptions;
using SmartHome.Application.MappingProfile;
using SmartHome.Application.Repositories.Contract;
using SmartHome.Application.Services.Contract.Buisness;

namespace SmartHome.Persistence.Services.Buisness
{
    public class LocationService : ILocationService
    {
        private readonly IRepositoryManager _repositoryManager;
        public LocationService(IRepositoryManager repositoryManager)
        {
            _repositoryManager = repositoryManager;
        }
        public async Task CreateAsync(LocationCreateDto entity)
        {

            await _repositoryManager.LocationRepository.CreateAsync(entity.MapToLocation());
        }

        public async Task DeleteAsync(int id)
        {
            var data = await _repositoryManager.LocationRepository.FindByIdAsync(id);

            if (data == null)
                throw new NotFoundException($"Location with ID {id} not found.");

            await _repositoryManager.LocationRepository.DeleteAsync(data);
        }

        public async Task<List<LocationGetDto>> GetAllAsync()
        {
            var data= await _repositoryManager.LocationRepository.GetAll().MapToLocationDtos().ToListAsync();
            if (data == null)
            {
                throw new NotFoundException("No categories found.");
            }
            return data;

        }
    }
}
