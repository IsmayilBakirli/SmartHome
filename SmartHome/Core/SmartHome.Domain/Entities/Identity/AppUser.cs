using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome.Domain.Entities.Identity
{
    public class AppUser:IdentityUser
    {
        public string? FirstName { get; set; }

        public string? LastName { get; set; }
        public string? HostId { get; set; }  

        public AppUser? Host { get; set; }
        public ICollection<AppUser>? Members { get; set; }

        public ICollection<DeviceUser>? DeviceUsers { get; set; }
    }
}
