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
            user_id = userId
        };

        var result = _db.Students.Add(studentModel);
        _db.SaveChanges();

        return student;
    }

    //public List<UsersViewModels> GetStudents()
    //{
    //    var students = _db.Students
    //        .AsNoTracking()
    //        .Include(s => s.User)
    //        .Where(x => x.DeleteFlag == false)
    //        .ToList();

    //    return List<students>;
    //}

    private static TblUsers UserMapping(UsersViewModels user)
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
            role_id = user.role_id,
            is_available = user.is_available,
            created_at = user.created_at,
            updated_at = user.updated_at,
            isDeleted = false
        };
    }
}
