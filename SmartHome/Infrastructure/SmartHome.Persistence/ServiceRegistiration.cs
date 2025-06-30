using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SmartHome.Application.Repositories.Contract;
using SmartHome.Application.Repositories.Contract.PostgreSqlDb;
using SmartHome.Application.Services.Contract;
using SmartHome.Application.Services.Contract.Buisness;
using SmartHome.Persistence.Contexts;
using SmartHome.Persistence.Repositories;
using SmartHome.Persistence.Repositories.PostgreSqlDb;
using SmartHome.Persistence.Services;
using SmartHome.Persistence.Services.Buisness;

namespace SmartHome.Persistence
{
    public static class ServiceRegistiration
    {
        public static IServiceCollection AddPersistenceServices(this IServiceCollection serviceCollection,IConfiguration configuration)
        {
            serviceCollection.AddScoped<IRepositoryManager, RepositoryManager>();
            serviceCollection.AddScoped<IServiceManager, ServiceManager>();

            serviceCollection.AddScoped<ICategoryRepository, CategoryRepository>();
            serviceCollection.AddScoped<ICategoryService, CategoryService>();


            serviceCollection.AddScoped<ILocationRepository, LocationRepository>();
            serviceCollection.AddScoped<ILocationService, LocationService>();
            
            serviceCollection.AddScoped<IDeviceRepository, DeviceRepository>();
            serviceCollection.AddScoped<IDeviceService, DeviceService>();

            serviceCollection.AddScoped<IDeviceUserRepository, DeviceUserRepository>();
            serviceCollection.AddScoped<IDeviceUserService, DeviceUserService>();

            serviceCollection.AddScoped<ISensorDataRepository, SensorDataRepository>();
            serviceCollection.AddScoped<ISensorDataService, SensorDataService>();

            serviceCollection.AddScoped<IAnalyticsService, AnalyticsService>();

            serviceCollection.AddScoped<IDeviceHealthService, DeviceHealthService>();
            
            serviceCollection.AddScoped<IUserService, UserService>();

            serviceCollection.AddScoped<ICurrentUserService, CurrentUserService>();

            serviceCollection.AddScoped<IJwtService, JwtService>();

            serviceCollection.AddHttpContextAccessor();
            serviceCollection.AddDbContext<SmartHomeContext>(options =>
            {
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"),
                    d => d.MigrationsAssembly("SmartHome.Persistence"));
            });
            return serviceCollection;
        }
    }
}
