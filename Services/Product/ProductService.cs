using App.Repositories.Product;

namespace App.Services.Product
{
    public class ProductService(IProductRepository productRepository) : IProductService
    {
    }
}
