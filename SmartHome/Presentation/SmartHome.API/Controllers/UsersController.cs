using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SmartHome.Domain.Entities.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using SmartHome.Application.Exceptions;
using SmartHome.Application.Common;
using SmartHome.Application.DTOs.User; // DTO-nun namespace-i

namespace SmartHome.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IConfiguration _configuration;

        public UsersController(UserManager<AppUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        [Authorize(Roles = "Admin,Host")]
        [HttpPost("create-member")]
        public async Task<IActionResult> CreateMember([FromBody] CreateUserDto dto)
        {
            var currentUser = await _userManager.GetUserAsync(User);

            var user = new AppUser
            {
                Email = dto.Email,
                UserName = dto.UserName,
                FirstName = dto.FirstName,
                LastName = dto.LastName,

                HostId = User.IsInRole("Host") ? currentUser.Id : null
            };

            var result = await _userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
            {

                var errors = string.Join("; ", result.Errors.Select(e => e.Description));

                throw new BadRequestException(errors);
            }

            await _userManager.AddToRoleAsync(user, "Member");
            var response = new ApiResponse(
                200,
                "Member user created successfully",
                null
                );

            return Ok(response);
        }


        [Authorize(Roles ="Admin")]
        [HttpPost("create-host")]
        public async Task<IActionResult> CreateHost([FromBody] CreateUserDto dto)
        {
            var user = new AppUser
            {
                Email = dto.Email,
                UserName = dto.UserName,
                FirstName = dto.FirstName,
                LastName = dto.LastName
            };

            var result = await _userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
            {
                var errors = string.Join("; ", result.Errors.Select(e => e.Description));
                throw new BadRequestException(errors);
            }

            await _userManager.AddToRoleAsync(user, "Host");
            var response = new ApiResponse(
                   200,
                  "Member user created successfully",
                   null
                   );

            return Ok(response);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null)
                throw new NotFoundException("user not found");

            var passwordValid = await _userManager.CheckPasswordAsync(user, dto.Password);
            if (!passwordValid)
                throw new BadRequestException( "Invalid password" );

            var roles = await _userManager.GetRolesAsync(user);

            // JWT token yaratma burada
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.UserName)
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddDays(int.Parse(_configuration["Jwt:ExpireDays"])),
                signingCredentials: creds
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            var authResult = new AuthResult(
                                   token: tokenString,  
                                   user: new
                                   {
                                       Id=user.Id,
                                       UserName=user.UserName,
                                       Email=user.Email,
                                       Roles=roles
                                   }
                                   );
            var response = new ApiResponse(
                200,
                "succesfully login",
                authResult
                );
            return Ok(response);
        }


        [Authorize(Roles = "Admin,Host")]
        [HttpGet("all")]
        public async Task<IActionResult> GetUsers()
        {
            var currentUser = await _userManager.GetUserAsync(User);

            if (User.IsInRole("Admin"))
            {
                var users = _userManager.Users.ToList();

                var result = users.Select(u => new
                {
                    u.Id,
                    u.UserName,
                    u.Email,
                    u.FirstName,
                    u.LastName,
                    Roles = _userManager.GetRolesAsync(u).Result,
                    HostId = u.HostId
                });

                return Ok(result);
            }
            else if (User.IsInRole("Host"))
            {
                var members = _userManager.Users
                    .Where(u => u.HostId == currentUser.Id)
                    .ToList();

                var result = members.Select(u => new
                {
                    u.Id,
                    u.UserName,
                    u.Email,
                    u.FirstName,
                    u.LastName,
                    Roles = _userManager.GetRolesAsync(u).Result,
                    HostId = u.HostId
                });
                var response = new ApiResponse(
                    200,
                    message: "Users retrieved successfully",
                    result
                    );

                return Ok(response);
            }

            return Forbid();
        }

    }
}
