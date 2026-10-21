using App.Repositories;
using App.Repositories.Products;
using App.Services.Products.Create;
using App.Services.Products.Update;
using App.Services.Products.UpdateStock;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace App.Services.Products
{
    public class ProductService(IProductRepository productRepository, IUnitWork unitOfWork, IMapper mapper) : IProductService
    {
        public async Task<ServiceResult<List<ProductDto>>> GetAllListAsync()
        {
            var products = await productRepository.GetAll().ToListAsync();
            var productAsDto=mapper.Map<List<ProductDto>>(products);

            return ServiceResult<List<ProductDto>>.Success(productAsDto);
        }

        public async Task<ServiceResult<List<ProductDto>>> GetPagedAllListAsync(int pageNumber, int pageSize)
        {
            var products = await productRepository.GetAll()
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize).ToListAsync();
            var productAsDto = mapper.Map<List<ProductDto>>(products);
            return ServiceResult<List<ProductDto>>.Success(productAsDto);
        }
        public async Task<ServiceResult<List<ProductDto>>> GetTopPriceProductsAsync(int count)
        {
            var products = await productRepository.GetTopPriceProductsAsync(count);
            var productAsDto=mapper.Map<List<ProductDto>>(products);
            return new ServiceResult<List<ProductDto>>
            {
                Data = productAsDto
            };
        }
        public async Task<ServiceResult<ProductDto?>> GetByIdAsync(int id)
        {
            var product = await productRepository.GetByIdAsync(id);
            if (product is null)
            {
                return ServiceResult<ProductDto?>.Fail($"Product with id {id} not found", HttpStatusCode.NotFound);
            }
            var productAsDto = mapper.Map<ProductDto>(product);
            return ServiceResult<ProductDto>.Success(productAsDto)!;
        }
        public async Task<ServiceResult<CreateProductResponse>> CreateAsync(CreateProductRequest request)
        {
            var anyProduct= await productRepository.Where(p => p.Name == request.Name).AnyAsync();
            if(anyProduct)
            {
                return ServiceResult<CreateProductResponse>.Fail($"Product with name {request.Name} already exists", HttpStatusCode.BadRequest);
            }
            var product = mapper.Map<Product>(request);
            await productRepository.AddAsync(product);
            await unitOfWork.SaveChangesAsync();
            return ServiceResult<CreateProductResponse>.SuccessAsCreated(new CreateProductResponse(product.Id), $"api/products/{product.Id}");
        }
        public async Task<ServiceResult> UpdateAsync(int id, UpdateProductRequest request)
        {
            var product = await productRepository.GetByIdAsync(id);
            if (product is null)
            {
                return ServiceResult.Fail($"Product with id {id} not found", HttpStatusCode.NotFound);
            }
            var isProductNameExist = await productRepository.Where(p => p.Name == request.Name && p.Id != id).AnyAsync();
            if (isProductNameExist)
            {
                return ServiceResult.Fail($"Product with name {request.Name} already exists", HttpStatusCode.BadRequest);
            }
            product = mapper.Map(request, product);
            await unitOfWork.SaveChangesAsync();
            return ServiceResult.Success(HttpStatusCode.NoContent);
        }
     
        public async Task<ServiceResult> UpdateStockAsync(UpdateProductStockRequest request)
        {
            var product = await productRepository.GetByIdAsync(request.ProductId);
            if (product is null)
            {
                return ServiceResult.Fail($"Product with id {request.ProductId} not found", HttpStatusCode.NotFound);
            }
            product!.Stock = request.Quantity;
            await unitOfWork.SaveChangesAsync();
            return ServiceResult.Success(HttpStatusCode.NoContent);
        }
        public async Task<ServiceResult> DeleteAsync(int id)
        {
            var product = await productRepository.GetByIdAsync(id);
            if (product is null)
            {
                return ServiceResult.Fail($"Product with id {id} not found", HttpStatusCode.NotFound);
            }
            productRepository.Delete(product);
            await unitOfWork.SaveChangesAsync();
            return ServiceResult.Success(HttpStatusCode.NoContent);
        }
    }
}
