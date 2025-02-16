using LearningManagementSystem.Domain.ViewModels;

namespace LearningManagementSystem.Domain.Services.CategoryServices
{
    public interface ICategoryRepository
    {
        CategoryViewModels CreateCategory(CategoryViewModels category);

        List<CategoryViewModels> GetCategories();

        List<CategoryViewModels> GetCategory(int id);

        CategoryViewModels UpdateCategory(int id, CategoryViewModels category);

        CategoryViewModels PatchCategory(int id, CategoryViewModels category);

        public bool? DeleteCategory(int id);
    }
}
