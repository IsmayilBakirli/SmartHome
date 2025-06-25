using SmartHome.Domain.Entities.Common;
using SmartHome.Domain.Entities.Identity;

namespace SmartHome.Domain.Entities
{
    public class DeviceUser:BaseEntity
    {
        public string? UserId { get; set; }
        public AppUser? User { get; set; }

        public int DeviceId { get; set; }
        public Device? Device { get; set; }


    }
}
