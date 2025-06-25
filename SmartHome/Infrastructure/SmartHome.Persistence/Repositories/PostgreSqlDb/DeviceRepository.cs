using SmartHome.Application.Repositories.Contract.PostgreSqlDb;
using SmartHome.Domain.Entities;
using SmartHome.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome.Persistence.Repositories.PostgreSqlDb
{
    public class DeviceRepository:PostgreSqlDbRepositoryBase<Device>,IDeviceRepository
    {
        public DeviceRepository(SmartHomeContext context) : base(context) { }
    }
}
