using SmartHome.Application.Repositories.Contract.PostgreSqlDb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome.Application.Repositories.Contract
{
    public interface IRepositoryManager
    {
        public ICategoryRepository CategoryRepository { get; }
        public ILocationRepository LocationRepository { get; }
        public IDeviceRepository DeviceRepository { get; }
        public IDeviceUserRepository DeviceUserRepository { get; }
        public ISensorDataRepository SensorDataRepository { get; }

    }
}
