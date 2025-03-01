using LearningManagementSystem.DataBase.Data;
using LearningManagementSystem.DataBase.Models;
using LearningManagementSystem.Domain.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace LearningManagementSystem.Domain.Services.StudentsServieces;

public class StudentRepository : IStudentRepository
{
    private readonly AppDbContext _db;

    public StudentRepository(AppDbContext db)
    {
        _db = db;
    }

    public UsersViewModels CreateStudent(UsersViewModels student)
    {
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

    public List<UsersViewModels> GetStudents()
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

    public UsersViewModels GetStudent(int id)
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

    private TblUsers UserMapping(UsersViewModels user)
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

    private UsersViewModels UsersViewModelsMapping(TblUsers user)
    {
        return new UsersViewModels()
        {
            username = user.username,
            email = user.email,
            //password = user.password,
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
}
