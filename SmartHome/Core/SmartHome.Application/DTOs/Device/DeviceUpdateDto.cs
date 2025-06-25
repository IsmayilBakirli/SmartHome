using System;
using System.ComponentModel.DataAnnotations;

namespace SmartHome.Application.DTOs.Device
{
    public class DeviceUpdateDto
    {
        public string Name { get; set; }
        public double PowerConsumptionWatts { get; set; }
        public int LocationId { get; set; }
        public int CategoryId { get; set; }
        public double? CpuUsage { get; set; }
        public double? RamUsage { get; set; }
        public double? Temperature { get; set; }
        public double? EnergyConsumption { get; set; }

 
    }
}
