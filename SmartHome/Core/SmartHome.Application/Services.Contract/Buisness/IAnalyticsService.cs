using SmartHome.Application.DTOs.Device;
using SmartHome.Application.DTOs.Location;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome.Application.Services.Contract.Buisness
{
    public interface IAnalyticsService
    {
        Task<double?> GetTotalEnergyConsumptionAsync(int deviceId);
        Task<DeviceStatusGetDto> GetDeviceStatusAsync();
        Task<LocationUsageDto> GetLocationUsageAsync();
    }
}
