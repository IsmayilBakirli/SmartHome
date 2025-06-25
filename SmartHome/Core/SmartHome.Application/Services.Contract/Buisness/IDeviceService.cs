using SmartHome.Application.DTOs.Device;
namespace SmartHome.Application.Services.Contract.Buisness
{
    public interface IDeviceService
    {
        Task<List<DeviceGetDto>> GetAllAsync();

        Task<DeviceGetDto> GetDeviceDetailsAsync(int id);
        Task CreateAsync(DeviceCreateDto entity);
        Task UpdateAsync(int id,DeviceUpdateDto entity);
        Task DeleteAsync(int id);
        Task<List<DeviceGetDto>> GetDevicesByLocationAsync(int locationId);

    }
}