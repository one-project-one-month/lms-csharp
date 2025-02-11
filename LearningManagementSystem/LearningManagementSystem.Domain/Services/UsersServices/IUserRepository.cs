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

    List<UsersViewModels> GetStudent(string id);

    List<UsersViewModels> GetInstructor(string id);
    
    UsersViewModels UpdateUser(string id, UsersViewModels user);

    UsersViewModels PatchUser(string id, UsersViewModels user);

    public bool? DeleteUser(string id);
}
