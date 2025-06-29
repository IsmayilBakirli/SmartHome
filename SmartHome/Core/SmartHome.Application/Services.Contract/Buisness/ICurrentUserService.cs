using SmartHome.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome.Application.Services.Contract.Buisness
{
    public interface ICurrentUserService
    {
        string GetUserId();
        List<string>  GetRoles();
        Task<AppUser> GetUser();
        Task<bool> IsInRole(string role);
    }
}
