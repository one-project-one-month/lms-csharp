namespace LearningManagementSystem.Api.Controllers.UserEndpoints;

[Authorize]
[Route("api/Users")]
[ApiController]
public class UsersEndpoint : ControllerBase
{
    private readonly AppDbContext _context;

    private readonly IUserServices _userServices;

    private readonly IValidator<UserRequest> _userRequestValidator;
    private readonly IResponseService _responseService;

    public UsersEndpoint(
        AppDbContext context,
        IUserServices userServices,
        IValidator<UserRequest> userRequestValidator,
        IResponseService responseService)
    {
        _context = context;
        _userServices = userServices;
        _userRequestValidator = userRequestValidator;
        _responseService = responseService;
    }


    [HttpPut("change-password")]
    public async Task<IActionResult> ChangePassword(UserChangePasswordRequest request)
    {
        var result = await _userServices.ChangePassword(request);
        if (result.Data is null)
        {
            return _responseService.Response(result);
        }
        return _responseService.Response(result);
    }


    [HttpGet("getInstructor/{id}")]
    public async Task<IActionResult> InstructorEndpoint(int id)
    { 
        var instructor = await _userServices.GetInstructor(id);
        if (instructor == null)
        {
            return NotFound("Instructor not found");
        }
        return Ok(instructor);
    }

    [HttpGet("getStudent/{id}")]
    public async Task<IActionResult> StudentsEndpoint(int id)
    { 
        var student = await _userServices.GetStudent(id);
        if (student is null)
        {
            return NotFound("Student not found");
        }
        return Ok(student);
    }

    [HttpGet("getInstructors")]
    public async Task<IActionResult> InstructorsEndpoint()
    {
        var instructors = await _userServices.GetInstructors();
        if (instructors.Data == null || !instructors.Data.Any())
        {
            return NotFound("No instructors found");
        }
        return _responseService.Response(instructors);
    }

    [HttpGet("getStudents")]
    public async Task<IActionResult> StudentsEndpoint()
    {
        var students = await _userServices.GetStudents();
        if (students.Data == null || !students.Data.Any())
        {
            return NotFound("No students found");
        }

        return _responseService.Response(students);
    }

    [HttpPatch("patch/{id}")]
    public async Task<IActionResult> PatchUser(int id, UserRequest user)
    {
        var validationResult = await _userRequestValidator.ValidateAsync(user);
        if (!validationResult.IsValid)
        {
            return _responseService.Response(
                Response<object>.ValidationError(validationResult.Errors
                .First().ErrorMessage)
            );
        }

        var result = await _userServices.PatchUser(id, user);

        return _responseService.Response(result);
    }

    [HttpPut("update/{id}")]
    public async Task<IActionResult> UpdateUser(int id, UserRequest user)
    {
        var validationResult = await _userRequestValidator
            .ValidateAsync(instance: user);

        if (!validationResult.IsValid)
        {
            return _responseService.Response(
                Response<object>.ValidationError(validationResult.Errors.First().ErrorMessage)
            );
        }

        var result = await _userServices.UpdateUser(id, user);
        return _responseService.Response(result);
    }

    [HttpGet("getUsers")]
    public async Task<IActionResult> GetUsers()
    {
        var users = await _userServices.GetUsers();
        if (users.Data == null || !users.Data.Any())
        {
            return _responseService.Response(users);
        }

        return _responseService.Response(users);
    }


    [HttpGet("getUser/{id}")]
    public async Task<IActionResult> GetUser(int id)
    {
        var user = await _userServices.GetUser(id);
        if (user.Data is null)
        {
            return _responseService.Response(user);
        }
        return _responseService.Response(user);
    }

    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var result = await _userServices.DeleteUser(id);
        if (result is null)
        {
            return NotFound("User not found");
        }
        return Ok(result);
    }
    
}

