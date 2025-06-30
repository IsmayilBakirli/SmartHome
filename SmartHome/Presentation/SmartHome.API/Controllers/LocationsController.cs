using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartHome.Application.Common;
using SmartHome.Application.Common.Constants;
using SmartHome.Application.DTOs.Location;
using SmartHome.Application.Services.Contract;

namespace SmartHome.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LocationsController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        public LocationsController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var data = await _serviceManager.LocationService.GetAllAsync();
            return Ok(new ApiResponse(ResponseCodes.Success, ResponseMessages.LocationsRetrieved, data));
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] LocationCreateDto entity)
        {
            await _serviceManager.LocationService.CreateAsync(entity);
            return Ok(new ApiResponse(ResponseCodes.Success, ResponseMessages.LocationCreated, null));
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _serviceManager.LocationService.DeleteAsync(id);
            return Ok(new ApiResponse(ResponseCodes.Success, ResponseMessages.LocationDeleted, null));
        }

        [HttpGet("{locationId}/devices")]
        public async Task<IActionResult> GetDevicesByLocation(int locationId)
        {
            var result = await _serviceManager.DeviceService.GetDevicesByLocationAsync(locationId);
            return Ok(new ApiResponse(ResponseCodes.Success, ResponseMessages.DevicesByLocationRetrieved, result));
        }
    }
}
