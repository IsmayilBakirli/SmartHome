using SmartHome.Application.DTOs.Device;
using SmartHome.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome.Application.Services.Contract.Buisness
{
    public interface IDeviceHealthService
    {
        Task<List<DeviceHealthReportDto>> GetDeviceHealthReport();
    }
}
