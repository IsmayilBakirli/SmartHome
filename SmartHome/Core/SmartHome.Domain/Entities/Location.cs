using SmartHome.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome.Domain.Entities
{
    public class Location:BaseEntity,IHasCreatedDate,IHasUpdatedDate
    {
        public string? Name { get; set; }      // Məsələn: "Living Room", "Bedroom", "Kitchen"

        public int Floor { get; set; }        // Məsələn: 0 - Zirzəmi, 1 - Birinci mərtəbə

        public string? Zone { get; set; }      // (Opsional) Məsələn: "North Wing", "Left side", "Garden area"

        public string? Description { get; set; }  // (Opsional) Əlavə izah: "Main living area with thermostat and lights"

        public ICollection<Device>? Devices { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get ; set ; }
    }
}
