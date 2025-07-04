﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SmartHome.Application.DTOs.Device;
using SmartHome.Application.Exceptions;
using SmartHome.Application.Repositories.Contract;
using SmartHome.Application.Services.Contract;
using SmartHome.Application.Services.Contract.Buisness;
using SmartHome.Domain.Entities;
using SmartHome.Domain.Entities.Identity;

namespace SmartHome.Persistence.Services.Buisness
{
    public class DeviceHealthService : IDeviceHealthService
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IServiceManager _serviceManager;
        private readonly UserManager<AppUser> _userManager; // UserManager daxil edirik

        public DeviceHealthService(IRepositoryManager repositoryManager, IServiceManager serviceManager, UserManager<AppUser> userManager)
        {
            _repositoryManager = repositoryManager;
            _userManager = userManager;
            _serviceManager = serviceManager;
        }

        public async Task<List<DeviceHealthReportDto>> GetDeviceHealthReport()
        {
            var user =await _serviceManager.CurrentUserService.GetUser();
            var userRole =_serviceManager.CurrentUserService.GetRoles().FirstOrDefault();   

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
