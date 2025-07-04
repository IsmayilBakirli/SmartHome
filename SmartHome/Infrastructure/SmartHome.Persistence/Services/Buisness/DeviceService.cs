﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SmartHome.Application.DTOs.Device;
using SmartHome.Application.Exceptions;
using SmartHome.Application.MappingProfile;
using SmartHome.Application.Repositories.Contract;
using SmartHome.Application.Services.Contract;
using SmartHome.Application.Services.Contract.Buisness;
using SmartHome.Domain.Entities;
using SmartHome.Domain.Entities.Identity;
using System.Security.Claims;
using System.Text;

namespace SmartHome.Persistence.Services.Buisness
{
    public class DeviceService : IDeviceService
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly IServiceManager _serviceManager;
        public DeviceService(IRepositoryManager repositoryManager, UserManager<AppUser> userManager, IServiceManager serviceManager)
        {
            _repositoryManager = repositoryManager;
            _userManager = userManager;
            _serviceManager = serviceManager;
        }
        public async Task CreateAsync(DeviceCreateDto entity)
        {
            var category = await _repositoryManager.CategoryRepository.FindByIdAsync(entity.CategoryId);
            var errors = new StringBuilder();

            if (category == null)
            {
                errors.Append($"Category with id {entity.CategoryId} not found.");
            }

            var location = await _repositoryManager.LocationRepository.FindByIdAsync(entity.LocationId);

            if (location == null)
            {
                errors.Append($"Location with id {entity.LocationId} not found");
            }

            if (errors.Length > 0)
            {
                throw new NotFoundException(errors.ToString().Trim());
            }
            var device = entity.MapToDevice();
            await _repositoryManager.DeviceRepository.CreateAsync(device);
        }

        public async Task DeleteAsync(int id)
        {
            var user =await _serviceManager.CurrentUserService.GetUser() ;
            var userId = _serviceManager.CurrentUserService.GetUserId();

            if (userId == null)
                throw new UnauthorizedAccessException("User not found.");

            var appUser = await _userManager.FindByIdAsync(userId);
            bool isAdmin = await _serviceManager.CurrentUserService.IsInRole("Admin");
            bool isHost = await _serviceManager.CurrentUserService.IsInRole("Host");

            var device = await _repositoryManager.DeviceRepository.FindByIdAsync(id);
            if (device == null)
                throw new NotFoundException($"Device with ID {id} not found.");

            if (isAdmin)
            {
                // Admin hər cihazı silə bilər
                await _repositoryManager.DeviceRepository.DeleteAsync(device);
            }
            else if (isHost)
            {
                // Host yalnız öz cihazlarını silə bilər
                bool ownsDevice = await _repositoryManager.DeviceUserRepository
                    .FindByCondition(du => du.UserId == userId && du.DeviceId == id)
                    .AnyAsync();

                if (!ownsDevice)
                    throw new ForbiddenException("You can only delete devices assigned to you.");

                await _repositoryManager.DeviceRepository.DeleteAsync(device);
            }
            else
            {
                throw new ForbiddenException("You do not have permission to delete devices.");
            }
        }


        public async Task<DeviceGetDto> GetDeviceDetailsAsync(int deviceId)
        {
            var user = await _serviceManager.CurrentUserService.GetUser();
            var userId = _serviceManager.CurrentUserService.GetUserId();

            bool isAdmin = await _serviceManager.CurrentUserService.IsInRole("Admin");
            bool isHost = await _serviceManager.CurrentUserService.IsInRole("Host");
            bool isMember = await _serviceManager.CurrentUserService.IsInRole("Member");


            Device device = null;

            if (isAdmin)
            {
                device = await _repositoryManager.DeviceRepository.FindByIdAsync(deviceId);
            }
            else if (isHost)
            {
                var assigned = await _repositoryManager.DeviceUserRepository
                    .FindByCondition(du => du.UserId == userId && du.DeviceId == deviceId)
                    .AnyAsync();

                if (!assigned)
                    throw new ForbiddenException("Access denied.");

                device = await _repositoryManager.DeviceRepository.FindByIdAsync(deviceId);
            }
            else if (isMember)
            {
                var assigned = await _repositoryManager.DeviceUserRepository
                    .FindByCondition(du => du.UserId == userId && du.DeviceId == deviceId)
                    .AnyAsync();

                if (!assigned)
                    throw new ForbiddenException("Access denied.");

                device = await _repositoryManager.DeviceRepository.FindByIdAsync(deviceId, includes: new string[] { "category", "location" });
            }
            else
            {
                throw new ForbiddenException("Role not supported.");
            }

            if (device == null)
                throw new NotFoundException("Device not found.");


            return device.MapToDeviceDto();
        }

