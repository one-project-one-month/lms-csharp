using LearningManagementSystem.DataBase.Data;
using LearningManagementSystem.DataBase.Models;
using LearningManagementSystem.Domain.Models;
using LearningManagementSystem.Domain.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
//using LearningManagementSystem.DataBase.Migrations;

namespace LearningManagementSystem.Domain.Services.CategoryServices
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext _db;

        public CategoryRepository(AppDbContext db)
        {
            _db = db;
        }

        public Result<CategoryResponseModel> CreateCategory1(CategoryViewModels category)
        {
            try
            {
                category.updated_at = null; // Need to amend and take out

                var model = CategoryMapping(category);

                _db.Category.Add(model);
                _db.SaveChanges();

                var item = new CategoryResponseModel()
                {
                    Category = category
                };

                var result = Result<CategoryResponseModel>
                    .Success(item, "Category created successfully.");

                return result;
            }
            catch (ValidationException ex)
            {
                return Result<CategoryResponseModel>.ValidationError("Validation Error: " + ex.Message);
            }
            catch (SystemException ex)
            {
                return Result<CategoryResponseModel>.SystemError("System Error: " + ex.Message);
            }
            catch (Exception ex)
            {
                return Result<CategoryResponseModel>.Error("An unexpected error occurred: " + ex.Message);
            }
        }

        public CategoryViewModels CreateCategory(CategoryViewModels category)
        {
            try
            {
                category.updated_at = null; // Need to amend and take out

                var model = CategoryMapping(category);

                _db.Category.Add(model);
                _db.SaveChanges();

                return category;
            }
            catch 
            {
                throw;
            }
        }

        public List<CategoryViewModels> GetCategories()
        {
            var model = _db.Category
                .AsNoTracking()
                .Where(x => // x.Role == "Instructor" && // need to check with roles
                x.isDeleted == false)
                .ToList();

            var userViewModels = model.Select(CategoryViewModelsMapping).ToList();

            return userViewModels;
        }

        public List<CategoryViewModels> GetCategory(int id)
        {
            var model = _db.Category
                .AsNoTracking()
                .Where(x => x.isDeleted == false && x.id == id) // need to check with roles
                .ToList();

            var userViewModels = model.Select(CategoryViewModelsMapping).ToList();

            return userViewModels;
        }

        public CategoryViewModels? UpdateCategory(int id, CategoryViewModels category)
        {
            var item = _db.Category
                .AsNoTracking()
                .FirstOrDefault(x => x.id == id
                && x.isDeleted == false);
            if (item is null) { return null; }

            item = UpdateCategoryDetails(id, category, item); // Updating Users info

            _db.Entry(item).State = EntityState.Modified;
            _db.SaveChanges();

            var model = CategoryViewModelsMapping(item);

            return model;
        }

        public CategoryViewModels? PatchCategory(int id, CategoryViewModels category)
        {
            var item = _db.Category
                .AsNoTracking()
                .FirstOrDefault(x => x.id == id
                && x.isDeleted == false);
            if (item is null) { return null; }

            item = UpdateCategoryDetails(id, category, item); // Updating Users info

            _db.Entry(item).State = EntityState.Modified;
            _db.SaveChanges();

            var model = CategoryViewModelsMapping(item);
            return model;
        }

        public bool? DeleteCategory(int id)
        {
            var item = _db.Category
                .AsNoTracking()
                .FirstOrDefault(x => x.id == id
                && x.isDeleted == false);
            if (item is null)
            {
                return null;
            }

            item.isDeleted = true;

            _db.Entry(item).State = EntityState.Modified;
            var result = _db.SaveChanges();

            return result > 0;
        }

        // Can use for instructor and students
        private static TblCategory CategoryMapping(CategoryViewModels category)
        {
            return new TblCategory
            {
                //id = Guid.NewGuid(),
                id = 0,
                name = category.name,
                created_at = category.created_at,
                updated_at = category.updated_at,
                isDeleted = false
            };
        }

        private static CategoryViewModels CategoryViewModelsMapping(TblCategory category)
        {
            return new CategoryViewModels
            {
                //id = category.id,
                name = category.name,
                created_at = category.created_at,
                updated_at = category.updated_at
                //isDeleted = false
            };
        }

        private static TblCategory UpdateCategoryDetails(int id, CategoryViewModels category, TblCategory item)
        {

            if (!string.IsNullOrEmpty(id.ToString()))
            {
                //item.Category_Id = Guid.Parse(id);
                item.id = id;
            }
            if (!string.IsNullOrEmpty(category.name))
            {
                item.name = category.name;
            }
            if (!string.IsNullOrEmpty(category.created_at.ToString()))
            {
                item.created_at = category.created_at;
            }

            item.updated_at = DateTime.Now; // Need to update everytime

            return item;
        }

        public TblTokens TokensCreate(TblTokens tokens)
        {
            try
            {
                tokens.updated_at = null; // Need to amend and take out

                tokens.user_id = 1;
                tokens.created_at = DateTime.UtcNow;
                //tokens.TblUser = null;

                //tokens.id = 1;

                _db.Tokens.Add(tokens);
                _db.SaveChanges();

                return tokens;
            }
            catch
            {
                throw;
            }
        }
    }
}
