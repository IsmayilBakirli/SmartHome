using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartHome.Application.Common;
using SmartHome.Application.Common.Constants;
using SmartHome.Application.DTOs.Category;
using SmartHome.Application.Services.Contract;

namespace SmartHome.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        public CategoriesController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("All")]
        public async Task<IActionResult> GetAll()
        {
            var data = await _serviceManager.CategoryService.GetAllAsync();
            return Ok(new ApiResponse(ResponseCodes.Success, ResponseMessages.CategoriesRetrieved, data));
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] CategoryCreateDto entity)
        {
            await _serviceManager.CategoryService.CreateAsync(entity);
            return Ok(new ApiResponse(ResponseCodes.Success, ResponseMessages.CategoryCreated, null));
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _serviceManager.CategoryService.DeleteAsync(id);
            return Ok(new ApiResponse(ResponseCodes.Success, ResponseMessages.CategoryDeleted, null));
        }
    }
}
