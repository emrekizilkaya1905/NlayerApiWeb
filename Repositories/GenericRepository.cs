using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace App.Repositories
{
    public class GenericRepository<T,Tid>(AppDbContext context) : IGenericRepository<T,Tid> where T : BaseEntity<Tid> where Tid : struct
    {
        protected  AppDbContext Context=context;
        private readonly DbSet<T> _dbSet = context.Set<T>();

        public Task<bool>AnyAsync(Tid id) => _dbSet.AnyAsync(x => x.Id.Equals(id));
        public IQueryable<T> GetAll() => _dbSet.AsQueryable().AsNoTracking();
        public IQueryable<T> Where(Expression<Func<T, bool>> predicate) => _dbSet.Where(predicate).AsNoTracking();
        public ValueTask<T?> GetByIdAsync(Tid id) => _dbSet.FindAsync(id);

        public async ValueTask AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }
        public void Update(T entity) => _dbSet.Update(entity);

        public void Delete(T entity) => _dbSet.Remove(entity);

    }
}
