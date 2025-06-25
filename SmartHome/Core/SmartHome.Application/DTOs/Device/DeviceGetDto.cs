using System;

namespace SmartHome.Application.DTOs.Device
{
    public class DeviceGetDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public bool IsOnline { get; set; }

        public double PowerConsumptionWatts { get; set; }

        public int LocationId { get; set; }

        public string? LocationName { get; set; }  

        public int CategoryId { get; set; }

        public string? CategoryName { get; set; }  

        public DateTime? CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public double? CpuUsage { get; set; } 
        public double? RamUsage { get; set; }  

        public double? Temperature { get; set; }  

        public double? EnergyConsumption { get; set; } 

    }
}
