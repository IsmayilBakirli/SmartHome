using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SmartHome.Application.DTOs.Device;
using SmartHome.Application.Exceptions;
using SmartHome.Application.Repositories.Contract;
using SmartHome.Application.Services.Contract.Buisness;
using SmartHome.Domain.Entities;
using SmartHome.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome.Persistence.Services.Buisness
{
    public class DeviceHealthService : IDeviceHealthService
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<AppUser> _userManager; // UserManager daxil edirik

        public DeviceHealthService(IRepositoryManager repositoryManager, IHttpContextAccessor httpContextAccessor, UserManager<AppUser> userManager)
        {
            _repositoryManager = repositoryManager;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }

        public async Task<List<DeviceHealthReportDto>> GetDeviceHealthReport()
        {
            var userName = _httpContextAccessor.HttpContext?.User?.Identity?.Name;
            var user = await _userManager.FindByNameAsync(userName);
            var userRole = (await _userManager.GetRolesAsync(user)).FirstOrDefault(); 

            var devices = await _repositoryManager.DeviceRepository.GetAll().ToListAsync();

            if (devices == null )
            {
                throw new NotFoundException("Devices not found");
            }

            if (userRole == "Host")
            {
                var userId = user.Id;

                var userDevices = await _repositoryManager.DeviceUserRepository
                    .FindByCondition(du => du.UserId == userId)
                    .ToListAsync();

                if (userDevices == null || !userDevices.Any())
                {
                    throw new NotFoundException("No devices found for this user");
                }

                var deviceIds = userDevices.Select(du => du.DeviceId).ToList();

                var userDeviceDetails = devices.Where(d => deviceIds.Contains(d.Id)).ToList();

                return userDeviceDetails.Select(device => new DeviceHealthReportDto
                {
                    DeviceId = device.Id,
                    DeviceName = device.Name,
                    HealthStatus = GetHealthStatus(device)
                }).ToList();
            }
            return devices.Select(device => new DeviceHealthReportDto
            {
                DeviceId = device.Id,
                DeviceName = device.Name,
                HealthStatus = GetHealthStatus(device)
            }).ToList();

        }

        private string GetHealthStatus(Device device)
        {
            if (device.CpuUsage >= 90 || device.RamUsage >= 90 || device.Temperature >= 80)
                return "Critical";  // Kritik vəziyyət
            if (device.CpuUsage >= 60 || device.RamUsage >= 60 || device.Temperature >= 50)
                return "Warning";   // xəbərdarlıq
            return "Normal";  // Normal vəziyyət
        }
    }
}
