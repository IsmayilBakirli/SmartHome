using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using SmartHome.Application.Services.Contract.Buisness;
using SmartHome.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome.Persistence.Services.Buisness
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<AppUser> _userManager;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor,
                                  UserManager<AppUser> userManager)
        {
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }

        public string GetUserId()
        {
            return _httpContextAccessor.HttpContext?.User?
                .FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }

        public List<string> GetRoles()
        {
            return _httpContextAccessor.HttpContext?.User?
                .FindAll(ClaimTypes.Role)
                .Select(r => r.Value)
                .ToList() ?? new List<string>();
        }

        public async Task<AppUser> GetUser()
        {
            var userId=GetUserId();
            AppUser user = await _userManager.FindByIdAsync(userId);
            return user;
        }

        public async Task<bool> IsInRole(string role)
        {
            var user = await GetUser();
            return await _userManager.IsInRoleAsync(user, role);
        }
    }

}
