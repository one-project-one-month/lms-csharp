using Azure;
using LearningManagementSystem.DataBase.Data;
using LearningManagementSystem.DataBase.Models;
using LearningManagementSystem.Domain.Models;
using LearningManagementSystem.Domain.Services.CategoryServices;
using LearningManagementSystem.Domain.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.Domain.Services.CourseService
{
    public class CourseService : ICourseService
    {
        private readonly AppDbContext _context;

        public CourseService(AppDbContext context)
        {
            _context = context;
        }     


        // Retrieve all courses
        public async Task<Result<List<CourseResponseModel>>> GetAllCoursesAsync()
        {
            try
            {
                var courses = await _context.Courses
                    .AsNoTracking()
                    .Where(c => !c.isDeleted)
                    .Include(c => c.TblInstructor)
                    .ThenInclude(i => i.TblUser)
                    .Include(c => c.TblCategory)
                    .ToListAsync();

                var responseModels = courses.Select(MapCourseToResponseModel).ToList();


                return Result<List<CourseResponseModel>>.Success(responseModels, "Courses retrieved successfully.");
            }
            catch (Exception ex)
            {
                return HandleException<List<CourseResponseModel>>(ex);
            }
        }



        // Retrieve all courses by instructor ID
        public async Task<Result<List<CourseResponseModel>>> GetAllCoursesByInstructorIdAsync(int id)
        {
            try
            {
                var courses = await _context.Courses
                    .AsNoTracking()
                    .Where(c => c.instructor_id == id && !c.isDeleted)
                    .Include(c => c.TblInstructor)
                    .Include(c => c.TblCategory)
                    .ToListAsync();

                var responseModels = courses.Select(MapCourseToResponseModel).ToList();

                return Result<List<CourseResponseModel>>.Success(responseModels, "Courses retrieved successfully.");
            }
            catch (Exception ex)
            {
                return HandleException<List<CourseResponseModel>>(ex);
            }
        }



        // Retrieve course by ID
        public async Task<Result<CourseResponseModel>> GetCourseByIdAsync(int courseId)
        {
            try
            {
                var course = await _context.Courses
                    .AsNoTracking()
                    .Where(c => c.id == courseId && !c.isDeleted)
                    .Include(c => c.TblInstructor)
                    .ThenInclude(i => i.TblUser)
                    .Include(c => c.TblCategory)                  
                    .FirstOrDefaultAsync();

                if (course == null)
                    return Result<CourseResponseModel>.Error("Course not found.");

                var responseModel = MapCourseToResponseModel(course);

                return Result<CourseResponseModel>.Success(responseModel, "Course retrieved successfully.");
            }
            catch (Exception ex)
            {
                return HandleException<CourseResponseModel>(ex);
            }
        }



        //Update Course
        public CourseResponseModel UpdateCourse(int id, CourseRequestModel request)
        {
            var model = _context.Courses
               .Include(c => c.TblCategory) // Load category
               .Include(c => c.TblInstructor)
               .ThenInclude(i => i.TblUser) // Load instructor and user
               .FirstOrDefault(c => c.id == id);

            if (model == null)
            {
                throw new InvalidOperationException("Course not found.");
            }

            model.thumbnail = request.thumbnail;
            model.courseName = request.courseName;
            model.description = request.description;
            model.is_available = request.isAvailable;
            model.type = request.type;
            model.level = request.level;
            model.current_price = request.currentPrice;
            model.original_price = request.originalPrice;
            model.updated_at = DateTime.UtcNow;
            model.category_id = request.categoryId;

            _context.Entry(model).State = EntityState.Modified;

            _context.SaveChanges();

            return MapCourseToResponseModel(model);
        }


        //Add Course
        //public CourseResponseModel CreateCourse(CourseRequestModel request , IFormFile image)
        //{
        //    var instructor = _context.Instructors.Find(request.instructorId);

        //    if (instructor == null)
        //    {
        //        throw new InvalidOperationException("Invalid Instructor Id");
        //    }
        
        //    TblCourses newCourse = MapRequestToCourseEntity(request , image);

        //    _context.Courses.Add(newCourse);

        //    _context.SaveChanges();

        //    var response = MapCourseToResponseModel(newCourse);


        //    return response;
        //}



        //Add Course
        public CourseResponseModel CreateCourse(CourseRequestModel request)
        {
            var instructor = _context.Instructors.Find(request.instructorId);

            if (instructor is null)
            {
                throw new InvalidOperationException("Invalid Instructor Id");
            }


            TblCourses newCourse = MapRequestToCourseEntity(request);

            _context.Courses.Add(newCourse);

            _context.SaveChanges();

            return new CourseResponseModel
            {
                id = newCourse.id,
                courseName = newCourse.courseName,
                categoryName = newCourse.TblCategory.name,
                description = newCourse.description,
                thumbnail = newCourse.thumbnail,
                isAvailable = newCourse.is_available,
                type = newCourse.type,
                level = newCourse.level,
                duration = newCourse.duration,
                currentPrice = newCourse.current_price,
                originalPrice = newCourse.original_price,
                createdAt = newCourse.created_at,
                updatedAt = newCourse.updated_at,
            };


        }



        //Remove Course
        public bool DeleteCourse(int id)
        {
            var model = _context.Courses.Find(id);

            if (model == null)
            {
                throw new InvalidOperationException("Course not found.");
            }

            model.isDeleted = true;

            // Set the entity state to Modified
            _context.Entry(model).State = EntityState.Modified;

            _context.SaveChanges();

            return true;
        }



        public async Task<CourseResponseModel> PatchCourseAsync(int courseId, JsonPatchDocument<CourseRequestModel> patchDoc)
        {
            var course = await _context.Courses.FindAsync(courseId);

            if (course == null)
            {
                throw new KeyNotFoundException("Course not found.");
            }

            var courseRequest = new CourseRequestModel
            {
                instructorId = course.instructor_id,
                categoryId = course.category_id,
                courseName = course.courseName,
                type = course.type,
                level = course.level,
                duration = course.duration,
                createAt = course.created_at,
                description = course.description,
                currentPrice = course.current_price,
                thumbnail = course.thumbnail,
                isAvailable = course.is_available,
                originalPrice = course.original_price
            };

            patchDoc.ApplyTo(courseRequest);

            var updatedCourse = UpdateCourse(courseId, courseRequest);

            return updatedCourse;
        }



        // Helper method to map Course to CourseResponseModel
        private static CourseResponseModel MapCourseToResponseModel(TblCourses course)
        {
            return new CourseResponseModel
            {
                id = course.id,
                courseName = course.courseName,
                categoryName = course.TblCategory.name,
                description = course.description,
                thumbnail = course.thumbnail,
                isAvailable = course.is_available,
                type = course.type,
                level = course.level,
                duration = course.duration,
                currentPrice = course.current_price,
                originalPrice = course.original_price,
                createdAt = course.created_at,
                updatedAt = course.updated_at,
                instructorName = course.TblInstructor.TblUser.username,
                educationBackgroun = course.TblInstructor.edu_background
            };
        }



        private  TblCourses MapRequestToCourseEntity(CourseRequestModel request)
        {
            var category = _context.Category.Find(request.categoryId);
            if (category == null)
            {
                throw new InvalidOperationException("Invalid Category Id");
            }

            var instructor = _context.Instructors.Find(request.instructorId);
            if (instructor == null)
            {
                throw new InvalidOperationException("Invalid Instructor Id");
            }

            //var socialLink = _context.Social_Links.Find(request.socialLinkId);
            //if (socialLink == null)
            //{
            //    throw new InvalidOperationException("Invalid Social Link Id");
            //}

            var newCourse = new TblCourses
            {
                courseName = request.courseName,
                thumbnail = request.thumbnail,
                is_available = request.isAvailable,
                type = request.type,
                level = request.level,
                duration = request.duration,
                description = request.description,
                current_price = request.currentPrice,
                original_price = request.originalPrice,
                created_at = request.createAt,

                // Assign foreign keys
                category_id = request.categoryId,
                instructor_id = request.instructorId,
                //social_link_id = request.socialLinkId,

                // Assign navigation properties
                TblCategory = category,
                TblInstructor = instructor,
                //TblSocial_Link = socialLink
            };

            return newCourse;
        }




        //private TblCourses MapRequestToCourseEntity(CourseRequestModel request, IFormFile? image)
        //{
        //    var category = _context.Category.Find(request.categoryId);
        //    if (category == null)
        //    {
        //        throw new InvalidOperationException("Invalid Category Id");
        //    }

        //    var instructor = _context.Instructors.Find(request.instructorId);
        //    if (instructor == null)
        //    {
        //        throw new InvalidOperationException("Invalid Instructor Id");
        //    }

        //    string thumbnailPath = string.Empty; // Default value if no image is provided

        //    if (image != null && image.Length > 0)
        //    {
        //        var uploadsFolder = Path.Combine("wwwroot/images");
        //        Directory.CreateDirectory(uploadsFolder); // Ensure the folder exists

        //        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
        //        var filePath = Path.Combine(uploadsFolder, fileName);

        //        using (var stream = new FileStream(filePath, FileMode.Create))
        //        {
        //            image.CopyToAsync(stream).Wait(); // Ensure it completes before proceeding
        //        }

        //        thumbnailPath = "/images/" + fileName; // Assign relative path for access
        //    }

        //    var newCourse = new TblCourses
        //    {
        //        courseName = request.courseName,
        //        thumbnail = thumbnailPath, // Use assigned thumbnail path or empty string
        //        is_available = request.isAvailable,
        //        type = request.type,
        //        level = request.level,
        //        duration = request.duration,
        //        description = request.description,
        //        current_price = request.currentPrice,
        //        original_price = request.originalPrice,
        //        created_at = request.createAt,

        //        // Assign foreign keys
        //        category_id = request.categoryId,
        //        instructor_id = request.instructorId,

        //        // Assign navigation properties
        //        TblCategory = category,
        //        TblInstructor = instructor
        //    };

        //    return newCourse;
        //}


        // Helper method to handle exceptions



        private Result<T> HandleException<T>(Exception ex)
        {
            if (ex is NullReferenceException nullEx)
                return Result<T>.Error($"Null reference error: {nullEx.Message}");
            else if (ex is ValidationException validationEx)
                return Result<T>.ValidationError($"Validation error: {validationEx.Message}");
            else if (ex is SystemException systemEx)
                return Result<T>.SystemError($"System error: {systemEx.Message}");
            else
                return Result<T>.Error($"An unexpected error occurred: {ex.Message}");
        }


    }
}
