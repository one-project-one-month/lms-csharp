using LearningManagementSystem.DataBase.Models;
using LearningManagementSystem.Domain.Services.CategoryServices;
using LearningManagementSystem.Domain.ViewModels;
using Microsoft.AspNetCore.Mvc;
//using LearningManagementSystem.DataBase.Migrations;

namespace LearningManagementSystem.Api.Controllers.Endpoints
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : BaseController
    {
        //Test
        private readonly ICategoryRepository _categoryRepository;

        public CategoryController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        [HttpGet("GetCategories")]
        public IActionResult GetCategories()
        {
            var items = _categoryRepository.GetCategories();

            return Ok(items);
        }

        [HttpGet("GetCategory/{id}")]
        public IActionResult GetCategory(int id)
        {
            var items = _categoryRepository.GetCategory(id);

            return Ok(items);
        }

        [HttpPost("CreateCategory")]
        public IActionResult CreateCategory(CategoryViewModels user)
        {
            var items = _categoryRepository.CreateCategory(user);

            return Ok(items);
            //return Execute(items);
        }

        [HttpPut("UpdateCategory/{id}")]
        public IActionResult UpdateCategory(int id, CategoryViewModels user)
        {

            var item = _categoryRepository.UpdateCategory(id, user);

            // need to write reponse & request

            if (item is null)
            {
                return BadRequest("Don`t have data");
            }
            return Ok(item);
        }

        [HttpPatch("PatchCategory/{id}")]
        public IActionResult PatchCategory(int id, CategoryViewModels user)
        {

            var item = _categoryRepository.PatchCategory(id, user);

            // need to write reponse & request

            if (item is null)
            {
                return BadRequest("Don`t have data");
            }
            return Ok(item);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteCategory(int id)
        {
            var item = _categoryRepository.DeleteCategory(id);

            if (item is null)
            {
                return BadRequest("Don`t have data");
            }
            return Ok("Deleting success");
        }


        [HttpPost("Token_Test")]
        public IActionResult Token_Test(TblTokens tokens)
        {
            var items = _categoryRepository.TokensCreate(tokens);

            return Ok(items);
            //return Execute(items);
        }
    }
}
