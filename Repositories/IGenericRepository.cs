

using System.Linq.Expressions;

namespace App.Repositories;

public interface IGenericRepository<T,Tid> where T : class where Tid : struct
{
    Task<bool> AnyAsync(Tid id);
    IQueryable<T> GetAll();
    IQueryable<T> Where(Expression<Func<T, bool>> predicate);
    ValueTask<T?> GetByIdAsync(Tid id);
    ValueTask AddAsync(T entity);
    void Update(T entity);
    void Delete(T entity);
}

