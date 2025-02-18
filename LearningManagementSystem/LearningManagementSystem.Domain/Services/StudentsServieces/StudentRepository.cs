using LearningManagementSystem.DataBase.Data;
using LearningManagementSystem.DataBase.Models.Users;
using LearningManagementSystem.Domain.ViewModels;

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
        var StudentModel = StudentMapping(student);
        var model = _db.Users.Add(StudentModel);
        _db.SaveChanges();

        return student;
    }

    private static Users StudentMapping(UsersViewModels user)
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
