using LearningManagementSystem.DataBase.Data;
using LearningManagementSystem.DataBase.Migrations;
using LearningManagementSystem.DataBase.Models.Users;
using LearningManagementSystem.Domain.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Net;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Users = LearningManagementSystem.DataBase.Models.Users.Users;

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
            .Where(x => // x.Role == "Instructor" && // need to check with roles
            x.DeleteFlag == false)
            .ToList();

        var userViewModels = model.Select(UsersViewModelsMapping).ToList();

        return userViewModels;
    }

    public List<UsersViewModels> GetStudents()
    {
        var model = _db.Users
            .AsNoTracking()
            .Where(x => //x.Role == "Student" && // need to check with roles
            x.DeleteFlag == false)
            .ToList();

        var userViewModels = model.Select(UsersViewModelsMapping).ToList();

        return userViewModels;
    }

    public List<UsersViewModels> GetInstructor(string id)
    {
        var model = _db.Users
            .AsNoTracking()
            .Where(x => //x.Role == "Instructor"&& // need to check with roles
            x.DeleteFlag == false && x.id.ToString() == id) 
            .ToList();

        var userViewModels = model.Select(UsersViewModelsMapping).ToList();

        return userViewModels;
    }

    public List<UsersViewModels> GetStudent(string id)
    {
        var model = _db.Users
            .AsNoTracking()
            .Where(x => x.DeleteFlag == false && x.id.ToString() == id) // need to check with roles
            .ToList();

        var userViewModels = model.Select(UsersViewModelsMapping).ToList();

        return userViewModels;
    }

    public UsersViewModels? UpdateUser(string id, UsersViewModels user)
    {
        var item = _db.Users
            .AsNoTracking()
            .FirstOrDefault(x => x.id.ToString() == id 
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
            .FirstOrDefault(x => x.id.ToString() == id
            && x.DeleteFlag == false);
        if (item is null) { return null; }

        item = UpdateUserDetails(id, user, item); // Updating Users info

        _db.Entry(item).State = EntityState.Modified;
        _db.SaveChanges();

        var model = UsersViewModelsMapping(item);
        return model;
    }

    public bool? DeleteUser(string id)
    {
        var item = _db.Users
            .AsNoTracking()
            .FirstOrDefault(x => x.id.ToString() == id 
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
            id = Guid.NewGuid(),
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

    private static UsersViewModels UsersViewModelsMapping(Users user)
    {
        return new UsersViewModels
        {

            //UserId = user.UserId,
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
            UpdatedDate = user.UpdatedDate
            //DeleteFlag = false
        };
    }

    private static Users UpdateUserDetails(string id, UsersViewModels user, Users item)
    {

        if (!string.IsNullOrEmpty(id))
        {
            item.id = Guid.Parse(id); // it`s guid
        }
        if (!string.IsNullOrEmpty(user.username))
        {
            item.username = user.username;
        }
        if (!string.IsNullOrEmpty(user.email))
        {
            item.email = user.email;
        }
        if (!string.IsNullOrEmpty(user.password))
        {
            item.password = user.password;
        }
        if (!string.IsNullOrEmpty(user.phone))
        {
            item.phone = user.phone;
        }
        if (!string.IsNullOrEmpty(user.dob.ToString()))
        {
            item.dob = user.dob;
        }
        if (!string.IsNullOrEmpty(user.address))
        {
            item.address = user.address;
        }
        if (!string.IsNullOrEmpty(user.profile_photo))
        {
            item.profile_photo = user.profile_photo;
        }
        if (!string.IsNullOrEmpty(user.role_id))
        {
            item.role_id = user.role_id;
        }
        if (!string.IsNullOrEmpty(user.is_available.ToString()))
        {
            item.is_available = user.is_available;
        }
        if (!string.IsNullOrEmpty(user.CreatedDate.ToString()))
        {
            item.CreatedDate = user.CreatedDate;
        }

        item.UpdatedDate = DateTime.Now; // Need to update everytime
        
        return item;
    }

}