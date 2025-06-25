using SmartHome.Domain.Entities.Common;
using SmartHome.Domain.Entities.Identity;

namespace SmartHome.Domain.Entities
{
    public class Device : BaseEntity, IHasCreatedDate, IHasUpdatedDate
    {

        public string? Name { get; set; }
        public bool IsOnline { get; set; }
        public double PowerConsumptionWatts { get; set; }
        public int LocationId { get; set; }
        public Location? Location { get; set; }
        public int CategoryId { get; set; }
        public Category? Category { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public ICollection<DeviceUser>? DeviceUsers { get; set; }
        public ICollection<SensorData>? SensorDatas { get; set; }
        public double? CpuUsage { get; set; }
        public double? RamUsage { get; set; }
        public double? Temperature { get; set; }
        public double? EnergyConsumption { get; set; }
        public string? HealthStatus { get; set; } // "Normal", "Warning", "Critical"

    }
}
