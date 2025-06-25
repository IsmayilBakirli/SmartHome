using SmartHome.Application.Services.Contract.Buisness;

namespace SmartHome.Application.Services.Contract
{
    public interface IServiceManager
    {
        public ICategoryService CategoryService { get; }
        public ILocationService LocationService { get; }
        public IDeviceService DeviceService { get; }
        public IDeviceUserService DeviceUserService { get; }
        public ISensorDataService SensorDataService { get; }
        public IAnalyticsService AnalyticsService { get; }
        public IDeviceHealthService DeviceHealthService { get; }
    }
}
