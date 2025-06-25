using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome.Application.DTOs.Device
{
    public class AssignHostDto
    {
        [Required(ErrorMessage = "DeviceId  is required.")]
        public int DeviceId { get; set; }

        [Required(ErrorMessage = "HostId  is required.")]
        public string HostId { get; set; }
    }
}
