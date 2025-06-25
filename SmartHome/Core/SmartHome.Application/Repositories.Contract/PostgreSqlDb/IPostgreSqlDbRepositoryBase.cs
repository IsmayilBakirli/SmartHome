using SmartHome.Domain.Entities.Common;

namespace SmartHome.Application.Repositories.Contract.PostgreSqlDb
{
    public interface IPostgreSqlDbRepositoryBase<T>:IBaseRepository<T> where T : BaseEntity
    {
    }
}
