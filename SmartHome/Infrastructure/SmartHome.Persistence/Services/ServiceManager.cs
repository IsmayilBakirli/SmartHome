using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.Extensions.DependencyInjection;
using SmartHome.Application.Services.Contract;
using SmartHome.Application.Services.Contract.Buisness;

namespace SmartHome.Persistence.Services
{
    public class ServiceManager : IServiceManager
    {
        private readonly Lazy<ICategoryService> _categoryService;
        private readonly Lazy<ILocationService> _locationService;
        private readonly Lazy<IDeviceService> _deviceService;
        private readonly Lazy<IDeviceUserService> _deviceUserService;
        private readonly Lazy<ISensorDataService> _sensorDataService;
        private readonly Lazy<IAnalyticsService> _analyticsService;
        private readonly Lazy<IDeviceHealthService> _deviceHealthService;
        private readonly Lazy<IUserService> _userService;
        private readonly Lazy<ICurrentUserService> _currentUserService;
        private readonly Lazy<IJwtService> _jwtService;
        public ServiceManager(IServiceProvider serviceProvider)
        {
            _categoryService = new Lazy<ICategoryService>(()=>serviceProvider.GetRequiredService<ICategoryService>());
            _locationService = new Lazy<ILocationService>(() => serviceProvider.GetRequiredService<ILocationService>());
            _deviceService = new Lazy<IDeviceService>(() => serviceProvider.GetRequiredService<IDeviceService>());
            _deviceUserService = new Lazy<IDeviceUserService>(() => serviceProvider.GetRequiredService<IDeviceUserService>());
            _sensorDataService = new Lazy<ISensorDataService>(() => serviceProvider.GetRequiredService<ISensorDataService>());
            _analyticsService = new Lazy<IAnalyticsService>(() => serviceProvider.GetRequiredService<IAnalyticsService>());
            _deviceHealthService = new Lazy<IDeviceHealthService>(() => serviceProvider.GetRequiredService<IDeviceHealthService>());
            _userService = new Lazy<IUserService>(() => serviceProvider.GetRequiredService<IUserService>());
            _currentUserService = new Lazy<ICurrentUserService>(() => serviceProvider.GetRequiredService<ICurrentUserService>());
            _jwtService = new Lazy<IJwtService>(() => serviceProvider.GetRequiredService<IJwtService>());


        }
        public ICategoryService CategoryService => _categoryService.Value;
        public ILocationService LocationService => _locationService.Value;
        public IDeviceService DeviceService => _deviceService.Value;
        public IDeviceUserService DeviceUserService => _deviceUserService.Value;
        public ISensorDataService SensorDataService => _sensorDataService.Value;
        public IAnalyticsService AnalyticsService => _analyticsService.Value;
        public IDeviceHealthService DeviceHealthService => _deviceHealthService.Value;
        public IUserService UserService => _userService.Value;
        public ICurrentUserService CurrentUserService => _currentUserService.Value;
        public IJwtService JwtService => _jwtService.Value; 

    }
}