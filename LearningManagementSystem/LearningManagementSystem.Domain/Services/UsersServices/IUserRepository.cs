using LearningManagementSystem.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.Domain.Services.UsersServices;

public interface IUserRepository
{
    UsersViewModels CreateUser(UsersViewModels user);

    List<UsersViewModels> GetStudents();

    List<UsersViewModels> GetInstructors();

    List<UsersViewModels> GetStudent(int id);

    List<UsersViewModels> GetInstructor(int id);
    
    UsersViewModels UpdateUser(int id, UsersViewModels user);

    UsersViewModels PatchUser(int id, UsersViewModels user);

    public bool? DeleteUser(int id);
}
