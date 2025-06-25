using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome.Application.DTOs.Device
{
    public class DeviceStatusGetDto
    {
        public int OnlineDevices { get; set; }
        public int OfflineDevices { get; set; }
    }
}
