using SmartHome.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome.Application.Repositories.Contract.PostgreSqlDb
{
    public interface IDeviceRepository:IPostgreSqlDbRepositoryBase<Device>
    {
    }
}
