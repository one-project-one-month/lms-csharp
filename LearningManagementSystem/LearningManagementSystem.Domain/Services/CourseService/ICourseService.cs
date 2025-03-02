using LearningManagementSystem.Domain.Models;
using LearningManagementSystem.Domain.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.Domain.Services.CourseService
{
    public interface ICourseService
    {
        Task<Result<List<CourseResponseModel>>> GetAllCoursesAsync();


        Task<Result<List<CourseResponseModel>>> GetAllCoursesByInstructorIdAsync(int id);


        Task<Result<CourseResponseModel>> GetCourseByIdAsync(int courseId);


        Task<CourseResponseModel> PatchCourseAsync(int courseId, JsonPatchDocument<CourseRequestModel> patchDoc);


        //CourseResponseModel CreateCourse(CourseRequestModel request , IFormFile image);


        CourseResponseModel CreateCourse(CourseRequestModel request);


        CourseResponseModel UpdateCourse(int id, CourseRequestModel request);


        bool DeleteCourse(int id);
    }

}