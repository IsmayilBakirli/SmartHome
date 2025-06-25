using Microsoft.Extensions.DependencyInjection;
using SmartHome.Application.Repositories.Contract;
using SmartHome.Application.Repositories.Contract.PostgreSqlDb;

namespace SmartHome.Persistence.Repositories
{
    public class RepositoryManager : IRepositoryManager
    {
        private readonly Lazy<ICategoryRepository> _categoryRepository;
        private readonly Lazy<ILocationRepository> _locatinRepository;
        private readonly Lazy<IDeviceRepository> _deviceRepository;
        private readonly Lazy<IDeviceUserRepository> _deviceUserRepository;
        private readonly Lazy<ISensorDataRepository> _sensorDataRepository;

        public RepositoryManager(IServiceProvider serviceProvider)
        {   
            _categoryRepository=new Lazy<ICategoryRepository>(serviceProvider.GetRequiredService<ICategoryRepository>());
            _locatinRepository = new Lazy<ILocationRepository>(serviceProvider.GetRequiredService<ILocationRepository>());
            _deviceRepository = new Lazy<IDeviceRepository>(serviceProvider.GetRequiredService<IDeviceRepository>());
            _deviceUserRepository = new Lazy<IDeviceUserRepository>(serviceProvider.GetRequiredService<IDeviceUserRepository>());
            _sensorDataRepository = new Lazy<ISensorDataRepository>(serviceProvider.GetRequiredService<ISensorDataRepository>());
        }

        public ICategoryRepository CategoryRepository => _categoryRepository.Value;
        public ILocationRepository LocationRepository => _locatinRepository.Value;
        public IDeviceRepository DeviceRepository =>_deviceRepository.Value;
        public IDeviceUserRepository DeviceUserRepository =>_deviceUserRepository.Value;
        public ISensorDataRepository SensorDataRepository =>_sensorDataRepository.Value;
    }
}
