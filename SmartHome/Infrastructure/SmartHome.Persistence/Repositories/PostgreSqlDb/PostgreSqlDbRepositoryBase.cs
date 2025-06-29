using Microsoft.EntityFrameworkCore;
using SmartHome.Application.Repositories.Contract.PostgreSqlDb;
using SmartHome.Domain.Entities.Common;
using SmartHome.Persistence.Contexts;
using System.Linq.Expressions;
namespace SmartHome.Persistence.Repositories.PostgreSqlDb
{
    public class PostgreSqlDbRepositoryBase<T> : BaseRepository<T>, IPostgreSqlDbRepositoryBase<T> where T : BaseEntity
    {
        private readonly SmartHomeContext _context;
        public PostgreSqlDbRepositoryBase(SmartHomeContext context)
        {
            _context = context;
        }
        public DbSet<T> Table => _context.Set<T>();
        public override  IQueryable<T> GetAll(int Skip = 0, int Take = int.MaxValue, params string[] includes)
        {
            IQueryable<T> query = Table;

            if (includes.Length > 0)
            {
                foreach (var include in includes)
                {
                    query.Include(include);
                }
            }

            return query.Where(n => n.IsDeleted == null).Skip(Skip).Take(Take);

        }
        public override IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, int Skip = 0, int Take = int.MaxValue, params string[] includes)
        {
            IQueryable<T> query = Table;

            if (includes.Length > 0)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }
            return query.Where(expression).Skip(Skip).Take(Take);
        }

        public async override Task<T> FindByIdAsync(int id, params string[] includes)
        {
            IQueryable<T> query = Table;
            if (includes.Length > 0)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }
            return await query.Where(n => n.Id == id && n.IsDeleted == null).FirstOrDefaultAsync();
        }
        public  override async Task CreateAsync(T entity)
        {
            await Table.AddAsync(entity);
            await _context.SaveChangesAsync();  
            
        }
        public override async Task UpdateAsync(T entity)
        {
            Table.Update(entity);
            await _context.SaveChangesAsync();
        }
        public override async Task DeleteAsync(T entity)
        {
            entity.IsDeleted = DateTime.UtcNow.AddHours(4);
            Table.Update(entity);
            await _context.SaveChangesAsync();
        }

    }
}