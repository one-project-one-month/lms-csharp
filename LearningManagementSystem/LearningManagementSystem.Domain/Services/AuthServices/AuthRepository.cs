using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using LearningManagementSystem.Domain.Services.AuthServices.Requests;
using LearningManagementSystem.Domain.Services.AuthServices.Responses;
using LearningManagementSystem.Domain.Services.AuthServices.Validators;
using LearningManagementSystem.Domain.Services.ResponseService;
using Microsoft.AspNetCore.Http.HttpResults;

namespace LearningManagementSystem.Domain.Services.AuthServices
{
    public class AuthRepository : IAuthRepository
    {
        private readonly AppDbContext _db;
        private readonly IResponseService _responseService;
        private readonly AuthService _authService;

        public AuthRepository(AppDbContext db, IResponseService responseService, AuthService authService)
        {
            _db = db;
            _responseService = responseService;
            _authService = authService;
        }

        public async Task<Response<RefreshTokenResponse>> RefreshToken(RefreshTokenRequest request)
        {
            if (string.IsNullOrEmpty(request.token) || !request.token.StartsWith("Bearer "))
            {
                return Response<RefreshTokenResponse>.Error("Invalid token");
            }

            #region get user data from jwt token
            var token = request.token.Substring("Bearer ".Length);
            // decode the token
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token) as JwtSecurityToken;

            var userId = jwtToken.Claims.First(
                claim => claim.Type == ClaimTypes.NameIdentifier
            ).Value;

            if (userId is null)
            {
                return Response<RefreshTokenResponse>.Error("User not found");
            }

            // you can now use that id to find that user in your database
            #endregion

            var user = await _db.Users
                .Include(u => u.TblRole)
                .FirstOrDefaultAsync(u => u.id == request.id);

            if (user is null)
            {
                return Response<RefreshTokenResponse>.Error("User not found");
            }

            var role = await GetRole(user.role_id);
            if (role is null)
            {
                return Response<RefreshTokenResponse>.Error("Role not found");
            }

            var refreshToken = await _authService.GenerateToken(user, role);
            if (refreshToken is null)
            {
                return Response<RefreshTokenResponse>.Error("Failed to generate refresh token");
            }

            var response = new RefreshTokenResponse
            {
                refreshToken = refreshToken,
                id = user.id.ToString()
            };

            return Response<RefreshTokenResponse>.Success(response, "Token refreshed successfully");
        }

        public async Task<TblRoles> GetRole(int roleId)
        {
            var role = await _db.Roles
                .Include(r => r.TblUsers)
                .FirstOrDefaultAsync(r => r.id == roleId);
            if (role is null)
            {
                throw new KeyNotFoundException($"Role with ID {roleId} not found");
            }

            return role;
        }

        public async Task<Response<LoginResponse>> Login(LoginRequest request)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.email == request.Email && !u.isDeleted);
            var confirmPassword = user != null && BCrypt.Net.BCrypt.Verify(request.Password, user.password);

            if (user is null || !confirmPassword)
            {
                return Response<LoginResponse>.Error("Invalid Credentials");
            }

            var role = await GetRole(user.role_id);
            if (role is null)
            {
                return Response<LoginResponse>.Error("Role not found");
            }

            var token = await _authService.GenerateToken(user, role);
            responseUser loginUser = mapResponseUser(role, user);
            var response = new LoginResponse
            {
                token = token,
                user = loginUser,
                // details = new ApiDetails
            };

            return Response<LoginResponse>.Success(response, "User logged in successfully.");
        }

        public async Task<Response<RegisterResponse>> Register(UsersViewModels model)
        {
            var role = await GetRole(model.role_id);
            if (role == null)
            {
                return Response<RegisterResponse>.Error("Role not found");
            }

            TblUsers user = MapUser(model, role);

            await _db.Users.AddAsync(user);
            await _db.SaveChangesAsync();

            await MapUserWithRole(model, role, user);

            await _db.SaveChangesAsync();

            var token = await _authService.GenerateToken(user, role);
            responseUser registerUser = mapResponseUser(role, user);
            // not using for now 
            // var details = new ApiDetails
            // {
            //     AdditionalProp1 = "User registered successfully.",
            //     AdditionalProp2 = "Welcome to Lms Platform",
            //     AdditionalProp3 = "Role: " + role.role
            // };

            var response = new RegisterResponse
            {
                token = token,
                user = registerUser,
                // details = details
            };

            return Response<RegisterResponse>.Success(response, "User registered successfully");
        }

        private static responseUser mapResponseUser(TblRoles role, TblUsers user)
        {
            return new responseUser
            {
                id = user.id,
                username = user.username,
                email = user.email,
                phone = user.phone,
                dob = user.dob,
                address = user.address,
                profile_photo = user.profile_photo,
                role = role.role,
            };
        }

        private async Task MapUserWithRole(UsersViewModels model, TblRoles role, TblUsers user)
        {
            switch (role.role.ToLower())
            {
                case "student":
                    await _db.Students.AddAsync(new TblStudents
                    {
                        user_id = user.id,
                        created_at = DateTime.UtcNow
                    });
                    break;
                case "instructor":
                    await _db.Instructors.AddAsync(new TblInstructors
                    {
                        user_id = user.id,
                        nrc = model.nrc,
                        edu_background = model.edu_background,
                        created_at = DateTime.UtcNow
                    });
                    break;
                case "admin":
                    await _db.Admins.AddAsync(new TblAdmins
                    {
                        user_id = user.id,
                        created_at = DateTime.UtcNow
                    });
                    break;
            }
        }

        private static TblUsers MapUser(UsersViewModels model, TblRoles role)
        {
            return new TblUsers
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
        }


    }
}