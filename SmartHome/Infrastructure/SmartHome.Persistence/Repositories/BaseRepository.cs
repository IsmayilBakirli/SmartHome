using SmartHome.Application.Repositories.Contract;
using SmartHome.Domain.Entities.Common;
using System.Linq.Expressions;

namespace SmartHome.Persistence.Repositories
{
    public abstract class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
    {
        public abstract Task CreateAsync(T entity);

        public abstract Task UpdateAsync(T entity);
        public abstract Task DeleteAsync(T entity);
        public abstract IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, int Skip = 0, int Take = int.MaxValue, params string[] includes);

        public abstract Task<T> FindByIdAsync(int id, params string[] includes);

        public abstract IQueryable<T> GetAll(int Skip = 0, int Take = int.MaxValue, params string[] includes);

    }
}