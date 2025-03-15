namespace LearningManagementSystem.Api.Controllers.Endpoints;

[Authorize(Roles = "Admins")]
[Route("api/[controller]")]
[ApiController]
public class CategoryController : BaseController
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoryController(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    [HttpGet("GetCategories")]
    public async Task<IActionResult> GetCategories()
    {
        var items = await _categoryRepository.GetCategories();

        return Ok(items);
    }

    [HttpGet("GetCategory")]
    public async Task<IActionResult> GetCategory(int id)
    {
        var items = await _categoryRepository.GetCategory(id);

        return Ok(items);
    }

    [HttpPost("CreateCategory")]
    public async Task<IActionResult> CreateCategory(CategoryViewModels user)
    {
        var items = await _categoryRepository.CreateCategory(user);

        return Ok(items);
    }

    [HttpPut("UpdateCategory")]
    public async Task<IActionResult> UpdateCategory(int id, CategoryViewModels user)
    {

        var item = await _categoryRepository.UpdateCategory(id, user);

        if (item is null)
        {
            return BadRequest("Don`t have data");
        }
        return Ok(item);
    }

    [HttpPatch("PatchCategory")]
    public async Task<IActionResult> PatchCategory(int id, CategoryViewModels user)
    {

        var item = await _categoryRepository.PatchCategory(id, user);

        if (item is null)
        {
            return BadRequest("Don`t have data");
        }
        return Ok(item);
    }

    [HttpDelete("DeleteCategory")]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        var item = await _categoryRepository.DeleteCategory(id);

        if (item is null)
        {
            return BadRequest("Don`t have data");
        }
        return Ok("Deleting success");
    }
}
