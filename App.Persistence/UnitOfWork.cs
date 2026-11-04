using App.Application.Contracts.Persistence;

namespace App.Persistence;

public class UnitOfWork(AppDbContext context) : IUnitWork
{
    public Task<int> SaveChangesAsync() => context.SaveChangesAsync();

}

