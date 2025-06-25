using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using SmartHome.Application.Common;
using SmartHome.Application.DTOs.Location;
using SmartHome.Application.Services.Contract;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SmartHome.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
            return Ok(new ApiResponse(200, "Devices retrieved successfully", data));
        }


        [Authorize(Roles = "Admin")]
        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] LocationCreateDto entity)
        {
            await _serviceManager.LocationService.CreateAsync(entity);
            return Ok(new ApiResponse(200, "successfully added", null));
        }


        [Authorize(Roles = "Admin")]
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _serviceManager.LocationService.DeleteAsync(id);
            return Ok(new ApiResponse(200, "Category deleted successfully", null));
        }   

        [Authorize] 
        [HttpGet("{locationId}/devices")]
        public async Task<IActionResult> GetDevicesByLocation(int locationId)
        {
            var result = await _serviceManager.DeviceService.GetDevicesByLocationAsync(locationId);
            return Ok(new ApiResponse(200, "Devices retrieved successfully", result));
        }

    }
}
