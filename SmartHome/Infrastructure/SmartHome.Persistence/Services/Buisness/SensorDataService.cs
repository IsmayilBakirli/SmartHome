using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SmartHome.Application.DTOs.Device;
using SmartHome.Application.DTOs.SensorData;
using SmartHome.Application.Exceptions;
using SmartHome.Application.Repositories.Contract;
using SmartHome.Application.Services.Contract;
using SmartHome.Application.Services.Contract.Buisness;
using SmartHome.Domain.Entities;
using SmartHome.Domain.Entities.Identity;

namespace SmartHome.Persistence.Services.Buisness
{
    public class SensorDataService : ISensorDataService
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly IServiceManager _serviceManager;
        public SensorDataService(IRepositoryManager repositoryManager,UserManager<AppUser> userManager,IServiceManager serviceManager)
        {
            _repositoryManager = repositoryManager;
            _userManager = userManager;
            _serviceManager = serviceManager;
        }
        public async Task AddSensorReadingAsync(int deviceId, SensorDataCreateDto dto)
        {
            var appUser = await _serviceManager.CurrentUserService.GetUser();
            var userIdStr = _serviceManager.CurrentUserService.GetUserId();

            if (string.IsNullOrEmpty(userIdStr) || !Guid.TryParse(userIdStr, out var userId))
                throw new UnauthorizedException("User not found or invalid ID.");

            var device = await _repositoryManager.DeviceRepository.FindByIdAsync(deviceId);
            if (device == null)
                throw new NotFoundException("Device not found");

            if (appUser == null)
                throw new NotFoundException("User not found");

            bool isAdmin = await _serviceManager.CurrentUserService.IsInRole("Admin");

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

            var user = await _serviceManager.CurrentUserService.GetUser();
            var userIdStr = _serviceManager.CurrentUserService.GetUserId();

            if (string.IsNullOrEmpty(userIdStr) || !Guid.TryParse(userIdStr, out var userId))
                throw new UnauthorizedException("User not found or invalid ID.");

            var device = await _repositoryManager.DeviceRepository.FindByIdAsync(deviceId);
            if (device == null)
                throw new NotFoundException("Device not found");

            bool isAdmin = await _serviceManager.CurrentUserService.IsInRole("Admin");
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
            var user = await _serviceManager.CurrentUserService.GetUser();
            var userIdStr = _serviceManager.CurrentUserService.GetUserId();

            if (string.IsNullOrEmpty(userIdStr) || !Guid.TryParse(userIdStr, out var userId))
                throw new UnauthorizedException("User not found or invalid ID.");

            var device = await _repositoryManager.DeviceRepository.FindByIdAsync(deviceId);
            if (device == null)
                throw new NotFoundException("Device not found");

            bool isAdmin = await _serviceManager.CurrentUserService.IsInRole("Admin");


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
            var userId = _serviceManager.CurrentUserService.GetUserId();
            bool isAdmin = await _serviceManager.CurrentUserService.IsInRole("Admin");
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