        public async Task<List<DeviceGetDto>> GetAllAsync()
        {
            var user = await _serviceManager.CurrentUserService.GetUser();
            var userId = _serviceManager.CurrentUserService.GetUserId();
            var roles = _serviceManager.CurrentUserService.GetRoles() ;


            IQueryable<Device> query;
            if (roles.Contains("Admin"))
            {
                query = _repositoryManager.DeviceRepository
                    .GetAll(includes: new string[] { "Category", "Location" });
            }
            else if (roles.Contains("Host"))
            {
                var hasDevice = await _repositoryManager.DeviceUserRepository
                    .FindByCondition(du => du.UserId == userId)
                    .AnyAsync();

                if (!hasDevice)
                {
                   
                    return new List<DeviceGetDto>();
                }

                query = _repositoryManager.DeviceRepository
                    .FindByCondition(d => d.DeviceUsers.Any(du => du.UserId == userId) && d.IsDeleted==null,
                                     includes: new string[] { "Category", "Location" });
            }
            else
            {
                query = _repositoryManager.DeviceRepository
                    .FindByCondition(d => d.DeviceUsers.Any(du => du.UserId == userId) && d.IsDeleted==null,
                                     includes: new string[] { "Category", "Location" });
            }
            var data = await query.MapToDeviceDtos().ToListAsync();

            if (data == null || data.Count == 0)
            {
                throw new NotFoundException("No devices found");
            }

            return data;
        }

        public async Task UpdateAsync(int deviceId, DeviceUpdateDto updateDto)
        {
            var user = await _serviceManager.CurrentUserService.GetUser();
            var userId = _serviceManager.CurrentUserService.GetUserId();

            bool isAdmin = await _serviceManager.CurrentUserService.IsInRole("Admin");
            bool isHost = await _serviceManager.CurrentUserService.IsInRole("Host");

            var device = await _repositoryManager.DeviceRepository.FindByIdAsync(deviceId);
            if (device == null)
                throw new NotFoundException($"Device with ID {deviceId} not found.");

            if (!isAdmin)
            {
                var assigned = await _repositoryManager.DeviceUserRepository
                    .FindByCondition(du => du.UserId == userId && du.DeviceId == deviceId)
                    .AnyAsync();

                if (!assigned)
                    throw new ForbiddenException("You can only update your own devices.");
            }

    
            var categoryExists = await _repositoryManager.CategoryRepository
                .FindByCondition(c => c.Id == updateDto.CategoryId)
                .AnyAsync();

            if (!categoryExists)
                throw new NotFoundException($"Category with ID {updateDto.CategoryId} not found.");

            var locationExists = await _repositoryManager.LocationRepository
                .FindByCondition(l => l.Id == updateDto.LocationId)
                .AnyAsync();

            if (!locationExists)
                throw new NotFoundException($"Location with ID {updateDto.LocationId} not found.");

            updateDto.MapToDevice(device);
          

            await _repositoryManager.DeviceRepository.UpdateAsync(device);
        }

        public async Task<List<DeviceGetDto>> GetDevicesByLocationAsync(int locationId)
        {
            var user = await _serviceManager.CurrentUserService.GetUser();
            var userId = _serviceManager.CurrentUserService.GetUserId();
            var roles = _serviceManager.CurrentUserService.GetRoles();

            bool isAdmin = roles.Contains("Admin");

            IQueryable<Device> query;

            if (isAdmin)
            {
                query = _repositoryManager.DeviceRepository
                    .FindByCondition(d => d.LocationId == locationId,
                                     includes: new[] { "Category", "Location" });
            }
            else
            {
                query = _repositoryManager.DeviceRepository
                    .FindByCondition(d => d.LocationId == locationId &&
                                          d.DeviceUsers.Any(du => du.UserId == userId),
                                     includes: new[] { "Category", "Location" });
            }
            var data = await query.MapToDeviceDtos().ToListAsync();

            if (data == null || !data.Any())
                throw new NotFoundException("No devices found for this location.");

            return data;
        }

    }
}
