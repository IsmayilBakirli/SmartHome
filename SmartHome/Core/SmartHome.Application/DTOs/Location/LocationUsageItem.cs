using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome.Application.DTOs.Location
{
    public class LocationUsageItem
    {
        public string Location { get; set; }  
        public int TotalDevices { get; set; }  
        public double TotalEnergyConsumption { get; set; } 
    }

}
