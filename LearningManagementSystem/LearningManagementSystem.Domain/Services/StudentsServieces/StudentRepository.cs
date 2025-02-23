using LearningManagementSystem.DataBase.Data;
using LearningManagementSystem.DataBase.Models.Students;
using LearningManagementSystem.DataBase.Models.Users;
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

        var studentModel = new Students
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

    private static Users UserMapping(UsersViewModels user)
    {
        return new Users
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
            CreatedDate = user.CreatedDate,
            UpdatedDate = user.UpdatedDate,
            DeleteFlag = false
        };
    }
}
