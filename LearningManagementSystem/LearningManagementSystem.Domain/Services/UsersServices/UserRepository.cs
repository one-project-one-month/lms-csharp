using LearningManagementSystem.DataBase.Data;
using LearningManagementSystem.DataBase.Migrations;
using LearningManagementSystem.DataBase.Models;
using LearningManagementSystem.Domain.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users = LearningManagementSystem.DataBase.Models.Users;

namespace LearningManagementSystem.Domain.Services.UsersServices
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _db;

        public UserRepository(AppDbContext db)
        {
            _db = db;
        }

        public UsersViewModels CreateUser(UsersViewModels user)
        {
            user.UpdatedDate = null; // Need to amend and take out

            Users userModel = UsersMapping(user);

            _db.Users.Add(userModel);
            _db.SaveChanges();

            return user;
        }

        public List<UsersViewModels> GetInstructors()
        {
            var model = _db.Users
                .AsNoTracking()
                .Where(x => x.Role == "Instructor" && x.DeleteFlag == false)
                .ToList();

            var userViewModels = model.Select(UsersViewModelsMapping).ToList();

            return userViewModels;
        }

        public List<UsersViewModels> GetStudents()
        {
            var model = _db.Users
                .AsNoTracking()
                .Where(x => x.Role == "Student" && x.DeleteFlag == false)
                .ToList();

            var userViewModels = model.Select(UsersViewModelsMapping).ToList();

            return userViewModels;
        }

        public List<UsersViewModels> GetInstructor(string id)
        {
            var model = _db.Users
                .AsNoTracking()
                .Where(x => x.Role == "Instructor"
                && x.DeleteFlag == false && x.UserId.ToString() == id) //I have proper reasons why I didn`t use Firstordefault.
                .ToList();

            var userViewModels = model.Select(UsersViewModelsMapping).ToList();

            return userViewModels;
        }

        public List<UsersViewModels> GetStudent(string id)
        {
            var model = _db.Users
                .AsNoTracking()
                .Where(x => x.Role == "Student"
                && x.DeleteFlag == false && x.UserId.ToString() == id) //I have proper reasons why I didn`t use Firstordefault.
                .ToList();

            var userViewModels = model.Select(UsersViewModelsMapping).ToList();

            return userViewModels;
        }

        // Can use for instructor and students
        private static Users UsersMapping(UsersViewModels user)
        {
            return new Users
            {
                UserId = Guid.NewGuid(),
                Name = user.Name,
                Email = user.Email,
                PasswordHash = user.PasswordHash,
                UserName = user.UserName,
                Phone = user.Phone,
                Address = user.Address,
                Role = user.Role,
                CreatedDate = user.CreatedDate,
                UpdatedDate = user.UpdatedDate,
                DeleteFlag = false
            };
        }

        private static UsersViewModels UsersViewModelsMapping(Users user)
        {
            return new UsersViewModels
            {
                //UserId = user.UserId,
                Name = user.Name,
                Email = user.Email,
                PasswordHash = user.PasswordHash,
                UserName = user.UserName,
                Phone = user.Phone,
                Address = user.Address,
                Role = user.Role,
                CreatedDate = user.CreatedDate,
                UpdatedDate = user.UpdatedDate
                //DeleteFlag = false
            };
        }
    }

}
