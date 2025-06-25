using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome.Application.DTOs.Location
{
    public class LocationGetDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Zone { get; set; }
        public int Floor { get; set; }
        public string? Description { get; set; }
    }
}
