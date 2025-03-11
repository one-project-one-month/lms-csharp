namespace LearningManagementSystem.Api.Controllers.Endpoints;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class LessonController : ControllerBase
{
    private readonly ILessonRepository _lessonRepository;

    public LessonController(ILessonRepository lessonRepository)
    {
        _lessonRepository = lessonRepository;
    }

    [HttpGet("GetLessons")]
    public IActionResult GetLessons()
    {
        var lessons = _lessonRepository.GetLessons();
        return Ok(lessons);
    }

    [HttpGet("GetLesson/{id}")]
    public IActionResult GetLessonById(int id)
    {
        var lesson = _lessonRepository.GetLessonById(id);
        if (lesson == null)
        {
            return NotFound("Lesson not found.");
        }
        return Ok(lesson);
    }

    [HttpPost("CreateLesson")]
    public IActionResult CreateLesson([FromBody] LessonViewModel lesson)
    {
        if (lesson == null)
        {
            return BadRequest("Invalid lesson data.");
        }

        var result = _lessonRepository.CreateLesson(lesson);

        if (!result.IsSuccess)
        {
            return BadRequest(result.Message);
        }

        return CreatedAtAction(nameof(GetLessonById), new { id = result.Data?.Lesson?.id }, result);
    }

    [HttpPut("UpdateLesson/{id}")]
    public IActionResult UpdateLesson(int id, [FromBody] LessonViewModel lesson)
    {
        if (lesson == null)
        {
            return BadRequest("Invalid lesson data.");
        }

        var updatedLesson = _lessonRepository.UpdateLesson(id, lesson);

        if (updatedLesson == null)
        {
            return NotFound("Lesson not found.");
        }

        return Ok(updatedLesson);
    }

    [HttpPatch("PatchLesson/{id}")]
    public IActionResult PatchLesson(int id, [FromBody] LessonViewModel lesson)
    {
        if (lesson == null)
        {
            return BadRequest("Invalid lesson data.");
        }

        var patchedLesson = _lessonRepository.PatchLesson(id, lesson);

        if (patchedLesson == null)
        {
            return NotFound("Lesson not found.");
        }

        return Ok(patchedLesson);
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteLesson(int id)
    {
        var deleted = _lessonRepository.DeleteLesson(id);
        if (!deleted)
        {
            return NotFound("Lesson not found.");
        }

        return Ok(new { message = "Lesson deleted successfully." });
    }
}
