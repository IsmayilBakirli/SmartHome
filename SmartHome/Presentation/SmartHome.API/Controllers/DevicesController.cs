using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartHome.Application.Common;
using SmartHome.Application.Common.Constants;
using SmartHome.Application.DTOs.Device;
using SmartHome.Application.DTOs.SensorData;
using SmartHome.Application.Services.Contract;

namespace SmartHome.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DevicesController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        public DevicesController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }
        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var data = await _serviceManager.DeviceService.GetAllAsync();
            return Ok(new ApiResponse(ResponseCodes.Success, ResponseMessages.DevicesRetrieved, data));
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] DeviceCreateDto dto)
        {
            await _serviceManager.DeviceService.CreateAsync(dto);
            return Ok(new ApiResponse(ResponseCodes.Success, ResponseMessages.DeviceCreated, null));
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("assignHost")]
        public async Task<IActionResult> AssignToHostDevice([FromBody] AssignHostDto dto)
        {
            await _serviceManager.DeviceUserService.AssignDeviceToHost(dto);
            return Ok(new ApiResponse(ResponseCodes.Success, ResponseMessages.DeviceAssignedToUser, null));
        }

        [Authorize(Roles = "Host")]
        [HttpPost("assignMember")]
        public async Task<IActionResult> AssignToMemberDevice([FromBody] AssignMemberDto dto)
        {
            await _serviceManager.DeviceUserService.AssignDeviceToMember(dto);
            return Ok(new ApiResponse(ResponseCodes.Success, ResponseMessages.DeviceAssignedToMember, null));
        }


        [HttpGet("Details/{deviceId}")]
        public async Task<IActionResult> Detail(int deviceId)
        {
            var deviceDetails = await _serviceManager.DeviceService.GetDeviceDetailsAsync(deviceId);
            return Ok(new ApiResponse(ResponseCodes.Success, ResponseMessages.DeviceRetrieved, deviceDetails));
        }

        [Authorize(Roles = "Admin,Host")]
        [HttpDelete("Delete/{deviceId}")]
        public async Task<IActionResult> Delete(int deviceId)
        {
            await _serviceManager.DeviceService.DeleteAsync(deviceId);
            return Ok(new ApiResponse(ResponseCodes.Success, ResponseMessages.DeviceDeleted, null));
        }

        [Authorize(Roles = "Admin,Host")]
        [HttpPut("Update/{deviceId}")]
        public async Task<IActionResult> Update(int deviceId, [FromBody] DeviceUpdateDto updateDto)
        {
            await _serviceManager.DeviceService.UpdateAsync(deviceId, updateDto);
            return Ok(new ApiResponse(ResponseCodes.Success, ResponseMessages.DeviceUpdated, null));
        }

        [HttpPost("{deviceId}/addreading")]
        public async Task<IActionResult> AddReading(int deviceId, [FromBody] SensorDataCreateDto dto)
        {
            await _serviceManager.SensorDataService.AddSensorReadingAsync(deviceId, dto);
            return Ok(new ApiResponse(ResponseCodes.Success, ResponseMessages.SensorReadingAdded, null));
        }

        [HttpGet("{id}/getreadings")]
        public async Task<IActionResult> GetRecentReadings(int id, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var readings = await _serviceManager.SensorDataService.GetRecentSensorReadingsAsync(id, page, pageSize);
            return Ok(new ApiResponse(ResponseCodes.Success, ResponseMessages.SensorReadingsFetched, readings));
        }


        [HttpGet("{id}/latestreading")]
        public async Task<IActionResult> GetLatestReading(int id)
        {
            var reading = await _serviceManager.SensorDataService.GetLatestSensorReadingAsync(id);
            return Ok(new ApiResponse(ResponseCodes.Success, ResponseMessages.LatestSensorReadingFetched, reading));
        }

        [Authorize(Roles = "Admin,Host")]
        [HttpPut("{deviceId}/setStatus")]
        public async Task<IActionResult> SetDeviceStatus(int deviceId, [FromBody] DeviceStatusDto dto)
        {
            await _serviceManager.SensorDataService.SetDeviceStatusAsync(deviceId, dto);
            return Ok(new ApiResponse(ResponseCodes.Success, ResponseMessages.SensorStatusUpdated, null));
        }
    }
}
