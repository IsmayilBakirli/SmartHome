using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SmartHome.Application.DTOs.Device;
using SmartHome.Application.DTOs.SensorData;
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
    public class SensorDataService : ISensorDataService
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public SensorDataService(IRepositoryManager repositoryManager,UserManager<AppUser> userManager,IHttpContextAccessor httpContextAccessor)
        {
            _repositoryManager = repositoryManager;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task AddSensorReadingAsync(int deviceId, SensorDataCreateDto dto)
        {
            var user = _httpContextAccessor.HttpContext.User;
            var userIdStr = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdStr) || !Guid.TryParse(userIdStr, out var userId))
                throw new UnauthorizedException("User not found or invalid ID.");

            var device = await _repositoryManager.DeviceRepository.FindByIdAsync(deviceId);
            if (device == null)
                throw new NotFoundException("Device not found");

            var appUser = await _userManager.FindByIdAsync(userIdStr);
            if (appUser == null)
                throw new NotFoundException("User not found");

            bool isAdmin = await _userManager.IsInRoleAsync(appUser, "Admin");

            if (!isAdmin)
            {
                var hasAccess = await _repositoryManager.DeviceUserRepository
                    .FindByCondition(x => x.DeviceId == deviceId && x.UserId == userIdStr)
                    .AnyAsync();

                if (!hasAccess)
                    throw new ForbiddenException("You don't have permission to add sensor data for this device.");
            }

            var sensorData = new SensorData
            {
                DeviceId = deviceId,
                Value = dto.Value,
                ReadingType = dto.ReadingType,
                Unit = dto.Unit,
                Timestamp = dto.Timestamp ?? DateTime.UtcNow,
                CreatedDate = DateTime.UtcNow
            };

            await _repositoryManager.SensorDataRepository.CreateAsync(sensorData);
        }

        public async Task<List<SensorDataGetDto>> GetRecentSensorReadingsAsync(int deviceId, int page, int pageSize )
        {
          
            var user = _httpContextAccessor.HttpContext.User;
            var userIdStr = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdStr) || !Guid.TryParse(userIdStr, out var userId))
                throw new UnauthorizedException("User not found or invalid ID.");

            var device = await _repositoryManager.DeviceRepository.FindByIdAsync(deviceId);
            if (device == null)
                throw new NotFoundException("Device not found");

            bool isAdmin = await _userManager.IsInRoleAsync(await _userManager.FindByIdAsync(userIdStr), "Admin");

            if (!isAdmin)
            {
                var hasAccess = await _repositoryManager.DeviceUserRepository
                    .FindByCondition(x => x.DeviceId == deviceId && x.UserId == userIdStr)
                    .AnyAsync();

                if (!hasAccess)
                    throw new ForbiddenException("You don't have permission to view readings for this device.");
            }

            var readings = await _repositoryManager.SensorDataRepository
                .FindByCondition(x => x.DeviceId == deviceId)
                .OrderByDescending(x => x.Timestamp) 
                .Skip((page - 1) * pageSize) 
                .Take(pageSize) 
                .Select(x => new SensorDataGetDto
                {
                    Id = x.Id,
                    Value = x.Value,
                    ReadingType = x.ReadingType,
                    Unit = x.Unit,
                    Timestamp = x.Timestamp,
                    DeviceId = x.DeviceId,
                    CreatedDate = x.CreatedDate
                })
                .ToListAsync();

            return readings;
        }
        public async Task<SensorDataGetDto> GetLatestSensorReadingAsync(int deviceId)
        {
            var user = _httpContextAccessor.HttpContext.User;
            var userIdStr = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdStr) || !Guid.TryParse(userIdStr, out var userId))
                throw new UnauthorizedException("User not found or invalid ID.");

            var device = await _repositoryManager.DeviceRepository.FindByIdAsync(deviceId);
            if (device == null)
                throw new NotFoundException("Device not found");

            bool isAdmin = await _userManager.IsInRoleAsync(await _userManager.FindByIdAsync(userIdStr), "Admin");

            if (!isAdmin)
            {
                var hasAccess = await _repositoryManager.DeviceUserRepository
                    .FindByCondition(x => x.DeviceId == deviceId && x.UserId == userIdStr)
                    .AnyAsync();

                if (!hasAccess)
                    throw new ForbiddenException("You don't have permission to view readings for this device.");
            }

            var latestReading = await _repositoryManager.SensorDataRepository
                .FindByCondition(x => x.DeviceId == deviceId)
                .OrderByDescending(x => x.Timestamp)
                .Select(x => new SensorDataGetDto
                {
                    Id = x.Id,
                    Value = x.Value,
                    ReadingType = x.ReadingType,
                    Unit = x.Unit,
                    Timestamp = x.Timestamp,
                    DeviceId = x.DeviceId,
                    CreatedDate = x.CreatedDate
                })
                .FirstOrDefaultAsync();

            if (latestReading == null)
                throw new NotFoundException("No sensor data found for this device.");

            return latestReading;
        }

        public async Task SetDeviceStatusAsync(int deviceId, DeviceStatusDto dto)
        {
            var user = _httpContextAccessor.HttpContext.User;
            var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var appUser = await _userManager.FindByIdAsync(userId);

            if (appUser == null)
                throw new NotFoundException("User not found");

            bool isAdmin = await _userManager.IsInRoleAsync(appUser, "Admin");

            var device = await _repositoryManager.DeviceRepository.FindByIdAsync(deviceId);
            if (device == null)
                throw new NotFoundException($"Device with Id {deviceId} not found.");

            if (!isAdmin)
            {
                var isOwner = await _repositoryManager.DeviceUserRepository
                    .FindByCondition(x => x.DeviceId == deviceId && x.UserId == userId)
                    .AnyAsync();

                if (!isOwner)
                    throw new ForbiddenException("You are not allowed to change status of this device.");
            }
            device.IsOnline = dto.IsOnline;

            await _repositoryManager.DeviceRepository.UpdateAsync(device);
        }
    }
}
