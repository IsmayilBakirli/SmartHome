using SmartHome.Application.Repositories.Contract.PostgreSqlDb;
using SmartHome.Domain.Entities;
using SmartHome.Persistence.Contexts;

namespace SmartHome.Persistence.Repositories.PostgreSqlDb
{
    public class DeviceUserRepository:PostgreSqlDbRepositoryBase<DeviceUser>,IDeviceUserRepository
    {
        public DeviceUserRepository(SmartHomeContext context):base(context) { }

    }
}