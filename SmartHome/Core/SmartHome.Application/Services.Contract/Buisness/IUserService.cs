using SmartHome.Application.DTOs.User;
using System.Security.Claims;

namespace SmartHome.Application.Services.Contract.Buisness
{
    public interface IUserService
    {
        Task CreateMemberAsync(CreateUserDto dto, ClaimsPrincipal currentUser);
        Task CreateHostAsync(CreateUserDto dto);
        Task<string> LoginAsync(LoginDto dto);
        Task<List<object>> GetUsersAsync(ClaimsPrincipal currentUser);
    }

}
