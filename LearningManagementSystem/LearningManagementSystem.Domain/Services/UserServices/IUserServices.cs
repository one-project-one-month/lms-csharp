using System;
using LearningManagementSystem.Domain.Services.ResponseService;
using LearningManagementSystem.Domain.Services.UserServices.Requests;
using LearningManagementSystem.Domain.Services.UserServices.Responses;

namespace LearningManagementSystem.Domain.Services.UserServices;

public interface IUserServices
{
    // public string CreateUser(UserRequest user);
    Task<Response<UserChangePasswordResponse>> ChangePassword(UserChangePasswordRequest request);
    Task<Response<List<UserResponse>>> GetUsers();
    Task<Response<UserResponse>> GetUser(int id);
    Task<Response<List<UserResponse>>> GetStudents();
    Task<Response<List<UserResponse>>> GetInstructors();
    Task<Response<UserResponse>> GetStudent(int id);
    Task<Response<UserResponse>> GetInstructor(int id);
    Task<Response<UserResponse>> UpdateUser(int id, UserRequest user);
    Task<Response<UserResponse>> PatchUser(int id, UserRequest user);
    Task<Response<bool>> DeleteUser(int id);
}
