using SmartHome.Application.Repositories.Contract.PostgreSqlDb;
using SmartHome.Domain.Entities;
using SmartHome.Persistence.Contexts;

namespace SmartHome.Persistence.Repositories.PostgreSqlDb
{
    public class SensorDataRepository:PostgreSqlDbRepositoryBase<SensorData>,ISensorDataRepository
    {
        public SensorDataRepository(SmartHomeContext smartHomeContext) : base(smartHomeContext) { }
    }
}
