namespace LearningManagementSystem.Domain.Services.CategoryServices;

public interface ICategoryRepository
{
    Result<CategoryResponseModel> CreateCategory1(CategoryViewModels category);

    Task<CategoryViewModels> CreateCategory(CategoryViewModels category);

    Task<List<CategoryViewModels>> GetCategories();

    Task<List<CategoryViewModels>> GetCategory(int id);

    Task<CategoryViewModels> UpdateCategory(int id, CategoryViewModels category);

    Task<CategoryViewModels> PatchCategory(int id, CategoryViewModels category);

    public Task<bool?> DeleteCategory(int id);
}