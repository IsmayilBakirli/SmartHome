using SmartHome.Application.DTOs.Device;
using SmartHome.Domain.Entities;
using System;
using System.Linq;

namespace SmartHome.Application.MappingProfile
{
    public static class DeviceMapping
    {
        public static IQueryable<DeviceGetDto> MapToDeviceDtos(this IQueryable<Device> devices)
        {
            return devices.Select(device => new DeviceGetDto
            {
                Id = device.Id,
                Name = device.Name,
                IsOnline = device.IsOnline,
                PowerConsumptionWatts = device.PowerConsumptionWatts,
                LocationId = device.LocationId,
                LocationName = device.Location != null ? device.Location.Name : null,
                CategoryId = device.CategoryId,
                CategoryName = device.Category != null ? device.Category.Name : null,
                CreatedDate = device.CreatedDate,
                UpdatedDate = device.UpdatedDate,

                CpuUsage = device.CpuUsage,
                RamUsage = device.RamUsage,
                Temperature = device.Temperature,
                EnergyConsumption = device.EnergyConsumption
            });
        }

        public static Device MapToDevice(this DeviceCreateDto deviceCreateDto)
        {
            return new Device
            {
                Name = deviceCreateDto.Name,
                PowerConsumptionWatts = deviceCreateDto.PowerConsumptionWatts,
                LocationId = deviceCreateDto.LocationId,
                CategoryId = deviceCreateDto.CategoryId,

                CpuUsage = deviceCreateDto.CpuUsage,
                RamUsage = deviceCreateDto.RamUsage,
                Temperature = deviceCreateDto.Temperature,
                EnergyConsumption = deviceCreateDto.EnergyConsumption
            };
        }

        public static DeviceGetDto MapToDeviceDto(this Device device)
        {
            return new DeviceGetDto
            {
                Id = device.Id,
                Name = device.Name,
                IsOnline = device.IsOnline,
                PowerConsumptionWatts = device.PowerConsumptionWatts,
                LocationId = device.LocationId,
                LocationName = device.Location != null ? device.Location.Name : null,
                CategoryId = device.CategoryId,
                CategoryName = device.Category != null ? device.Category.Name : null,
                CreatedDate = device.CreatedDate,
                UpdatedDate = device.UpdatedDate,

                // Sağlamlıqla əlaqəli yeni sahələr
                CpuUsage = device.CpuUsage,
                RamUsage = device.RamUsage,
                Temperature = device.Temperature,
                EnergyConsumption = device.EnergyConsumption,
            };
        }
        public static Device MapToDevice(this DeviceUpdateDto deviceUpdateDto, Device existingDevice)
        {
            existingDevice.Name = deviceUpdateDto.Name;
            existingDevice.PowerConsumptionWatts = deviceUpdateDto.PowerConsumptionWatts;
            existingDevice.LocationId = deviceUpdateDto.LocationId;
            existingDevice.CategoryId = deviceUpdateDto.CategoryId;

            existingDevice.CpuUsage = deviceUpdateDto.CpuUsage;
            existingDevice.RamUsage = deviceUpdateDto.RamUsage;
            existingDevice.Temperature = deviceUpdateDto.Temperature;
            existingDevice.EnergyConsumption = deviceUpdateDto.EnergyConsumption;

            return existingDevice;
        }
    }
}
