namespace LearningManagementSystem.Api.Controllers.Endpoints;

[Route("api/[controller]")]
[ApiController]
public class EnrollmentController : ControllerBase
{
    private readonly IEnrollmentRepository _enrollmentRepository;

    public EnrollmentController(IEnrollmentRepository enrollmentRepository)
    {
        _enrollmentRepository = enrollmentRepository;
    }

    [HttpGet("GetEnrollments")]
    public async Task<IActionResult> GetEnrollments()
    {
        var items = await _enrollmentRepository.GetEnrollments();

        return Ok(items);
    }

    [HttpGet("GetEnrollment")]
    public async Task<IActionResult> GetEnrollment(int id)
    {
        var items = await _enrollmentRepository.GetEnrollment(id);

        return Ok(items);
    }

    [HttpPost("CreateEnrollment")]
    public async Task<IActionResult> CreateEnrollment(EnrollmentViewModels user)
    {
        var items = await _enrollmentRepository.CreateEnrollment(user);

        return Ok(items);
    }

    [HttpPut("UpdateEnrollment")]
    public async Task<IActionResult> UpdateEnrollment(int id, EnrollmentViewModels user)
    {

        var item = await _enrollmentRepository.UpdateEnrollment(id, user);

        if (item is null)
        {
            return BadRequest("Don`t have data");
        }
        return Ok(item);
    }

    [HttpPatch("PatchEnrollment")]
    public async Task<IActionResult> PatchEnrollment(int id, EnrollmentViewModels user)
    {

        var item = await _enrollmentRepository.PatchEnrollment(id, user);

        if (item is null)
        {
            return BadRequest("Don`t have data");
        }
        return Ok(item);
    }

    [HttpDelete("DeleteEnrollment")]
    public async Task<IActionResult> DeleteEnrollment(int id)
    {
        var item = await _enrollmentRepository.DeleteEnrollment(id);

        if (item is null)
        {
            return BadRequest("Don`t have data");
        }
        return Ok("Deleting success");
    }
}
