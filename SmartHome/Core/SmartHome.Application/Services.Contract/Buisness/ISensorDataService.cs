using SmartHome.Application.DTOs.Device;
using SmartHome.Application.DTOs.SensorData;

namespace SmartHome.Application.Services.Contract.Buisness
{
    public interface ISensorDataService
    {
        Task AddSensorReadingAsync(int deviceId,SensorDataCreateDto dto);
        Task<List<SensorDataGetDto>> GetRecentSensorReadingsAsync(int deviceId,int page,int pageSize);

        Task<SensorDataGetDto> GetLatestSensorReadingAsync(int deviceId);

        Task SetDeviceStatusAsync(int deviceId, DeviceStatusDto dto);

    }
}