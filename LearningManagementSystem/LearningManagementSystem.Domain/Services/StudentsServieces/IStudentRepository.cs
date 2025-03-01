using LearningManagementSystem.DataBase.Models;
using LearningManagementSystem.Domain.Services.UsersServices;
using LearningManagementSystem.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.Domain.Services.StudentsServieces;

public interface IStudentRepository
{
    UsersViewModels CreateStudent(UsersViewModels user);

    List<UsersViewModels> GetStudents();

    UsersViewModels GetStudent(int id);

    bool DeleteStudent(int id);
}
