using LearningManagementSystem.DataBase.Data;
using LearningManagementSystem.DataBase.Models.Category;
using LearningManagementSystem.Domain.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace LearningManagementSystem.Domain.Services.CategoryServices
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext _db;

        public CategoryRepository(AppDbContext db)
        {
            _db = db;
        }

        public CategoryViewModels CreateCategory(CategoryViewModels category)
        {
            category.UpdatedDate = null; // Need to amend and take out

            var model = CategoryMapping(category);

            _db.Category.Add(model);
            _db.SaveChanges();

            return category;
        }

        public List<CategoryViewModels> GetCategories()
        {
            var model = _db.Category
                .AsNoTracking()
                .Where(x => // x.Role == "Instructor" && // need to check with roles
                x.DeleteFlag == false)
                .ToList();

            var userViewModels = model.Select(CategoryViewModelsMapping).ToList();

            return userViewModels;
        }

        public List<CategoryViewModels> GetCategory(string id)
        {
            var model = _db.Category
                .AsNoTracking()
                .Where(x => x.DeleteFlag == false && x.Category_Id.ToString() == id) // need to check with roles
                .ToList();

            var userViewModels = model.Select(CategoryViewModelsMapping).ToList();

            return userViewModels;
        }

        public CategoryViewModels? UpdateCategory(string id, CategoryViewModels category)
        {
            var item = _db.Category
                .AsNoTracking()
                .FirstOrDefault(x => x.Category_Id.ToString() == id
                && x.DeleteFlag == false);
            if (item is null) { return null; }

            item = UpdateCategoryDetails(id, category, item); // Updating Users info

            _db.Entry(item).State = EntityState.Modified;
            _db.SaveChanges();

            var model = CategoryViewModelsMapping(item);

            return model;
        }

        public CategoryViewModels? PatchCategory(string id, CategoryViewModels category)
        {
            var item = _db.Category
                .AsNoTracking()
                .FirstOrDefault(x => x.Category_Id.ToString() == id
                && x.DeleteFlag == false);
            if (item is null) { return null; }

            item = UpdateCategoryDetails(id, category, item); // Updating Users info

            _db.Entry(item).State = EntityState.Modified;
            _db.SaveChanges();

            var model = CategoryViewModelsMapping(item);
            return model;
        }

        public bool? DeleteCategory(string id)
        {
            var item = _db.Category
                .AsNoTracking()
                .FirstOrDefault(x => x.Category_Id.ToString() == id
                && x.DeleteFlag == false);
            if (item is null)
            {
                return null;
            }

            item.DeleteFlag = true;

            _db.Entry(item).State = EntityState.Modified;
            var result = _db.SaveChanges();

            return result > 0;
        }

        // Can use for instructor and students
        private static Category CategoryMapping(CategoryViewModels category)
        {
            return new Category
            {
                Category_Id = Guid.NewGuid(),
                Category_Name = category.Category_Name,
                CreatedDate = category.CreatedDate,
                UpdatedDate = category.UpdatedDate,
                DeleteFlag = false
            };
        }

        private static CategoryViewModels CategoryViewModelsMapping(Category category)
        {
            return new CategoryViewModels
            {
                //Category_Id = category.Category_Id,
                Category_Name = category.Category_Name,
                CreatedDate = category.CreatedDate,
                UpdatedDate = category.UpdatedDate
                //DeleteFlag = false
            };
        }

        private static Category UpdateCategoryDetails(string id, CategoryViewModels category, Category item)
        {

            if (!string.IsNullOrEmpty(id))
            {
                item.Category_Id = Guid.Parse(id); // it`s guid
            }
            if (!string.IsNullOrEmpty(category.Category_Name))
            {
                item.Category_Name = category.Category_Name;
            }
            if (!string.IsNullOrEmpty(category.CreatedDate.ToString()))
            {
                item.CreatedDate = category.CreatedDate;
            }

            item.UpdatedDate = DateTime.Now; // Need to update everytime

            return item;
        }
    }
}
