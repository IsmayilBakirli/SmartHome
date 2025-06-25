using SmartHome.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome.Domain.Entities
{
    public class Category:BaseEntity,IHasCreatedDate,IHasUpdatedDate
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public ICollection<Device>? Devices { get; set; }
        public DateTime? CreatedDate { get ; set; }
        public DateTime? UpdatedDate { get ; set; }
    }
}
