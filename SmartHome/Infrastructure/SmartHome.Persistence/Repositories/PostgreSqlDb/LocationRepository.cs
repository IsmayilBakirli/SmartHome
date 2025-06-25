using SmartHome.Application.Repositories.Contract.PostgreSqlDb;
using SmartHome.Domain.Entities;
using SmartHome.Persistence.Contexts;

namespace SmartHome.Persistence.Repositories.PostgreSqlDb
{
    public class LocationRepository : PostgreSqlDbRepositoryBase<Location>, ILocationRepository
    {
        public LocationRepository(SmartHomeContext context) : base(context) { }
    }
}
