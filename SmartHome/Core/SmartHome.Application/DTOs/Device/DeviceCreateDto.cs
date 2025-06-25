using System;
using System.ComponentModel.DataAnnotations;

namespace SmartHome.Application.DTOs.Device
{
    public class DeviceCreateDto
    {
   
        [Required(ErrorMessage = "Device name is required.")]
        [MaxLength(100, ErrorMessage = "Device name cannot exceed 100 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Power consumption is required.")]
        [Range(0.0, double.MaxValue, ErrorMessage = "Power consumption must be a positive number.")]
        public double PowerConsumptionWatts { get; set; }

        [Required(ErrorMessage = "LocationId is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "LocationId must be a positive integer.")]
        public int LocationId { get; set; }

        [Required(ErrorMessage = "CategoryId is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "CategoryId must be a positive integer.")]
        public int CategoryId { get; set; }

        [Range(0.0, 100.0, ErrorMessage = "CPU usage must be between 0 and 100.")]
        public double? CpuUsage { get; set; }  

        [Range(0.0, 100.0, ErrorMessage = "RAM usage must be between 0 and 100.")]
        public double? RamUsage { get; set; }  

        [Range(-100.0, 100.0, ErrorMessage = "Temperature must be between -100°C and 100°C.")]
        public double? Temperature { get; set; }  

        [Range(0.0, double.MaxValue, ErrorMessage = "Energy consumption must be a positive number.")]
        public double? EnergyConsumption { get; set; }  

        
    }
}
