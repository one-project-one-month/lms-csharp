using LearningManagementSystem.DataBase.Data;
using LearningManagementSystem.DataBase.Models;
using LearningManagementSystem.Domain.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Pomelo.EntityFrameworkCore.MySql.Metadata.Internal;

namespace LearningManagementSystem.Domain.Services.StudentsServieces;

public class StudentRepository : IStudentRepository
{
    private readonly AppDbContext _db;

    public StudentRepository(AppDbContext db)
    {
        _db = db;
    }

    public StudentsViewModels CreateStudent(StudentsViewModels student)
    {
        var filepath = UploadImage(student.profile_photo_file);
        student.profile_photo = filepath;

        var userModel = UserMapping(student);

        var model = _db.Users.Add(userModel);
        _db.SaveChanges();

        int userId = userModel.id;

        var studentModel = new TblStudents
        {
            user_id = userId,
            created_at = DateTime.Now
        };

        var result = _db.Students.Add(studentModel);
        _db.SaveChanges();

        return student;
    }

    public List<StudentsViewModels> GetStudents()
    {
        var roleId = _db.Roles
            .Where(x => x.role == "student")
            .Select(x => x.id)
            .FirstOrDefault();

        var userModel = _db.Users
            .AsNoTracking()
            .Where(x => x.role_id == roleId && x.isDeleted == false)
            .ToList();

        var userViewModels = userModel.Select(UsersViewModelsMapping).ToList();

        return userViewModels;
    }

    public StudentsViewModels GetStudent(int id)
    {
        var roleId = _db.Roles
            .Where(x => x.role == "student")
            .Select(x => x.id)
            .FirstOrDefault();

        var userModel = _db.Users
            .AsNoTracking()
            .Where(x => x.isDeleted == false && x.id == id && x.role_id == roleId)
            .FirstOrDefault();

        var userViewModels = UsersViewModelsMapping(userModel!);
        return userViewModels;
    }

    public StudentsViewModels UpdateStudent(int id, StudentsViewModels student)
    {
        var item = _db.Users
            .AsNoTracking()
            .Where(s => s.id == id && s.isDeleted == false)
            .Include(s => s.TblRole)
            .FirstOrDefault();

        if (item is null) return null;

        item = UpdateUserDetail(id, student, item);

        _db.Entry(item).State = EntityState.Modified;
        _db.SaveChanges();

        var userViewModels = UsersViewModelsMapping(item);
        return userViewModels;
    }

    public bool DeleteStudent(int id)
    {
        var studentUser = _db.Users
            .AsNoTracking()
            .Where(x => x.id == id && x.isDeleted == false)
            .FirstOrDefault();
        if(studentUser is null) return false;

        studentUser.isDeleted = true;

        var student = _db.Students
            .AsNoTracking()
            .Where(s => s.user_id == id && s.isDeleted == false)
            .FirstOrDefault();

        student!.isDeleted = true;

        _db.Entry(studentUser).State = EntityState.Modified;
        _db.Entry(student).State = EntityState.Modified;
        var result = _db.SaveChanges();
        return result > 0;
    }

    private TblUsers UpdateUserDetail(int id, StudentsViewModels user, TblUsers item)
    {
        if (!string.IsNullOrEmpty(user.username.ToString()))
        {
            item.username = user.username;
        }
        if (user.role_id != 0)
        {
            item.role_id = user.role_id;
        }
        if (!string.IsNullOrEmpty(user.username))
        {
            item.username = user.username;
        }
        if (!string.IsNullOrEmpty(user.email))
        {
            item.email = user.email;
        }
        if (!string.IsNullOrEmpty(user.password))
        {
            item.password = user.password;
        }
        if (!string.IsNullOrEmpty(user.phone))
        {
            item.phone = user.phone;
        }
        if (!string.IsNullOrEmpty(user.dob.ToString()))
        {
            item.dob = user.dob;
        }
        if (!string.IsNullOrEmpty(user.address))
        {
            item.address = user.address;
        }
        if (!string.IsNullOrEmpty(user.profile_photo))
        {
            item.profile_photo = user.profile_photo;
        }
        if (!string.IsNullOrEmpty(user.is_available.ToString()))
        {
            item.is_available = user.is_available;
        }

        item.updated_at = DateTime.Now;

        return item;
    }

    private TblUsers UserMapping(StudentsViewModels user)
    {
        return new TblUsers
        {
            username = user.username,
            email = user.email,
            password = user.password,
            phone = user.phone,
            dob = user.dob,
            address = user.address,
            profile_photo = user.profile_photo,
            role_id = 4,
            is_available = user.is_available,
            created_at = user.created_at,
            updated_at = user.updated_at,
            isDeleted = false
        };
    }

    private StudentsViewModels UsersViewModelsMapping(TblUsers user)
    {
        return new StudentsViewModels()
        {
            username = user.username,
            email = user.email,
            password = user.password,
            phone = user.phone,
            dob = user.dob,
            address = user.address,
            profile_photo = user.profile_photo,
            role_id = user.role_id,
            is_available = user.is_available,
            created_at = user.created_at,
            updated_at = user.updated_at
        };
    }

    public string UploadImage(IFormFile file)
    {
        var filePath = "";
        if (file is not null && file.Length > 0)
        {
            filePath = Path.Combine(Directory.GetCurrentDirectory(), "UploadImages", file.FileName);

            var uploadDirectory = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(uploadDirectory))
            {
                Directory.CreateDirectory(uploadDirectory!);
            }

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(stream);
            }
        }
        return filePath;
    }
}
