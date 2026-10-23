using App.Repositories;
using App.Repositories.Categories;
using App.Services.Categories.Create;
using App.Services.Categories.Dto;
using App.Services.Categories.Update;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace App.Services.Categories
{
    public class CategoryService(ICategoryRepository categoryRepository, IUnitWork unitOfWork, IMapper mapper) : ICategoryService
    {
        public async Task<ServiceResult<List<CategoryDto>>> GetAllListAsync()
        {
            var categories = await categoryRepository.GetAll().ToListAsync();
            var categoryAsDto = mapper.Map<List<CategoryDto>>(categories);
            return ServiceResult<List<CategoryDto>>.Success(categoryAsDto);
        }
        // Get category by id
        public async Task<ServiceResult<CategoryDto?>> GetByIdAsync(int categoryId)
        {
            var category = await categoryRepository.GetByIdAsync(categoryId);
            if (category is null)
            {
                return ServiceResult<CategoryDto?>.Fail($"Category with id {categoryId} not found", HttpStatusCode.NotFound);
            }
            var categoryAsDto = mapper.Map<CategoryDto>(category);
            return ServiceResult<CategoryDto>.Success(categoryAsDto)!;
        }

        // Category with products
        public async Task<ServiceResult<CategoryWithProductsDto?>> GetCategoryWithProductsAsync(int categoryId)
        {
            var category = await categoryRepository.GetCategoryWithProductsAsync(categoryId);
            if (category is null)
            {
                return ServiceResult<CategoryWithProductsDto?>.Fail($"Category with id {categoryId} not found", HttpStatusCode.NotFound);
            }
            var categoryAsDto = mapper.Map<CategoryWithProductsDto>(category);
            return ServiceResult<CategoryWithProductsDto>.Success(categoryAsDto)!;
        }
        // Overload GetCategoryWithProductsAsync

        public async Task<ServiceResult<List<CategoryWithProductsDto?>>> GetCategoryWithProductsAsync()
        {
            var category = await categoryRepository.GetCategoryWithProducts().ToListAsync();

            var categoryAsDto = mapper.Map<List<CategoryWithProductsDto?>>(category);
            return ServiceResult<List<CategoryWithProductsDto?>>.Success(categoryAsDto)!;
        }
        public async Task<ServiceResult<int>> CreateAsync(CreateCategoryRequest request)
        {
            var anyCategory = await categoryRepository.Where(x => x.Name == request.Name).AnyAsync();
            if (anyCategory)
            {
                return ServiceResult<int>.Fail("Category already exists");
            }
            var newCategory = mapper.Map<Category>(request);
            await categoryRepository.AddAsync(newCategory);
            await unitOfWork.SaveChangesAsync();
            return ServiceResult<int>.SuccessAsCreated(newCategory.Id, $"api/categories/{newCategory.Id}");
        }

        public async Task<ServiceResult> UpdateAsync(int categoryId, UpdateCategoryRequest request)
        {
            var category = await categoryRepository.GetByIdAsync(categoryId);
            if (category == null)
            {
                return ServiceResult.Fail("Category not found");
            }
            var isCategoryNameExists = await categoryRepository.Where(x => x.Name == request.Name && x.Id != category.Id).AnyAsync();
            if (isCategoryNameExists)
            {
                return ServiceResult.Fail("Category name already exists");
            }
            mapper.Map(request, category);
            await unitOfWork.SaveChangesAsync();
            return ServiceResult.Success(HttpStatusCode.NoContent);
        }
        // Delete method
        public async Task<ServiceResult> DeleteAsync(int categoryId)
        {
            var category = await categoryRepository.GetByIdAsync(categoryId);
            if (category == null)
            {
                return ServiceResult.Fail("Category not found", HttpStatusCode.NotFound);
            }
            categoryRepository.Delete(category);
            await unitOfWork.SaveChangesAsync();
            return ServiceResult.Success(HttpStatusCode.NoContent);
        }
    }
}
