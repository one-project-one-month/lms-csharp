namespace LearningManagementSystem.Api.Controllers.Endpoints;


[Authorize(Policy = "RequireWorkerRole")]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class CourseController : ControllerBase
{
    private readonly ICourseService _service;


    public CourseController(ICourseService service)
    {
        _service = service;
    }


    [HttpGet]
    public async Task<IActionResult> GetAllCourses()
    {
        var models = await _service.GetAllCoursesAsync();

        if (models is null)
        {
            return NotFound();
        }

        return Ok(models);
    }


    [HttpGet("{id}")]
    public async Task<IActionResult> GetCourse(int id)
    {
        var model = await _service.GetCourseByIdAsync(id);

        if (model is null)
        {
            return NotFound();
        }

        return Ok(model);
    }


    //[HttpPost]
    //public IActionResult CreateCourse([FromBody] CourseRequestModel request , IFormFile image)
    //{
    //    if(image is not null)
    //    {

    //    }

    //    var model = _service.CreateCourse(request , image);

    //    if (model == null)
    //    {
    //        return NotFound();
    //    }

    //    return Ok(model);
    //}

    [HttpPost]
    public IActionResult CreateCourse([FromBody] CourseRequestModel request)
    {

        var model = _service.CreateCourse(request);

        if (model is null)
        {
            return NotFound();
        }

        return Ok(model);
    }


    [HttpPut("{courseId}")]
    public IActionResult UpdateCourse(int courseId, [FromBody] CourseRequestModel request)
    {
        var model = _service.UpdateCourse(courseId, request);

        if (model is null)
        {
            return NotFound();
        }

        return Ok(model);
    }


    [HttpPatch("{courseId}")]
    public async Task<IActionResult> PatchCourse(int courseId, JsonPatchDocument<CourseRequestModel> patchDoc)
    {
        var updatedCourse = await _service.PatchCourseAsync(courseId, patchDoc);

        if (updatedCourse is null)
        {
            return NotFound();
        }

        return Ok(updatedCourse);
    }


    [HttpDelete("{courseId}")]
    public IActionResult DeleteCourse(int courseId)
    {
        var success = _service.DeleteCourse(courseId);

        if (!success)
        {
            return NotFound();
        }

        return Ok();
    }


}
