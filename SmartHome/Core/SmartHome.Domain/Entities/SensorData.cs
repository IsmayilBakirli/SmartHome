using SmartHome.Domain.Entities.Common;

namespace SmartHome.Domain.Entities
{
    public class SensorData:BaseEntity,IHasCreatedDate
    {
        public double? Value { get; set; }


        public string? ReadingType { get; set; }  // Temperature, Humidity, etc.

        public string? Unit { get; set; }         // °C, %, Watt, etc.

        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        public int DeviceId { get; set; }
        public Device? Device { get; set; }
        public DateTime? CreatedDate { get ; set ; }
    }
}
