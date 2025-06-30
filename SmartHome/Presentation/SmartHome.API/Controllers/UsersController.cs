using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartHome.Application.Common;
using SmartHome.Application.Common.Constants;
using SmartHome.Application.DTOs.User;
using SmartHome.Application.Services.Contract;

namespace SmartHome.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        public UsersController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        [Authorize(Roles = "Admin,Host")]
        [HttpPost("create-member")]
        public async Task<IActionResult> CreateMember([FromBody] CreateUserDto dto)
        {
            await _serviceManager.UserService.CreateMemberAsync(dto, User);
            return Ok(new ApiResponse(ResponseCodes.Success, ResponseMessages.MemberCreated));
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("create-host")]
        public async Task<IActionResult> CreateHost([FromBody] CreateUserDto dto)
        {
            await _serviceManager.UserService.CreateHostAsync(dto);
            return Ok(new ApiResponse(ResponseCodes.Success, ResponseMessages.HostCreated));
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var response = await _serviceManager.UserService.LoginAsync(dto);
            return Ok(new ApiResponse(ResponseCodes.Success,ResponseMessages.LoginSuccess,response));
        }

        [Authorize(Roles = "Admin,Host")]
        [HttpGet("all")]
        public async Task<IActionResult> GetUsers()
        {
            var response = await _serviceManager.UserService.GetUsersAsync(User);
            return Ok(new ApiResponse(ResponseCodes.Success,ResponseMessages.UsersRetrieved,response)); 
        }
    }
}
