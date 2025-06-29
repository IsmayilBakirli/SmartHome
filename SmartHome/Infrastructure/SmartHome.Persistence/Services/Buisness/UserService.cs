using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using SmartHome.Application.DTOs.User;
using SmartHome.Application.Exceptions;
using SmartHome.Application.Common;
using SmartHome.Domain.Entities.Identity;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using SmartHome.Application.Services.Contract.Buisness;

public class UserService : IUserService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IConfiguration _configuration;
    private readonly IJwtService _jwtService;


    public UserService(UserManager<AppUser> userManager, IConfiguration configuration,IJwtService jwtService)
    {
        _userManager = userManager;
        _configuration = configuration;
        _jwtService = jwtService;
    }

    public async Task CreateMemberAsync(CreateUserDto dto, ClaimsPrincipal currentUser)
    {
        var userEntity = await _userManager.GetUserAsync(currentUser);

        var user = new AppUser
        {
            Email = dto.Email,
            UserName = dto.UserName,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            HostId = currentUser.IsInRole("Host") ? userEntity.Id : null
        };

        var result = await _userManager.CreateAsync(user, dto.Password);
        if (!result.Succeeded)
        {
            var errors = string.Join("; ", result.Errors.Select(e => e.Description));
            throw new BadRequestException(errors);
        }

        await _userManager.AddToRoleAsync(user, "Member");
        
    }

    public async Task CreateHostAsync(CreateUserDto dto)
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
    }

    public async Task<string> LoginAsync(LoginDto dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.Email);
        if (user == null)
            throw new NotFoundException("User not found");

        var passwordValid = await _userManager.CheckPasswordAsync(user, dto.Password);
        if (!passwordValid)
            throw new BadRequestException("Invalid password");

        var roles = await _userManager.GetRolesAsync(user);

        var tokenString = _jwtService.GenerateToken(user, roles);

        return tokenString;
    }

    public async Task<List<object>> GetUsersAsync(ClaimsPrincipal currentUser)
    {
        var current = await _userManager.GetUserAsync(currentUser);

        List<AppUser> users;

        if (currentUser.IsInRole("Admin"))
        {
            users = _userManager.Users.ToList();
        }
        else if (currentUser.IsInRole("Host"))
        {
            users = _userManager.Users.Where(u => u.HostId == current.Id).ToList();
        }
        else
        {
            throw new UnauthorizedAccessException("Forbidden");
        }

        var result = new List<object>();

        foreach (var user in users)
        {
            var roles = await _userManager.GetRolesAsync(user);

            result.Add(new
            {
                user.Id,
                user.UserName,
                user.Email,
                user.FirstName,
                user.LastName,
                Roles = roles,
                user.HostId
            });
        }

        return result.ToList();
    }


}
