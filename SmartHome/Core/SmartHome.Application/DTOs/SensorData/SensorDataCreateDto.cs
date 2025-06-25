using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome.Application.DTOs.SensorData
{
    public class SensorDataCreateDto
    {
        [Required(ErrorMessage = "The Value field is required.")]
        public double? Value { get; set; }

        [Required(ErrorMessage = "The ReadingType field is required.")]
        public string ReadingType { get; set; }  // Example: Temperature, Humidity

        [Required(ErrorMessage = "The Unit field is required.")]
        public string Unit { get; set; }         // Example: °C, %, Watt

        public DateTime? Timestamp { get; set; } // Optional. If not provided, server will assign UTC time.
    }

}
