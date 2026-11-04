namespace App.Application.Contracts.Persistence
{
    public interface IUnitWork
    {
        Task<int> SaveChangesAsync();
    }
}
