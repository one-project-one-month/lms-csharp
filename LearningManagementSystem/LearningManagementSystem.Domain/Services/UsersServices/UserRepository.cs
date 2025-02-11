using LearningManagementSystem.DataBase.Data;
using LearningManagementSystem.DataBase.Migrations;
using LearningManagementSystem.DataBase.Models;
using LearningManagementSystem.Domain.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Users = LearningManagementSystem.DataBase.Models.Users;

namespace LearningManagementSystem.Domain.Services.UsersServices;

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

    public UsersViewModels? UpdateUser(string id, UsersViewModels user)
    {
        var item = _db.Users
            .AsNoTracking()
            .FirstOrDefault(x => x.UserId.ToString() == id 
            && x.DeleteFlag == false);
        if (item is null) { return null; }

        item = UpdateUserDetails(id, user, item); // Updating Users info

        _db.Entry(item).State = EntityState.Modified;
        _db.SaveChanges();

        var model = UsersViewModelsMapping(item);

        return model;
    }

    public UsersViewModels? PatchUser(string id, UsersViewModels user)
    {
        var item = _db.Users
            .AsNoTracking()
            .FirstOrDefault(x => x.UserId.ToString() == id 
            && x.DeleteFlag == false);
        if (item is null) { return null; }

        item = UpdateUserDetails(id, user, item); // Updating Users info

        _db.Entry(item).State = EntityState.Modified;
        _db.SaveChanges();

        var model = UsersViewModelsMapping(item);
        return user;
    }


    public bool? DeleteUser(string id)
    {
        var item = _db.Users
            .AsNoTracking()
            .FirstOrDefault(x => x.UserId.ToString() == id 
            && x.DeleteFlag == false);
        if (item is null)
        {
            return null;
        }

        item.DeleteFlag = true;

        _db.Entry(item).State = EntityState.Modified;
        var result = _db.SaveChanges();

        return result > 0;
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

    private static Users UpdateUserDetails(string id, UsersViewModels user, Users item)
    {

        if (!string.IsNullOrEmpty(id))
        {
            item.UserId = Guid.Parse(id); // it`s guid
        }
        if (!string.IsNullOrEmpty(user.UserName))
        {
            item.UserName = user.UserName;
        }
        if (!string.IsNullOrEmpty(user.Name))
        {
            item.Name = user.Name;
        }
        if (!string.IsNullOrEmpty(user.Email))
        {
            item.Email = user.Email;
        }
        if (!string.IsNullOrEmpty(user.PasswordHash))
        {
            item.PasswordHash = user.PasswordHash;
        }
        if (!string.IsNullOrEmpty(user.Phone))
        {
            item.Phone = user.Phone;
        }
        if (!string.IsNullOrEmpty(user.Address))
        {
            item.Address = user.Address;
        }
        if (!string.IsNullOrEmpty(user.Role))
        {
            item.Role = user.Role;
        }
        if (!string.IsNullOrEmpty(user.CreatedDate.ToString()))
        {
            item.CreatedDate = user.CreatedDate;
        }

        item.UpdatedDate = DateTime.Now; // Need to update everytime
        
        return item;
    }

}