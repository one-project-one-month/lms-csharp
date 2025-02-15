using LearningManagementSystem.Domain.ViewModels;

namespace LearningManagementSystem.Domain.Services.CategoryServices
{
    public interface ICategoryRepository
    {
        CategoryViewModels CreateCategory(CategoryViewModels category);

        List<CategoryViewModels> GetCategories();

        List<CategoryViewModels> GetCategory(string id);

        CategoryViewModels UpdateCategory(string id, CategoryViewModels category);

        CategoryViewModels PatchCategory(string id, CategoryViewModels category);

        public bool? DeleteCategory(string id);
    }
}
