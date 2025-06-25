using SmartHome.Application.DTOs.Device;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome.Application.Services.Contract.Buisness
{
    public interface IDeviceUserService
    {
        Task AssignDeviceToHost(AssignHostDto dto);
        Task AssignDeviceToMember(AssignMemberDto dto);
    }
}