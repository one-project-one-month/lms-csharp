using System;
using LearningManagementSystem.DataBase.Data;
using LearningManagementSystem.Domain.Services.ResponseService;
using LearningManagementSystem.Domain.Services.UserServices.Requests;
using LearningManagementSystem.Domain.Services.UserServices.Responses;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Crypto.Generators;

namespace LearningManagementSystem.Domain.Services.UserServices;

public class UserServices : IUserServices
{
    private readonly AppDbContext _context;

    public UserServices(AppDbContext context)
    {
        _context = context;
    }
    #region simpleUserServices
    // public bool? DeleteUser(int id)
    // {
    //     var user = _context.Users.Find(id);
    //     if (user == null) return null;
    //     user.isDeleted = true;
    //     user.updated_at = DateTime.UtcNow;
    //     _context.SaveChanges();
    //     return true;
    // }

    // public UserResponse GetInstructor(int id)
    // {
    //     throw new NotImplementedException();
    // }

    // public List<UserResponse> GetInstructors()
    // {
    //     throw new NotImplementedException();
    // }

    // public UserResponse GetStudent(int id)
    // {
    //     throw new NotImplementedException();
    // }

    // public List<UserResponse> GetStudents()
    // {
    //     throw new NotImplementedException();
    // }

    // public UserResponse PatchUser(int id, UserRequest user)
    // {
    //     throw new NotImplementedException();
    // }

    // public UserResponse UpdateUser(int id, UserRequest user)
    // {
    //     throw new NotImplementedException();
    // }
    #endregion

    public async Task<Response<UserChangePasswordResponse>> ChangePassword(UserChangePasswordRequest request)
    {
        var user = await _context.Users
            .Include(u => u.TblRole)
            .Include(u => u.TblStudent)
            .Include(u => u.TblInstructor)
            .Include(u => u.TblAdmin)
            .FirstOrDefaultAsync(u => u.id == request.id);

        if (user is null)
        {
            return Response<UserChangePasswordResponse>.Error("user not found");
        }

        if (request.newPassword != request.confirmPassword)
        {
            return Response<UserChangePasswordResponse>.Error("new password and confirm password do not match");
        }

        if (request.OldPassword != null)
        {
            var verifyOldPassword = BCrypt.Net.BCrypt.Verify(request.OldPassword, user.password);
            if (!verifyOldPassword)
            {
                return Response<UserChangePasswordResponse>.Error("Old password is incorrect");
            }

        }

        if (BCrypt.Net.BCrypt.Verify(request.newPassword, user.password))
        {
            return Response<UserChangePasswordResponse>.Error("new password cannot be the same as old password");
        }
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.newPassword);

        user.password = hashedPassword;
        user.updated_at = DateTime.UtcNow;

        await _context.SaveChangesAsync();


        var response = new UserChangePasswordResponse
        {
            id = user.id,
            username = user.username,
            newPassword = request.newPassword,
            updated_at = DateTime.UtcNow

        };

        return Response<UserChangePasswordResponse>.Success(response, "password changed successfully");

        // if (request.newPassword == request.confirmPassword)
        // {
        //     if (request.OldPassword != null)
        //     {
        //         var verifyOldPassword = BCrypt.Net.BCrypt.Verify(request.OldPassword, user.password);
        //         if (!verifyOldPassword)
        //         {
        //             return Response<UserChangePasswordResponse>.Error("Old password is incorrect");
        //         }
        //     }

        //     var hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.newPassword);
        // }



