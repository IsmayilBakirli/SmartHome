using SmartHome.Domain.Entities.Common;
using System.Linq.Expressions;

namespace SmartHome.Application.Repositories.Contract
{
    public interface IBaseRepository<T> where T : BaseEntity
    {
        IQueryable<T> GetAll(int Skip=0,int Take = int.MaxValue, params string[] includes);

        IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, int Skip = 0, int Take = int.MaxValue, params string[] includes);

        Task<T> FindByIdAsync(int id,params string[] includes);

        Task CreateAsync(T entity);

        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);

        
    }
}
