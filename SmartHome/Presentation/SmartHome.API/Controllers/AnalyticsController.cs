using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartHome.Application.Common;
using SmartHome.Application.Services.Contract;

namespace SmartHome.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnalyticsController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;
        public AnalyticsController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }


        [Authorize(Roles ="Admin,Host")]
        [HttpGet("devices/{deviceId}/energy-usage")]
        public async Task<IActionResult> GetTotalEnergyConsumption(int deviceId)
        {
            var totalEnergyConsumption = await _serviceManager.AnalyticsService.GetTotalEnergyConsumptionAsync(deviceId);
            return Ok(new ApiResponse(200, "Total energy consumption fetched successfully.", totalEnergyConsumption));
        }


        [Authorize(Roles = "Admin,Host")] 
        [HttpGet("device-status")]
        public async Task<IActionResult> GetDeviceStatus()
        {
            var deviceStatus = await _serviceManager.AnalyticsService.GetDeviceStatusAsync();
            return Ok(new ApiResponse(200, "Device status fetched successfully.", deviceStatus));
        }


        [Authorize(Roles = "Admin,Host")]
        [HttpGet("location/usage")]
        public async Task<IActionResult> GetLocationUsage()
        {
            var locationUsage = await _serviceManager.AnalyticsService.GetLocationUsageAsync();
            return Ok(new ApiResponse(200, "Location usage fetched successfully.", locationUsage));
        }


        [Authorize(Roles ="Admin,Host")]
        [HttpGet("devices/health")]
        public async Task<IActionResult> GetHealth()
        {
            var data =await _serviceManager.DeviceHealthService.GetDeviceHealthReport();
            return Ok(data);
        }

    }
}