        // return Response<UserChangePasswordResponse>.Success(new UserChangePasswordResponse(), "password changed successfully");
    }
    public async Task<Response<UserResponse>> GetUser(int id)
    {
        var user = await _context.Users
            .AsNoTracking()
            .Include(u => u.TblRole)
            .Include(u => u.TblStudent)
            .Include(u => u.TblInstructor)
            .Include(u => u.TblAdmin)
            .FirstOrDefaultAsync(u => u.id == id);

        if (user is null)
        {
            return Response<UserResponse>.Error("user not found");
        }
        //v2    
        var response = new UserResponse
        {
            id = user.id,
            username = user.username,
            email = user.email,
            phone = user.phone,
            address = user.address,
            role_id = user.role_id,
            role = user.TblRole.role,
            dob = user.dob,
            profile_photo = user.profile_photo,
            created_at = user.created_at,
            updated_at = user.updated_at,

        };
        if (user.TblInstructor != null)
        {
            response.nrc = user.TblInstructor.nrc;
            response.edu_background = user.TblInstructor.edu_background;
        }

        return Response<UserResponse>.Success(response, "user retrieved successfully");

        //v1
        // if (user.TblStudent is not null)
        // {
        //     var response = new UserResponse
        //     {
        //         id = user.id,
        //         username = user.username,
        //         email = user.email,
        //         phone = user.phone,
        //         address = user.address,
        //         role_id = user.role_id,
        //         role = user.TblRole.role,
        //         dob = user.dob,
        //         profile_photo = user.profile_photo,

        //     };
        //     return Response<UserResponse>.Success(response, "user retrieved successfully");
        // }

        // if (user.TblInstructor is not null)
        // {
        //     var response = new UserResponse
        //     {
        //         id = user.id,
        //         username = user.username,
        //         email = user.email,
        //         phone = user.phone,
        //         address = user.address,
        //         role_id = user.role_id,
        //         role = user.TblRole.role,
        //         dob = user.dob,
        //         profile_photo = user.profile_photo,
        //         nrc = user.TblInstructor.nrc,
        //         edu_background = user.TblInstructor.edu_background,
        //     };
        //     return Response<UserResponse>.Success(response, "user retrieved successfully");
        // }

        // if (user.TblAdmin is not null)
        // {
        //     var response = new UserResponse
        //     {
        //         id = user.id,
        //         username = user.username,
        //         email = user.email,
        //         phone = user.phone,
        //         address = user.address,
        //         role_id = user.role_id,
        //         role = user.TblRole.role,
        //         dob = user.dob
        //     };

        //     return Response<UserResponse>.Success(response, "user retrieved successfully");

        // }

        // return Response<UserResponse>.Error("user not found");

    }

    public async Task<Response<List<UserResponse>>> GetUsers()
    {
        var users = await _context.Users
            .AsNoTracking()
            .Include(u => u.TblRole)
            .Include(u => u.TblStudent)
            .Include(u => u.TblInstructor)
            .Include(u => u.TblAdmin)
            .Select(u => new UserResponse
            {
                id = u.id,
                username = u.username,
                email = u.email,
                phone = u.phone,
                address = u.address,
                role_id = u.role_id,
                role = u.TblRole.role,
                dob = u.dob,
                profile_photo = u.profile_photo,
                created_at = u.created_at,
                updated_at = u.updated_at,

                // instructor
                nrc = u.TblInstructor != null ? u.TblInstructor.nrc : null,
                edu_background = u.TblInstructor != null ? u.TblInstructor.edu_background : null,

            })
            .ToListAsync();

        if (!users.Any())
        {
            return Response<List<UserResponse>>.Error("users not found");
        }

        return Response<List<UserResponse>>.Success(users, "users retrieved successfully");
    }
    public async Task<Response<bool>> DeleteUser(int id)
    {

        //v1
        // var user = await _context.Users.FindAsync(id);
        // if (user is null)
        // {
        //     return Response<bool>.Error("user not found");
        // }

        // user.isDeleted = true;
        // user.updated_at = DateTime.UtcNow;
        // await _context.SaveChangesAsync();

        //v2
        var user = await _context.Users
             .Include(u => u.TblStudent)
             .Include(u => u.TblInstructor)
             .Include(u => u.TblAdmin)
             .FirstOrDefaultAsync(u => u.id == id);

        if (user is null)
        {
            return Response<bool>.Error("user not found");
        }

        if (user.TblStudent is not null)
        {
            user.TblStudent.isDeleted = true;
            user.TblStudent.updated_at = DateTime.UtcNow;
        }

        if (user.TblInstructor is not null)
        {
            user.TblInstructor.isDeleted = true;
            user.TblInstructor.updated_at = DateTime.UtcNow;
        }

        if (user.TblAdmin is not null)
        {
            user.TblAdmin.isDeleted = true;
            user.TblAdmin.updated_at = DateTime.UtcNow;
        }

        user.isDeleted = true;
        user.updated_at = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return Response<bool>.Success(true, "user deleted successfully");
    }

    public async Task<Response<UserResponse>> GetInstructor(int id)
    {
        //v1
        // var instructor = await _context.Instructors.FindAsync(id);
        // if (instructor is null)
        //     return Response<UserResponse>.Error("instructor not found");
        // var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.id == instructor.user_id);
        // if (existingUser is null || existingUser.isDeleted)
        //     return Response<UserResponse>.Error("Instructor related user not found");

        // var existingInstructor = await _context.Users
        //     .Include(u => u.TblRole)
        //     .Where(u => u.id == id && u.TblRole.role == "Instructor" && !u.isDeleted)
        //     .Select(u => new UserResponse
        //     {
        //         id = instructor.id,
        //         username = u.username,
        //         email = u.email,
        //         phone = u.phone,
        //         address = u.address,
        //         role = u.TblRole.role,
        //     })
        //     .FirstOrDefaultAsync();

        // return existingInstructor == null
        //      ? Response<UserResponse>.Error("Instructor not found")
        //      : Response<UserResponse>.Success(existingInstructor, "Instructor retrieved successfully");
        //v2
        var instructor = await _context.Instructors
             .Include(i => i.TblUser)
             .ThenInclude(u => u.TblRole)
             .FirstOrDefaultAsync(i => i.id == id && !i.isDeleted);

        if (instructor is null)
        {
            return Response<UserResponse>.Error("Instructor not found");
        }

        var response = new UserResponse
        {
            id = instructor.id,
            username = instructor.TblUser.username,
            email = instructor.TblUser.email,
            phone = instructor.TblUser.phone,
            address = instructor.TblUser.address,
            role = instructor.TblUser.TblRole.role,
        };

        return Response<UserResponse>.Success(response, "Instructor retrieved successfully");
        //v3
        //     var instructor = await _context.Users
        //    .Include(u => u.TblRole)
        //    .Include(u => u.TblInstructor)
        //    .Where(u => u.TblInstructor.id == id && !u.isDeleted)
        //    .Select(u => new UserResponse
        //    {
        //        id = u.TblInstructor.id,
        //        username = u.username,
        //        email = u.email,
        //        phone = u.phone,
        //        address = u.address,
        //        role = u.TblRole.role
        //    })
        //    .FirstOrDefaultAsync();

        //     if (instructor is null)
        //     {
        //         return Response<UserResponse>.Error("Instructor not found");
        //     }

        //     return Response<UserResponse>.Success(instructor, "Instructor retrieved successfully");
    }

    public async Task<Response<List<UserResponse>>> GetInstructors()
    {
        var instructors = await _context.Instructors
            .AsNoTracking()
            .Include(u => u.TblUser)
            .ThenInclude(u => u.TblRole)
            .Where(i => !i.isDeleted)
            .Select(i => new UserResponse
            {
                id = i.id,
                username = i.TblUser.username,
                email = i.TblUser.email,
                phone = i.TblUser.phone,
                address = i.TblUser.address,
                role = i.TblUser.TblRole.role,
                nrc = i.nrc,
                dob = i.TblUser.dob,
                edu_background = i.edu_background,
                profile_photo = i.TblUser.profile_photo,
            })
            .ToListAsync();


        if (!instructors.Any())
        {
            return Response<List<UserResponse>>.Error("Instructor not found");
        }

        return Response<List<UserResponse>>.Success(instructors, "Instructors retrieved successfully");

        //v1
        // var response = await instructors.Select(i => new UserResponse
        // {
        //     id = i.id,
        //     username = i.TblUser.username,
        //     email = i.TblUser.email,
        //     phone = i.TblUser.phone,
        //     address = i.TblUser.address,
        //     role = i.TblUser.TblRole.role,
        //     nrc = i.nrc,
        //     dob = i.TblUser.dob,
        //     edu_background = i.edu_background,
        // }).ToListAsync();


    }

    public async Task<Response<UserResponse>> GetStudent(int id)
    {
        var student = await _context.Students
            .Include(s => s.TblUser)
            .ThenInclude(u => u.TblRole)
            .FirstOrDefaultAsync(s => s.id == id && !s.isDeleted);

        if (student is null)
        {
            return Response<UserResponse>.Error("Student not found");
        }

        var response = new UserResponse
        {
            id = student.id,
            username = student.TblUser.username,
            email = student.TblUser.email,
            phone = student.TblUser.phone,
            address = student.TblUser.address,
            role = student.TblUser.TblRole.role,
            profile_photo = student.TblUser.profile_photo,
        };

        return Response<UserResponse>.Success(response, "Student retrieved successfully");
    }

    public async Task<Response<List<UserResponse>>> GetStudents()
    {
        var students = await _context.Students
        .AsNoTracking()
        .Include(s => s.TblUser)
        .ThenInclude(u => u.TblRole)
        .Where(s => !s.isDeleted)
        .Select(s => new UserResponse
        {
            id = s.id,
            username = s.TblUser.username,
            email = s.TblUser.email,
            phone = s.TblUser.phone,
            address = s.TblUser.address,
            role = s.TblUser.TblRole.role,
            dob = s.TblUser.dob,
            profile_photo = s.TblUser.profile_photo,
            // nrc = i.nrc,
            // edu_background = i.edu_background,
        }).ToListAsync();

        if (!students.Any())
        {
            return Response<List<UserResponse>>.Error("Student not found");
        }

        return Response<List<UserResponse>>.Success(students, "Students retrieved successfully");

    }

    public async Task<Response<UserResponse>> PatchUser(int id, UserRequest user)
    {
        var existingUser = await _context.Users
                        .Include(u => u.TblRole)
                        .Include(u => u.TblStudent)
                        .Include(u => u.TblInstructor)
                        .Include(u => u.TblAdmin)
                        .FirstOrDefaultAsync(u => u.id == id && !u.isDeleted);

        if (existingUser is null)
        {
            return Response<UserResponse>.Error("User not found");
        }

        // update only given fields
        if (!string.IsNullOrEmpty(user.username)) existingUser.username = user.username;
        if (!string.IsNullOrEmpty(user.email)) existingUser.email = user.email;
        // might need to add confirm password field
        if (!string.IsNullOrEmpty(user.password))
        {
            // hash password
            if (BCrypt.Net.BCrypt.Verify(existingUser.password, existingUser.password))
            {
                existingUser.password = BCrypt.Net.BCrypt.HashPassword(user.password);
            }
            // existingUser.password = user.password;

        }
        if (!string.IsNullOrEmpty(user.phone)) existingUser.phone = user.phone;
        if (!string.IsNullOrEmpty(user.address)) existingUser.address = user.address;
        if (!string.IsNullOrEmpty(user.profile_photo)) existingUser.profile_photo = user.profile_photo;
        // role might not change after creation 
        // if (user.role_id != 0) existingUser.role_id = user.role_id;
        if (user.dob != default) existingUser.dob = user.dob;
        // instructor
        if (existingUser.TblInstructor is not null)
        {
            if (!string.IsNullOrEmpty(user.nrc)) existingUser.TblInstructor.nrc = user.nrc;
            if (!string.IsNullOrEmpty(user.edu_background)) existingUser.TblInstructor.edu_background = user.edu_background;
            existingUser.TblInstructor.updated_at = DateTime.UtcNow;
        }
        if (existingUser.TblStudent is not null)
        {
            existingUser.TblStudent.updated_at = DateTime.UtcNow;
        }
        if (existingUser.TblAdmin is not null)
        {
            existingUser.TblAdmin.updated_at = DateTime.UtcNow;
        }

        existingUser.updated_at = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        var response = new UserResponse
        {
            role_id = existingUser.role_id,
            id = existingUser.id,
            username = existingUser.username,
            email = existingUser.email,
            phone = existingUser.phone,
            address = existingUser.address,
            role = existingUser.TblRole.role,
            profile_photo = existingUser.profile_photo,
            dob = existingUser.dob,
            nrc = existingUser.TblInstructor?.nrc,
            edu_background = existingUser.TblInstructor?.edu_background,
            updated_at = existingUser.updated_at,
        };

        return Response<UserResponse>.Success(response, "User updated successfully");

    }

    public async Task<Response<UserResponse>> UpdateUser(int id, UserRequest user)
    {
        var existingUser = await _context.Users
            .Include(u => u.TblRole)
            .Include(u => u.TblStudent)
            .Include(u => u.TblInstructor)
            .Include(u => u.TblAdmin)
            .FirstOrDefaultAsync(u => u.id == id && !u.isDeleted);

        if (existingUser is null)
        {
            return Response<UserResponse>.Error("User not found");
        }

        existingUser.username = user.username;
        existingUser.email = user.email;
        existingUser.password = BCrypt.Net.BCrypt.HashPassword(user.password);
        existingUser.phone = user.phone;
        existingUser.address = user.address;
        existingUser.profile_photo = user.profile_photo;
        existingUser.role_id = user.role_id;
        existingUser.dob = user.dob;
        existingUser.updated_at = DateTime.UtcNow;
        if (existingUser.TblInstructor is not null)
        {
            existingUser.TblInstructor.nrc = user.nrc;
            existingUser.TblInstructor.edu_background = user.edu_background;
            existingUser.TblInstructor.updated_at = DateTime.UtcNow;
        }
        if (existingUser.TblStudent is not null)
        {
            existingUser.TblStudent.updated_at = DateTime.UtcNow;
        }
        if (existingUser.TblAdmin is not null)
        {
            existingUser.TblAdmin.updated_at = DateTime.UtcNow;
        }
        await _context.SaveChangesAsync();

        var response = new UserResponse
        {
            role_id = existingUser.role_id,
            id = existingUser.id,
            username = existingUser.username,
            email = existingUser.email,
            password = user.password,
            phone = existingUser.phone,
            address = existingUser.address,
            role = existingUser.TblRole.role,
            profile_photo = existingUser.profile_photo,
            dob = existingUser.dob,
            nrc = existingUser.TblInstructor?.nrc,
            edu_background = existingUser.TblInstructor?.edu_background,
        };

        return Response<UserResponse>.Success(response, "User updated successfully");
    }
}
