namespace SmartHome.Application.DTOs.SensorData
{
    public class SensorDataGetDto
    {
        public int Id { get; set; }
        public double? Value { get; set; }
        public string ReadingType { get; set; }
        public string Unit { get; set; }
        public DateTime Timestamp { get; set; }
        public int DeviceId { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
