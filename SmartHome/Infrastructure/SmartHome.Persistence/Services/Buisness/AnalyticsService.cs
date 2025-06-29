using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SmartHome.Application.DTOs.Device;
using SmartHome.Application.DTOs.Location;
using SmartHome.Application.Exceptions;
using SmartHome.Application.Repositories.Contract;
using SmartHome.Application.Services.Contract;
using SmartHome.Application.Services.Contract.Buisness;
using SmartHome.Domain.Entities;
using SmartHome.Domain.Entities.Identity;
using SmartHome.Domain.Enums;
using System.Security.Claims;

namespace SmartHome.Persistence.Services.Buisness
{
    public class AnalyticsService:IAnalyticsService
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IServiceManager _serviceManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly ICurrentUserService _currentUserService;
        

        public AnalyticsService(IRepositoryManager repositoryManager,
                                UserManager<AppUser> userManager,
                                ICurrentUserService currentUserService,
                                IServiceManager serviceManager)
        {
            _repositoryManager = repositoryManager;
            _userManager = userManager;
            _serviceManager = serviceManager;
            _currentUserService = currentUserService;
        }
        public async Task<double?> GetTotalEnergyConsumptionAsync(int deviceId)
        {
            var userId = _currentUserService.GetUserId();
            var role = await GetCurrentUserRoleAsync();

            var device = await _repositoryManager.DeviceRepository.FindByIdAsync(deviceId);
            if (device == null)
                throw new NotFoundException($"Device with ID {deviceId} not found.");

            if (role == Roles.Host)
            {
                bool hasAccess = await HasAccessToDeviceAsync(deviceId, userId);
                if (!hasAccess)
                    throw new ForbiddenException("You do not have permission to access data for this device.");
            }
            else if (role != Roles.Admin)
            {
                throw new ForbiddenException("You do not have permission to view this data.");
            }

            var totalEnergy = await _repositoryManager.SensorDataRepository
                .FindByCondition(x => x.DeviceId == deviceId && x.IsDeleted == null)
                .SumAsync(x => x.Value);

            return totalEnergy;
        }
        private async Task<Roles> GetCurrentUserRoleAsync()
        {
            if (await _currentUserService.IsInRole(Roles.Admin.ToString()))
                return Roles.Admin;

            if (await _currentUserService.IsInRole(Roles.Host.ToString()))
                return Roles.Host;

            return Roles.Member;
        }
        private async Task<bool> HasAccessToDeviceAsync(int deviceId, string userId)
        {
            var deviceIds = await _repositoryManager.DeviceUserRepository
                .FindByCondition(x => x.UserId == userId)
                .Select(x => x.DeviceId)
                .ToListAsync();

            return deviceIds.Contains(deviceId);
        }

        public async Task<DeviceStatusGetDto> GetDeviceStatusAsync()
        {
            var appUser =await _serviceManager.CurrentUserService.GetUser();
            var userId = _serviceManager.CurrentUserService.GetUserId();
            bool isAdmin = await _serviceManager.CurrentUserService.IsInRole("Admin");
            bool isHost = await _serviceManager.CurrentUserService.IsInRole("Host");
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
            var userId =  _serviceManager.CurrentUserService.GetUserId();
            var appUser = await _serviceManager.CurrentUserService.GetUser();
            bool isAdmin = await _serviceManager.CurrentUserService.IsInRole("Admin");
            bool isHost = await _serviceManager.CurrentUserService.IsInRole("Host");

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