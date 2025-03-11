using LearningManagementSystem.DataBase.Data;
using LearningManagementSystem.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LearningManagementSystem.DataBase.Models;
using Microsoft.EntityFrameworkCore;

namespace LearningManagementSystem.Domain.Services.InstructorServices
{
    public class InstructorRepository : IInstructorRepository
    {
        private readonly AppDbContext _db;

        public InstructorRepository(AppDbContext db)
        {
            _db = db;
        }

        //Test
        public InstructorViewModels CreateInstructor(InstructorViewModels reqModel)
        {
            var instrUserModel = UsersMapping(reqModel);

            var model = _db.Users.Add(instrUserModel);
            _db.SaveChanges();

            int userId = instrUserModel.id;

            var instrModel = InstructorMapping(reqModel, userId);
            var result = _db.Instructors.Add(instrModel);
            _db.SaveChanges();
            return reqModel;
        }


        public List<InstructorViewModels> GetInstructors()
        {

            // Retrieve the Role ID for 'Instructor'
            var roleId = _db.Roles
            .Where(x => x.role == "Instructor")
            .Select(x => x.id)
            .FirstOrDefault();

            var instViewModel = _db.Users
            .AsNoTracking()
            .Where(x => x.role_id == roleId && x.isDeleted == false)
            .Join(
            _db.Instructors,
            user => user.id,
            instructor => instructor.user_id,
            (user, instructor) => new InstructorViewModels
            {
                //Fields form Users table
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


                //Fields form Instructors table
                nrc = instructor.nrc,
                edu_background = instructor.edu_background
            })
            .ToList();

            return instViewModel;
        }

        public InstructorViewModels GetInstructorById(int id)
        {
            // Retrieve the Role ID for 'Instructor'
            var roleId = _db.Roles
            .Where(x => x.role == "Instructor")
            .Select(x => x.id)
            .FirstOrDefault();

            var instViewModel = _db.Users
            .AsNoTracking()
            .Where(x => x.role_id == roleId && x.id == id && x.isDeleted == false)
            .Join(
            _db.Instructors,
            user => user.id,
            instructor => instructor.user_id,
            (user, instructor) => new InstructorViewModels
            {
                //Fields form Users table
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


                //Fields form Instructors table
                nrc = instructor.nrc,
                edu_background = instructor.edu_background
            })
            .FirstOrDefault();

            return instViewModel!;

        }


        public bool DeleteInstructor(int id)
        {
            var instructorUser = _db.Users
                .AsNoTracking()
                .Where(x => x.id == id && x.isDeleted == false)
                .FirstOrDefault();
            if (instructorUser is null) return false;
            instructorUser.isDeleted = true;

            var instructor = _db.Instructors
                .AsNoTracking()
                .Where(x => x.user_id == id && x.isDeleted == false)
                .FirstOrDefault();
            instructor!.isDeleted = true;

            _db.Entry(instructorUser).State = EntityState.Modified;
            _db.Entry(instructor).State = EntityState.Modified;
            var result = _db.SaveChanges();
            return result > 0;

        }

        public InstructorViewModels UpdateInstructor(int id, InstructorViewModels instUser)
        {
            var item1 = _db.Users
                .AsNoTracking()
                .Where(s => s.id == id && s.isDeleted == false)
                .Include(s => s.TblRole)
                .FirstOrDefault();

            if (item1 is null) return null;

            item1 = UpdateUserDetail(id, instUser, item1);

            _db.Entry(item1).State = EntityState.Modified;
            _db.SaveChanges();

            var item2 = _db.Instructors
               .AsNoTracking()
               .Where(s => s.user_id == id && s.isDeleted == false)
               .FirstOrDefault();

            if (item2 is null) return null;

            item2 = UpdateInstructorDetail(instUser, item2);

            _db.Entry(item2).State = EntityState.Modified;
            _db.SaveChanges();

            return instUser;
        }


        private TblUsers UpdateUserDetail(int id, InstructorViewModels instUser, TblUsers item)
        {
            if (!string.IsNullOrEmpty(instUser.username.ToString()))
            {
                item.username = instUser.username;
            }
            if (instUser.role_id != 0)
            {
                item.role_id = instUser.role_id;
            }
            if (!string.IsNullOrEmpty(instUser.username))
            {
                item.username = instUser.username;
            }
            if (!string.IsNullOrEmpty(instUser.email))
            {
                item.email = instUser.email;
            }
            if (!string.IsNullOrEmpty(instUser.password))
            {
                item.password = instUser.password;
            }
            if (!string.IsNullOrEmpty(instUser.phone))
            {
                item.phone = instUser.phone;
            }
            if (!string.IsNullOrEmpty(instUser.dob.ToString()))
            {
                item.dob = instUser.dob;
            }
            if (!string.IsNullOrEmpty(instUser.address))
            {
                item.address = instUser.address;
            }
            if (!string.IsNullOrEmpty(instUser.profile_photo))
            {
                item.profile_photo = instUser.profile_photo;
            }
            if (!string.IsNullOrEmpty(instUser.is_available.ToString()))
            {
                item.is_available = instUser.is_available;
            }

            item.updated_at = DateTime.Now;

            return item;
        }

        private TblInstructors UpdateInstructorDetail(InstructorViewModels instUser, TblInstructors item)
        {
            if (!string.IsNullOrEmpty(instUser.nrc.ToString()))
            {
                item.nrc = instUser.nrc;
            }
            if (!string.IsNullOrEmpty(instUser.edu_background.ToString()))
            {
                item.edu_background = instUser.edu_background;
            }
            item.updated_at = DateTime.Now;
            return item;
        }


        private static TblUsers UsersMapping(InstructorViewModels reqModel)
        {
            return new TblUsers
            {
                //id = Guid.NewGuid(), 
                //id = 0,
                username = reqModel.username,
                email = reqModel.email,
                password = reqModel.password,
                phone = reqModel.phone,
                dob = reqModel.dob,
                address = reqModel.address,
                profile_photo = reqModel.profile_photo,
                role_id = reqModel.role_id,
                is_available = reqModel.is_available,
                created_at = reqModel.created_at,
                updated_at = reqModel.updated_at,
                isDeleted = false
            };
        }

        private static TblInstructors InstructorMapping(InstructorViewModels instructor, int userId)
        {
            return new TblInstructors
            {
                //id = Guid.NewGuid(), 
                user_id = userId,
                nrc = instructor.nrc,
                edu_background = instructor.edu_background,
                created_at = instructor.created_at,
                updated_at = instructor.updated_at
            };
        }
    }
}
