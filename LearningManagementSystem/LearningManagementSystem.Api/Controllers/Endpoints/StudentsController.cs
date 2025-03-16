namespace LearningManagementSystem.Api.Controllers.Endpoints;

[Authorize(Roles = "Student")]
[Route("api/[controller]")]
[ApiController]

public class StudentsController : ControllerBase
{
    private readonly IStudentRepository _studentRepository;

    public StudentsController(IStudentRepository studentRepository)
    {
        _studentRepository = studentRepository;
    }

    [HttpGet]

    public IActionResult GetStudents()
    {
        var students = _studentRepository.GetStudents();
        return Ok(students);
    }

    [HttpPost("Register")]
    public IActionResult CreateStudent([FromForm] StudentsViewModels user)
    {
        var item = _studentRepository.CreateStudent(user);
        return Ok(item);
    }

    [HttpGet("GetStudent/{id}")]
    public IActionResult GetStudent(int id)
    {
        var item = _studentRepository.GetStudent(id);
        return Ok(item);
    }

    [HttpPatch("UpdateStudent/{id}")]
    public IActionResult UpdateStudent(int id, StudentsViewModels user)
    {
        var item = _studentRepository.UpdateStudent(id, user);
        return Ok(item);
    }

    [HttpDelete("DeleteStudent/{id}")]
    public IActionResult DeleteStudent(int id)
    {
        var item = _studentRepository.DeleteStudent(id);
        return Ok(item);
    }
}
