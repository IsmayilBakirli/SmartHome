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
    [Authorize(Roles = "Admin")]
    public class CategoriesController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        public CategoriesController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        [HttpGet("All")]
        public async Task<IActionResult> GetAll()
        {
            var data = await _serviceManager.CategoryService.GetAllAsync();
            return Ok(new ApiResponse(ResponseCodes.Success, ResponseMessages.CategoriesRetrieved, data));
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] CategoryCreateDto entity)
        {
            await _serviceManager.CategoryService.CreateAsync(entity);
            return Ok(new ApiResponse(ResponseCodes.Success, ResponseMessages.CategoryCreated, null));
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _serviceManager.CategoryService.DeleteAsync(id);
            return Ok(new ApiResponse(ResponseCodes.Success, ResponseMessages.CategoryDeleted, null));
        }
    }
}
