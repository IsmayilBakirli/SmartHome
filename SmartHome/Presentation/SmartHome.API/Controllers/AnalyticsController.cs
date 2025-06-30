using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartHome.Application.Common;
using SmartHome.Application.Common.Constants;
using SmartHome.Application.Services.Contract;

namespace SmartHome.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin,Host")]
    public class AnalyticsController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        public AnalyticsController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }
        [HttpGet("devices/{deviceId}/energy-usage")]
        public async Task<IActionResult> GetTotalEnergyConsumption(int deviceId)
        {
            var totalEnergyConsumption = await _serviceManager.AnalyticsService.GetTotalEnergyConsumptionAsync(deviceId);
            return Ok(new ApiResponse(ResponseCodes.Success, ResponseMessages.EnergyUsageFetched, totalEnergyConsumption));
        }

        [HttpGet("device-status")]
        public async Task<IActionResult> GetDeviceStatus()
        {
            var deviceStatus = await _serviceManager.AnalyticsService.GetDeviceStatusAsync();
            return Ok(new ApiResponse(ResponseCodes.Success, ResponseMessages.DeviceStatusFetched, deviceStatus));
        }
        [HttpGet("location/usage")]
        public async Task<IActionResult> GetLocationUsage()
        {
            var locationUsage = await _serviceManager.AnalyticsService.GetLocationUsageAsync();
            return Ok(new ApiResponse(ResponseCodes.Success, ResponseMessages.LocationUsageFetched, locationUsage));
        }

 
        [HttpGet("devices/health")]
        public async Task<IActionResult> GetHealth()
        {
            var data = await _serviceManager.DeviceHealthService.GetDeviceHealthReport();
            return Ok(new ApiResponse(ResponseCodes.Success, ResponseMessages.DeviceHealthFetched, data));
        }
    }
}
