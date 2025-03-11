//using Microsoft.AspNetCore.Mvc;

//namespace LearningManagementSystem.Api.Controllers.Endpoints
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class UsersController : ControllerBase
//    {
//        //private readonly IUserRepository _userRepository;

//        //public UsersController(IUserRepository userRepository)
//        //{
//        //    _userRepository = userRepository;
//        //}

//        //[HttpGet("HelloWorld")]
//        //public IActionResult HelloWorld()
//        //{
//        //    return Ok("Hello Worlds");
//        //}

//        //[HttpGet("GetStudents")]
//        //public IActionResult GetStudents()
//        //{
//        //    var items = _userRepository.GetStudents();

//        //    return Ok(items);
//        //}

//        //[HttpGet("GetInstructors")]
//        //public IActionResult GetInstructors()
//        //{
//        //    var items = _userRepository.GetInstructors();

//        //    return Ok(items);
//        //}

//        //[HttpGet("GetStudent/{id}")]
//        //public IActionResult GetStudent(int id)
//        //{
//        //    var items = _userRepository.GetStudent(id);

//        //    return Ok(items);
//        //}

//        //[HttpGet("GetInstructor/{id}")]
//        //public IActionResult GetInstructor(int id)
//        //{
//        //    var items = _userRepository.GetInstructor(id);

//        //    return Ok(items);
//        //}

//        //[HttpPost("Register")]
//        //public IActionResult CreateUser(UsersViewModels user)
//        //{
//        //    var items = _userRepository.CreateUser(user);

//        //    return Ok(items);
//        //}

//        //[HttpPut("UpdateUser/{id}")]
//        //public IActionResult UpdateUser(int id,UsersViewModels user)
//        //{

//        //    var item = _userRepository.UpdateUser(id,user);

//        //    // need to write reponse & request

//        //    if (item is null)
//        //    {
//        //        return BadRequest("Don`t have data");
//        //    }
//        //    return Ok(item);
//        //}

//        //[HttpPatch("PatchUser/{id}")]
//        //public IActionResult PatchUser(int id, UsersViewModels user)
//        //{

//        //    var item = _userRepository.PatchUser(id, user);

//        //    // need to write reponse & request

//        //    if (item is null)
//        //    {
//        //        return BadRequest("Don`t have data");
//        //    }
//        //    return Ok(item);
//        //}

//        //[HttpDelete("{id}")]
//        //public IActionResult DeleteUser(int id)
//        //{
//        //    var item = _userRepository.DeleteUser(id);

//        //    if (item is null)
//        //    {
//        //        return BadRequest("Don`t have data");
//        //    }
//        //    return Ok("Deleting success");
//        //}
//    }
//}
