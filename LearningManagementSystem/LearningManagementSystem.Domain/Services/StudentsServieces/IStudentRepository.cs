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
    StudentsViewModels CreateStudent(StudentsViewModels user);

    List<StudentsViewModels> GetStudents();

    StudentsViewModels GetStudent(int id);

    bool DeleteStudent(int id);

    StudentsViewModels UpdateStudent(int id, StudentsViewModels student);
}
