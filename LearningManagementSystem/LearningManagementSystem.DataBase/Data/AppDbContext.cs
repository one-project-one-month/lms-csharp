using LearningManagementSystem.DataBase.Models.Users;
using LearningManagementSystem.DataBase.Models.Roles;
using LearningManagementSystem.DataBase.Models.Students;
using LearningManagementSystem.DataBase.Models.Social_Links;
using LearningManagementSystem.DataBase.Models.Lessons;
using LearningManagementSystem.DataBase.Models.Instructors;
using LearningManagementSystem.DataBase.Models.Enrollments;
using LearningManagementSystem.DataBase.Models.Courses;
using LearningManagementSystem.DataBase.Models.Category;
using LearningManagementSystem.DataBase.Models.Admins;
using Microsoft.EntityFrameworkCore;

namespace LearningManagementSystem.DataBase.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions options) : base(options)
    {

    }
    public DbSet<Users> Users { get; set; }
    public DbSet<Students> Students { get; set; }
    public DbSet<Admins> Admins { get; set; }
    public DbSet<Instructors> Instructors { get; set; }
    public DbSet<Category> Category { get; set; }
    public DbSet<Courses> Courses { get; set; }
    public DbSet<Enrollments> Enrollments { get; set; }
    public DbSet<Lessons> Lessons { get; set; }
    public DbSet<Social_Links> Social_Links { get; set; }
    public DbSet<Roles> Roles { get; set; }
}
