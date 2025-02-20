using FluentValidation;
using LearningManagementSystem.DataBase.Data;
using LearningManagementSystem.DataBase.Models;
using LearningManagementSystem.Domain.Services.AuthServices;
using LearningManagementSystem.Domain.Services.AuthServices.Requests;
using LearningManagementSystem.Domain.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace LearningManagementSystem.Api.Controllers.AuthEndpoints
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IValidator<UsersViewModels> _registrationValidator;
        private readonly IValidator<LoginRequest> _loginValidator;
        private readonly IConfiguration _onfiguration;
        private readonly AuthService _authService;
        private readonly AppDbContext _context;
        //private readonly ILogger _logger;

        public AuthController(
            IValidator<UsersViewModels> registrationValidator,
            IValidator<LoginRequest> loginValidator,
            IConfiguration onfiguration,
            AuthService authService,
            AppDbContext context
        //ILogger logger
        )
        {
            _registrationValidator = registrationValidator;
            _loginValidator = loginValidator;
            _onfiguration = onfiguration;
            _authService = authService;
            _context = context;
            //_logger = logger;
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("roles")]
        public async Task<IActionResult> GetRoles()
        {
            var roles = await _context.Roles.ToListAsync();
            return Ok(roles);
        }

        [Authorize(Roles = "Instructor")]
        [HttpGet("instructors")]
        public async Task<IActionResult> GetInstructors()
        {
            var instructors = await _context.Instructors
                .Include(i => i.TblUser)
                .ToListAsync();
            return Ok(instructors);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UsersViewModels model)
        {
            var validationResult = await _registrationValidator.ValidateAsync(model);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var role = await _context.Roles.FindAsync(model.role_id);
            if (role == null)
            {
                return BadRequest("Invalid role ");
            }

            var user = new TblUsers
            {
                username = model.username,
                email = model.email,
                password = BCrypt.Net.BCrypt.HashPassword(model.password),
                phone = model.phone,
                dob = model.dob,
                address = model.address,
                profile_photo = model.profile_photo,
                role_id = role.id,
                created_at = DateTime.UtcNow,

            };

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();


            switch (role.role.ToLower())
            {
                case "student":
                    await _context.Students.AddAsync(new TblStudents
                    {
                        user_id = user.id,
                        created_at = DateTime.UtcNow
                    });
                    break;
                case "instructor":
                    await _context.Instructors.AddAsync(new TblInstructors
                    {
                        user_id = user.id,
                        nrc = model.nrc,
                        edu_background = model.edu_background,
                        created_at = DateTime.UtcNow

                    });
                    break;
                case "admin":
                    await _context.Admins.AddAsync(new TblAdmins
                    {
                        user_id = user.id,
                        created_at = DateTime.UtcNow
                    });
                    break;

            }

            await _context.SaveChangesAsync();

            var token = await _authService.GenerateToken(user, role);
            var response = new responseUser
            {
                id = user.id,
                username = user.username,
                role = role.role
            };
            return Ok(new
            {
                token,
                response
                //user.id,user.username, role.role
            });

        }

        public class responseUser()
        {
            public int id { get; set; }
            public string username { get; set; }
            public string role { get; set; }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var validationResult = await _loginValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.email == request.Email && !u.isDeleted);
            var confirmPassword = BCrypt.Net.BCrypt.Verify(request.Password, user.password);
            if (user == null)
            {
                return Unauthorized(new
                {
                    message = "Invalid Credentials"
                });
            }

            var role = await _context.Roles.FindAsync(user.role_id);
            var token = await _authService.GenerateToken(user, role);

            var response = new responseUser
            {
                id = user.id,
                username = user.username,
                role = role.role
            };

            return Ok(new
            {
                token,
                //user,
                response
            });
        }

        [HttpPost("refreshToken")]
        public async Task<IActionResult> RefreshToken(TokenRequest request)
        {
            if (string.IsNullOrEmpty(request.token) || !request.token.StartsWith("Bearer "))
            {
                return BadRequest(new
                {
                    status = 400,
                    message = "Invalid token format"
                });
            }
            var token = request.token.Substring("Bearer ".Length);
            //Decode the token
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token) as JwtSecurityToken;

            var userId = jsonToken.Claims.First(
                claim => claim.Type == ClaimTypes.NameIdentifier).Value;

            var user = await _context.Users
                .Include(u => u.TblRole)
                .FirstOrDefaultAsync(u => u.id.ToString() == userId);

            if (user == null)
            {
                return Unauthorized(new
                {
                    status = 401,
                    //message = "Invalid or expired token"
                    message = "User not found"
                });
            }

            var validToken = await _context.Tokens
                .FirstOrDefaultAsync(t => t.user_id == user.id);

            if (validToken == null)
            {
                return Unauthorized(new
                {
                    status = 401,
                    message = "No valid token found"
                });
            }


            //var tokenEntry = await _context.Tokens
            //    //.Include(t => t.TblUser)
            //    //.ThenInclude(u => u.TblRole)
            //    .FirstOrDefaultAsync(t => t.token == token);

            //if (tokenEntry == null)
            //{
            //    return Unauthorized(new
            //    {
            //        status = 401,
            //        message = "Invalid or expired token"
            //    });
            //}

            //var newToken = await _authService.GenerateToken(tokenEntry.TblUser, tokenEntry.TblUser.TblRole);
            var newToken = await _authService.GenerateToken(user, user.TblRole);
            return Ok(new
            {
                status = 200,
                acessToken = newToken
            });
        }

        public class TokenRequest
        {
            public string token { get; set; }
        }
    }
}
