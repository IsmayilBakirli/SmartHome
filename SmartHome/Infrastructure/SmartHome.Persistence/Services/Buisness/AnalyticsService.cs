using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SmartHome.Application.DTOs.Device;
using SmartHome.Application.DTOs.Location;
using SmartHome.Application.Exceptions;
using SmartHome.Application.Repositories.Contract;
using SmartHome.Application.Services.Contract.Buisness;
using SmartHome.Domain.Entities;
using SmartHome.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome.Persistence.Services.Buisness
{
    public class AnalyticsService:IAnalyticsService
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<AppUser> _userManager;
        

        public AnalyticsService(IRepositoryManager repositoryManager,IHttpContextAccessor httpContextAccessor,UserManager<AppUser> userManager)
        {
            _repositoryManager = repositoryManager;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }
        public async Task<double?> GetTotalEnergyConsumptionAsync(int deviceId)
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var appUser = await _userManager.FindByIdAsync(userId);


            bool isAdmin = await _userManager.IsInRoleAsync(appUser, "Admin");
            bool isHost = await _userManager.IsInRoleAsync(appUser, "Host");

            var device = await _repositoryManager.DeviceRepository.FindByIdAsync(deviceId);
            if (device == null)
            {
                throw new NotFoundException($"Device with ID {deviceId} not found.");
            }

            IQueryable<SensorData> query;

            if (isAdmin)
            {
                query = _repositoryManager.SensorDataRepository.FindByCondition(x => x.DeviceId == deviceId && x.IsDeleted == null);
            }
            else if (isHost)
            {
                var deviceIds = await _repositoryManager.DeviceUserRepository
                    .FindByCondition(x => x.UserId == userId)
                    .Select(x => x.DeviceId)
                    .ToListAsync();

                if (!deviceIds.Contains(deviceId))
                    throw new ForbiddenException("You do not have permission to access data for this device.");

                query = _repositoryManager.SensorDataRepository.FindByCondition(x => x.DeviceId == deviceId && x.IsDeleted == null);
            }
            else
            {
                throw new ForbiddenException("You do not have permission to view this data.");
            }

            // Enerji istehlakını hesablamaq
            var totalEnergyConsumption = await query.SumAsync(x => x.Value);

            return totalEnergyConsumption;
        }

        public async Task<DeviceStatusGetDto> GetDeviceStatusAsync()
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var appUser = await _userManager.FindByIdAsync(userId);

            bool isAdmin = await _userManager.IsInRoleAsync(appUser, "Admin");
            bool isHost = await _userManager.IsInRoleAsync(appUser, "Host");
            IQueryable<Device> query;

            if (isAdmin)
            {
                query = _repositoryManager.DeviceRepository.FindByCondition(x => x.IsDeleted == null);
            }
            else if (isHost)
            {
                var deviceIds = await _repositoryManager.DeviceUserRepository
                    .FindByCondition(x => x.UserId == userId)
                    .Select(x => x.DeviceId)
                    .ToListAsync();

                query = _repositoryManager.DeviceRepository.FindByCondition(x => deviceIds.Contains(x.Id) && x.IsDeleted == null);
            }
            else
            {
                throw new ForbiddenException("You do not have permission to view this data.");
            }

            var onlineDevicesCount = await query
                .Where(x => x.IsOnline == true)
                .CountAsync();

            var offlineDevicesCount = await query
                .Where(x => x.IsOnline == false)
                .CountAsync();

            var deviceStatus = new DeviceStatusGetDto
            {
                OnlineDevices = onlineDevicesCount,
                OfflineDevices = offlineDevicesCount
            };

            return deviceStatus;
        }

        public async Task<LocationUsageDto> GetLocationUsageAsync()
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var appUser = await _userManager.FindByIdAsync(userId);

            bool isAdmin = await _userManager.IsInRoleAsync(appUser, "Admin");
            bool isHost = await _userManager.IsInRoleAsync(appUser, "Host");

            IQueryable<Device> query;

            if (isAdmin)
            {
                query = _repositoryManager.DeviceRepository.FindByCondition(
                    x => x.IsDeleted == null,
                    includes: new string[] { "SensorDatas", "Location" });
            }
            else if (isHost)
            {
                var userDevicesQuery = _repositoryManager.DeviceUserRepository
                    .FindByCondition(x => x.UserId == userId)
                    .Select(x => x.DeviceId);
                if (!await userDevicesQuery.AnyAsync())
                {
                    throw new NotFoundException("No devices found for the specified user.");
                }

                var deviceIds = await userDevicesQuery.ToListAsync();

                query = _repositoryManager.DeviceRepository
                    .FindByCondition(x => deviceIds.Contains(x.Id) && x.IsDeleted == null, includes: new string[] { "SensorDatas", "Location" });
            }

            else
            {
                throw new ForbiddenException("You do not have permission to view this data.");
            }
            var devices = await query.ToListAsync();
            var locationUsage = devices
                .Where(x => x.Location != null && x.SensorDatas != null && x.SensorDatas.Any()) 
                .GroupBy(x => x.Location.Name ?? "Unknown") 
                .Select(g => new LocationUsageItem
                {
                    Location = g.Key, 
                    TotalDevices = g.Count(), 
                    TotalEnergyConsumption = g.Sum(device => device.SensorDatas?.Sum(sd => sd.Value ?? 0) ?? 0) 
                })
                .ToList();
          
            return new LocationUsageDto
            {
                Locations = locationUsage
            };
        }


    }
}
