namespace LearningManagementSystem.Api.Controllers.Endpoints;

//[Authorize ]
[Route("api/[controller]")]
[ApiController]
public class social_linksController : ControllerBase
{
    private readonly ISocial_linksRepository _social_linksRepository;

    public social_linksController(ISocial_linksRepository social_linksRepository)
    {
        _social_linksRepository = social_linksRepository;
    }

    [HttpGet("GetSocial_links")]
    public async Task<IActionResult> GetSocial_links()
    {
        var items = await _social_linksRepository.GetSocial_links();

        return Ok(items);
    }

    [HttpGet("GetSocial_linksById")]
    public async Task<IActionResult> GetSocial_linksById(int id)
    {
        var items = await _social_linksRepository.GetSocial_linksById(id);

        return Ok(items);
    }

    [HttpPost("CreateSocial_links")]
    public async Task<IActionResult> CreateSocial_links(Social_linksViewModels user)
    {
        var items = await _social_linksRepository.CreateSocial_links(user);

        return Ok(items);
    }

    [HttpPut("UpdateSocial_links")]
    public async Task<IActionResult> UpdateSocial_links(int id, Social_linksViewModels user)
    {
        var item = await _social_linksRepository.UpdateSocial_links(id, user);

        if (item is null)
        {
            return BadRequest("Don`t have data");
        }
        return Ok(item);
    }

    [HttpPatch("PatchSocial_links")]
    public async Task<IActionResult> PatchSocial_links(int id, Social_linksViewModels user)
    {
        var item = await _social_linksRepository.PatchSocial_links(id, user);

        if (item is null)
        {
            return BadRequest("Don`t have data");
        }
        return Ok(item);
    }

    [HttpDelete("DeleteSocial_links")]
    public async Task<IActionResult> DeleteSocial_links(int id)
    {
        var item = await _social_linksRepository.DeleteSocial_links(id);

        if (item is null)
        {
            return BadRequest("Don`t have data");
        }
        return Ok("Deleting success");
    }
}
