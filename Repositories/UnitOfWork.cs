namespace App.Repositories;

public class UnitOfWork(AppDbContext context) : IUnitWork
{
    public Task<int> SaveChangesAsync() => context.SaveChangesAsync();

}

