
namespace App.Repositories.Product
{
    public interface IProductRepository:IGenericRepository<Product>
    {
        public Task <List<Product>> GetTopPriceProductsAsync(int count);
    }
}
    