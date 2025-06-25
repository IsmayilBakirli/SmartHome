using SmartHome.Application.Repositories.Contract.PostgreSqlDb;
using SmartHome.Domain.Entities;
using SmartHome.Persistence.Contexts;

namespace SmartHome.Persistence.Repositories.PostgreSqlDb
{
    public class CategoryRepository:PostgreSqlDbRepositoryBase<Category>,ICategoryRepository
    {
        public CategoryRepository(SmartHomeContext context):base(context) { }
    }
}
