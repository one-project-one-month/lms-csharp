using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LearningManagementSystem.Domain.Services.AuthServices.Requests;
using LearningManagementSystem.Domain.Services.AuthServices.Responses;
using LearningManagementSystem.Domain.Services.ResponseService;

namespace LearningManagementSystem.Domain.Services.AuthServices
{
    public interface IAuthRepository
    {
        Task<Response<RegisterResponse>> Register(UsersViewModels model);
        Task<Response<LoginResponse>> Login(LoginRequest request);
        Task<TblRoles> GetRole(int roleId);

        Task<Response<RefreshTokenResponse>> RefreshToken(RefreshTokenRequest request);
        // Task<bool> UserExists(string username);
        // Task<TblUsers> GetUser(int id);

    }
}