namespace LearningManagementSystem.Api.Controllers.AuthEndpoints
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IValidator<UsersViewModels> _registrationValidator;
        private readonly IValidator<LoginRequest> _loginValidator;
        // private readonly IConfiguration _configuration;
        // private readonly AuthService _authService;
        private readonly AppDbContext _context;

        // private readonly IUserServices _userServices;

        private readonly IResponseService _responseService;
        private readonly IAuthRepository _authRepository;
        // private readonly IUploadImageRepository _imageUpload;
        //private readonly ILogger _logger;

        public AuthController(
            IValidator<UsersViewModels> registrationValidator,
            IValidator<LoginRequest> loginValidator,
            // IConfiguration configuration,
            // AuthService authService,
            AppDbContext context,
            IResponseService responseService,
            // IUserServices userServices,
            IAuthRepository authRepository
        // IUploadImageRepository imageUpload
        //ILogger logger
        )
        {
            _registrationValidator = registrationValidator;
            _loginValidator = loginValidator;
            // _configuration = configuration;
            // _authService = authService;
            _context = context;
            _responseService = responseService;
            //_logger = logger;
            // _userServices = userServices;
            _authRepository = authRepository;
            // _imageUpload = imageUpload;
        }

        #region test-endpoints Authorization
        // [HttpGet("instructor")]
        // public async Task<IActionResult> InstructorEndpoint(int id)
        // {
        //     var student = await _userServices.GetInstructor(id);
        //     if (student == null)
        //     {
        //         return NotFound("Student not found");
        //     }
        //     return Ok(student);
        // }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpGet("admin-only")]
        public IActionResult AdminEndpoint()
        {
            return Ok("Admin access granted");
        }

        [Authorize(Policy = "RequireInstructorRole")]
        [HttpGet("instructor-student")]
        public IActionResult InstructorEndpoint()
        {
            return Ok("Instructor/Student access granted");
        }

        [Authorize(Policy = "RequireWorkerRole")]
        [HttpGet("worker-only")]
        public IActionResult WorkerEndpoint()
        {
            return Ok("Worker access granted");
        }

        [Authorize(Policy = "RequireInstructorRole")]
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
                .ThenInclude(u => u.TblRole)
                .ToListAsync();
            return Ok(instructors.Count);
        }

        #endregion

        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return _responseService.Response(Response<object>.Error("User not authenticated"));
            }

            var token = await _context.Tokens
                .FirstOrDefaultAsync(t => t.user_id == int.Parse(userId));

            if (token != null)
            {
                _context.Tokens.Remove(token);
                await _context.SaveChangesAsync();
            }

            return _responseService.Response(Response<object>.Success<object>(null, "User logged out successfully"));
        }
        //v2
        [HttpPost("register")]
        public async Task<IActionResult> Register(UsersViewModels model)
        {
            var validationResult = await _registrationValidator.ValidateAsync(model);
            if (!validationResult.IsValid)
            {
                return _responseService.Response(
                    Response<object>.ValidationError(validationResult.Errors.First().ErrorMessage)
                );
            }

            var registerResponse = await _authRepository.Register(model);
            if (!registerResponse.IsSuccess)
            {
                return _responseService.Response(
                    Response<object>.Error(registerResponse.Message)
                );
            }

            return _responseService.Response(
                Response<object>.Success(registerResponse.Data, registerResponse.Message)
            );
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var validationResult = await _loginValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var loginResponse = await _authRepository.Login(request);
            if (!loginResponse.IsSuccess)
            {
                return _responseService.Response(
                    Response<object>.Error(loginResponse.Message)
                );
            }

            return _responseService.Response(
                Response<object>.Success(loginResponse.Data, loginResponse.Message)
            );
        }

        [Authorize]
        [HttpPost("refreshToken")]
        public async Task<IActionResult> RefreshToken(RefreshTokenRequest request)
        {
            var refreshTokenResponse = await _authRepository.RefreshToken(request);
            if (!refreshTokenResponse.IsSuccess)
            {
                return _responseService.Response(
                    Response<object>.Error(refreshTokenResponse.Message)
                );
            }

            return _responseService.Response(
                Response<object>.Success(refreshTokenResponse.Data, refreshTokenResponse.Message)
            );
        }
        #region v1
        // [HttpPost("v1/register")]
        // public async Task<IActionResult> Register(UsersViewModels model)
        // {
        //     var validationResult = await _registrationValidator.ValidateAsync(model);
        //     if (!validationResult.IsValid)
        //     {
        //         return BadRequest(validationResult.Errors);
        //     }

        //     var role = await _context.Roles.FindAsync(model.role_id);
        //     if (role == null)
        //     {
        //         return BadRequest("Invalid role ");
        //     }

        //     var currentTime = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");
        //     var formattedDateTime = DateTime.Parse(currentTime);

        //     var user = new TblUsers
        //     {
        //         username = model.username,
        //         email = model.email,
        //         password = BCrypt.Net.BCrypt.HashPassword(model.password),
        //         phone = model.phone,
        //         dob = model.dob,
        //         address = model.address,
        //         profile_photo = model.profile_photo,
        //         role_id = role.id,
        //         // created_at = DateTime.UtcNow,
        //         created_at = formattedDateTime,

        //     };

        //     await _context.Users.AddAsync(user);
        //     await _context.SaveChangesAsync();


        //     switch (role.role.ToLower())
        //     {
        //         case "student":
        //             await _context.Students.AddAsync(new TblStudents
        //             {
        //                 user_id = user.id,
        //                 // created_at = DateTime.UtcNow
        //                 created_at = formattedDateTime
        //             });
        //             break;
        //         case "instructor":
        //             await _context.Instructors.AddAsync(new TblInstructors
        //             {
        //                 user_id = user.id,
        //                 nrc = model.nrc,
        //                 edu_background = model.edu_background,
        //                 created_at = DateTime.UtcNow

        //             });
        //             break;
        //         case "admin":
        //             await _context.Admins.AddAsync(new TblAdmins
        //             {
        //                 user_id = user.id,
        //                 created_at = DateTime.UtcNow
        //             });
        //             break;

        //     }

        //     await _context.SaveChangesAsync();

        //     var token = await _authService.GenerateToken(user, role);
        //     var response = new responseUser
        //     {
        //         id = user.id,
        //         username = user.username,
        //         role = role.role
        //     };

        //     var details = new ApiDetails
        //     {
        //         AdditionalProp1 = "User registered successfully.",
        //         AdditionalProp2 = "Welcome to Lms Platform",
        //         AdditionalProp3 = "Role: " + role.role
        //     };
        //     // return Ok(new
        //     // {
        //     //     token,
        //     //     response
        //     //     //user.id,user.username, role.role
        //     // });

        //     return Ok(new ApiResponse
        //     {
        //         Status = "Success",
        //         Data = new { token, response },
        //         Error = null,
        //         Message = "User registered successfully.",
        //         Details = details
        //     });

        // }

        //v1
        // [HttpPost("login")]
        // public async Task<IActionResult> Login(LoginRequest request)
        // {
        //     var validationResult = await _loginValidator.ValidateAsync(request);
        //     if (!validationResult.IsValid)
        //     {
        //         return BadRequest(validationResult.Errors);
        //     }

        //     var user = await _context.Users.FirstOrDefaultAsync(u => u.email == request.Email && !u.isDeleted);
        //     var confirmPassword = BCrypt.Net.BCrypt.Verify(request.Password, user.password);
        //     if (user == null || !confirmPassword)
        //     {
        //         return Unauthorized(new
        //         {
        //             message = "Invalid Credentials"
        //         });
        //     }

        //     var role = await _context.Roles.FindAsync(user.role_id);
        //     var token = await _authService.GenerateToken(user, role);

        //     var response = new responseUser
        //     {
        //         id = user.id,
        //         username = user.username,
        //         role = role.role
        //     };

        //     var details = new ApiDetails
        //     {
        //         AdditionalProp1 = "User logged in successfully.",
        //         AdditionalProp2 = "Welcome to Lms Platform " + user.username,
        //         AdditionalProp3 = "Role: " + role.role
        //     };

        //     // return Ok(new
        //     // {
        //     //     token,
        //     //     //user,
        //     //     response
        //     // });

        //     return Ok(new ApiResponse
        //     {
        //         Status = "Success",
        //         Data = new { token, response },
        //         Error = null,
        //         Message = "User logged in successfully.",
        //         Details = details
        //     });
        // }

        //v1.1
        // [HttpPost("signUp")]
        // public async Task<IActionResult> SignUp(UsersViewModels model)
        // {
        //     var validationResult = await _registrationValidator.ValidateAsync(model);
        //     if (!validationResult.IsValid)
        //     {
        //         return _responseService.Response(
        //             Response<object>.ValidationError(validationResult.Errors.First().ErrorMessage)
        //         );
        //     }

        //     var role = await _context.Roles.FindAsync(model.role_id);
        //     if (role == null)
        //     {
        //         return _responseService.Response(
        //             Response<object>.ValidationError("Invalid role")
        //         );
        //     }

        //     var user = new TblUsers
        //     {
        //         username = model.username,
        //         email = model.email,
        //         password = BCrypt.Net.BCrypt.HashPassword(model.password),
        //         phone = model.phone,
        //         dob = model.dob,
        //         address = model.address,
        //         profile_photo = model.profile_photo,
        //         role_id = role.id,
        //         // created_at = DateTime.UtcNow,
        //         created_at = DateTime.UtcNow,
        //     };

        //     await _context.Users.AddAsync(user);
        //     await _context.SaveChangesAsync();

        //     switch (role.role.ToLower())
        //     {
        //         case "student":
        //             await _context.Students.AddAsync(new TblStudents
        //             {
        //                 user_id = user.id,
        //                 // created_at = DateTime.UtcNow
        //                 created_at = DateTime.UtcNow
        //             });
        //             break;
        //         case "instructor":
        //             await _context.Instructors.AddAsync(new TblInstructors
        //             {
        //                 user_id = user.id,
        //                 nrc = model.nrc,
        //                 edu_background = model.edu_background,
        //                 created_at = DateTime.UtcNow

        //             });
        //             break;
        //         case "admin":
        //             await _context.Admins.AddAsync(new TblAdmins
        //             {
        //                 user_id = user.id,
        //                 created_at = DateTime.UtcNow
        //             });
        //             break;

        //     }

        //     await _context.SaveChangesAsync();

        //     var token = await _authService.GenerateToken(user, role);
        //     var newUser = new UserResponse
        //     {
        //         id = user.id,
        //         username = user.username,
        //         email = user.email,
        //         password = user.password,
        //         phone = user.phone,
        //         dob = user.dob,
        //         address = user.address,
        //         // profile_photo = user.profile_photo,
        //         created_at = user.created_at,
        //         role_id = user.role_id,
        //         role = role.role,
        //         //instructor
        //         nrc = model.nrc,
        //         edu_background = model.edu_background,
        //         // is_available = false
        //     };

        //     var response = new SignInResponse
        //     {
        //         token = token,
        //         response = newUser
        //     };

        //     // var details = new ApiDetails
        //     // {
        //     //     AdditionalProp1 = "User registered successfully.",
        //     //     AdditionalProp2 = "Welcome to Lms Platform " + user.username,
        //     //     AdditionalProp3 = "Role: " + role.role
        //     // };

        //     return _responseService.Response(
        //         Response<object>.Success(response, "User registered successfully.")
        //     );
        // }
        // [HttpPost("signIn")]
        // public async Task<IActionResult> SignIn(LoginRequest request)
        // {
        //     var validationResult = await _loginValidator.ValidateAsync(request);
        //     if (!validationResult.IsValid)
        //     {
        //         return _responseService.Response(
        //             Response<object>.ValidationError(validationResult.Errors.First().ErrorMessage)
        //         );

        //     }

        //     var user = await _context.Users.FirstOrDefaultAsync(u => u.email == request.Email && !u.isDeleted);
        //     var confirmPassword = user != null && BCrypt.Net.BCrypt.Verify(request.Password, user.password);

        //     if (user == null || !confirmPassword)
        //     {
        //         return _responseService.Response(
        //             Response<object>.Error("Invalid Credentials")
        //         );
        //     }

        //     var role = await _context.Roles.FindAsync(user.role_id);
        //     var token = await _authService.GenerateToken(user, role);
        //     var response = new responseUser
        //     {
        //         id = user.id,
        //         username = user.username,
        //         role = role.role
        //     };

        //     var LoginResponse = new { token, response };

        //     return _responseService.Response(Response<object>.Success(LoginResponse, "User logged in successfully."));
        // }



        // [HttpPost("refreshToken")]
        // public async Task<IActionResult> RefreshToken(TokenRequest request)
        // {
        //     if (string.IsNullOrEmpty(request.token) || !request.token.StartsWith("Bearer "))
        //     {
        //         return BadRequest(new
        //         {
        //             status = 400,
        //             message = "Invalid token format"
        //         });
        //     }
        //     var token = request.token.Substring("Bearer ".Length);
        //     //Decode the token
        //     var handler = new JwtSecurityTokenHandler();
        //     var jsonToken = handler.ReadToken(token) as JwtSecurityToken;

        //     var userId = jsonToken.Claims.First(
        //         claim => claim.Type == ClaimTypes.NameIdentifier).Value;

        //     var user = await _context.Users
        //         .Include(u => u.TblRole)
        //         .FirstOrDefaultAsync(u => u.id.ToString() == userId);

        //     if (user == null)
        //     {
        //         return Unauthorized(new
        //         {
        //             status = 401,
        //             //message = "Invalid or expired token"
        //             message = "User not found"
        //         });
        //     }

        //     var validToken = await _context.Tokens
        //         .FirstOrDefaultAsync(t => t.user_id == user.id);

        //     if (validToken == null)
        //     {
        //         return Unauthorized(new
        //         {
        //             status = 401,
        //             message = "No valid token found"
        //         });
        //     }


        //     //var tokenEntry = await _context.Tokens
        //     //    //.Include(t => t.TblUser)
        //     //    //.ThenInclude(u => u.TblRole)
        //     //    .FirstOrDefaultAsync(t => t.token == token);

        //     //if (tokenEntry == null)
        //     //{
        //     //    return Unauthorized(new
        //     //    {
        //     //        status = 401,
        //     //        message = "Invalid or expired token"
        //     //    });
        //     //}

        //     //var newToken = await _authService.GenerateToken(tokenEntry.TblUser, tokenEntry.TblUser.TblRole);

        //     var details = new ApiDetails
        //     {
        //         AdditionalProp1 = "Token is Refresh successfully.",
        //         AdditionalProp2 = "Dear " + user.username + " your Token is Refreshed",
        //         AdditionalProp3 = "Token is Refreshed at : " + DateTime.UtcNow,
        //     };
        //     var newToken = await _authService.GenerateToken(user, user.TblRole);
        //     return Ok(new ApiResponse
        //     {
        //         Status = "Success",
        //         Data = new { newToken },
        //         Error = null,
        //         Message = "Token is Refresh successfully.",
        //         Details = details
        //     });
        // }



        // public class TokenRequest
        // {
        //     public string token { get; set; }
        // }


        // public class responseUser()
        // {
        //     public int id { get; set; }
        //     public string username { get; set; }
        //     public string role { get; set; }
        // }

        // public class SignInResponse
        // {
        //     public string token { get; set; }
        //     public object response { get; set; }
        // }
        #endregion

    }

}
