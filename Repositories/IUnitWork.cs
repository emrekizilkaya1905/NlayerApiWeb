namespace App.Repositories
{
    public interface IUnitWork
    {
        Task<int> SaveChangesAsync();
    }
}
