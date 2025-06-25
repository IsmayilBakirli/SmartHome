using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartHome.Application.Common;
using SmartHome.Application.DTOs.Device;
using SmartHome.Application.DTOs.SensorData;
using SmartHome.Application.Services.Contract;
using SmartHome.Domain.Entities;
using System.Security.Claims;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SmartHome.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DevicesController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;
        public DevicesController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        [Authorize]
        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var data = await _serviceManager.DeviceService.GetAllAsync();
            return Ok(new ApiResponse(200, "Devices retrieved successfully", data));
        }

        [Authorize(Roles ="Admin")]
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody]DeviceCreateDto dto)
        {
            await _serviceManager.DeviceService.CreateAsync(dto);
            return Ok(new ApiResponse(200, "Category created successfully", null));

        }


        [Authorize(Roles = "Admin")]
        [HttpPost("assignHost")]
        public async Task<IActionResult> AssignToHostDevice([FromBody]AssignHostDto dto)
        {
            await _serviceManager.DeviceUserService.AssignDeviceToHost(dto);
            return Ok(new ApiResponse(200, "Device assigned to user successfully", null));
        }


        [Authorize(Roles = "Host")]
        [HttpPost("assignMember")]
        public async Task<IActionResult> AssignToMemberDevice([FromBody]AssignMemberDto dto)
        {
            await _serviceManager.DeviceUserService.AssignDeviceToMember(dto);
            return Ok(new ApiResponse(200, "Device assigned to member successfully", null));
        }


        [Authorize]
        [HttpGet("Details/{deviceId}")]
        public async Task<IActionResult> Detail(int deviceId)
        {

            var deviceDetails = await _serviceManager.DeviceService.GetDeviceDetailsAsync(deviceId);
            return Ok(new ApiResponse(200, "Device retrieved successfully", deviceDetails));
        }


        [Authorize(Roles ="Admin,Host")]
        [HttpDelete("Delete/{deviceId}")]
        public async Task<IActionResult> Delete(int deviceId)
        {
            await _serviceManager.DeviceService.DeleteAsync(deviceId);
            return Ok(new ApiResponse(200, "Device deleted successfully",null));
        }



        [Authorize(Roles = "Admin,Host")]
        [HttpPut("Update/{deviceId}")]
        public async Task<IActionResult> Update(int deviceId, [FromBody] DeviceUpdateDto updateDto)
        {
            await _serviceManager.DeviceService.UpdateAsync(deviceId, updateDto);
            return Ok(new ApiResponse(200, "Device updated successfully", null));
        }

        [Authorize]
        [HttpPost("{deviceId}/addreading")]
        public async Task<IActionResult> AddReading(int deviceId, [FromBody] SensorDataCreateDto dto)
        {
            await _serviceManager.SensorDataService.AddSensorReadingAsync(deviceId, dto);
            return Ok(new ApiResponse(200, "Sensor reading added successfully", null));
        }


        [Authorize]
        [HttpGet("{id}/getreadings")]
        public async Task<IActionResult> GetRecentReadings(int id, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var readings = await _serviceManager.SensorDataService.GetRecentSensorReadingsAsync(id, page, pageSize);
            return Ok(new ApiResponse(200, "Recent sensor readings fetched successfully", readings));
        }


        [Authorize]
        [HttpGet("{id}/latestreading")]
        public async Task<IActionResult> GetLatestReading(int id)
        {
            var reading = await _serviceManager.SensorDataService.GetLatestSensorReadingAsync(id);
            return Ok(new ApiResponse(200, "Latest sensor reading fetched successfully", reading));
        }


        [Authorize(Roles ="Admin,Host")]
        [HttpPut("{deviceId}/setStatus")]
        public async Task<IActionResult> SetDeviceStatus(int deviceId, [FromBody] DeviceStatusDto dto)
        {
            await _serviceManager.SensorDataService.SetDeviceStatusAsync(deviceId, dto);
            return Ok(new ApiResponse(200, "Sensor reading updated successfully", null));

        }

    }
}
