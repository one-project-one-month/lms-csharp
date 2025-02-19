using Microsoft.EntityFrameworkCore;
using LearningManagementSystem.DataBase.Models;

namespace LearningManagementSystem.DataBase.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions options) : base(options)
    {

    }
    public DbSet<TblUsers> Users { get; set; }
    public DbSet<TblStudents> Students { get; set; }
    public DbSet<TblAdmins> Admins { get; set; }
    public DbSet<TblInstructors> Instructors { get; set; }
    public DbSet<TblCategory> Category { get; set; }
    public DbSet<TblCourses> Courses { get; set; }
    public DbSet<TblEnrollments> Enrollments { get; set; }
    public DbSet<TblLessons> Lessons { get; set; }
    public DbSet<TblSocial_Links> Social_Links { get; set; }
    public DbSet<TblRoles> Roles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        #region User

        // One-to-Many: Role and Users
        modelBuilder.Entity<TblUsers>()
            .HasOne(u => u.TblRole)
            .WithMany(r => r.TblUsers)
            .HasForeignKey(u => u.role_id)
            .OnDelete(DeleteBehavior.Restrict);

        // One-to-One: User and Student
        modelBuilder.Entity<TblStudents>()
            .HasOne(s => s.TblUser)
            .WithOne(u => u.TblStudent)
            .HasForeignKey<TblStudents>(s => s.user_id)
            .OnDelete(DeleteBehavior.Cascade);

        // One-to-One: User and Admin
        modelBuilder.Entity<TblAdmins>()
            .HasOne(a => a.TblUser)
            .WithOne(u => u.TblAdmin)
            .HasForeignKey<TblAdmins>(a => a.user_id)
            .OnDelete(DeleteBehavior.Cascade);

        // One-to-One: User and Instructor
        modelBuilder.Entity<TblInstructors>()
            .HasOne(i => i.TblUser)
            .WithOne(u => u.TblInstructor)
            .HasForeignKey<TblInstructors>(i => i.user_id)
            .OnDelete(DeleteBehavior.Cascade);

        // ***** One-to-Many: User and Enrollments *****
        modelBuilder.Entity<TblEnrollments>()
            .HasOne(l => l.TblUser)
            .WithMany(c => c.TblEnrollments)
            .HasForeignKey(l => l.user_id)
            .OnDelete(DeleteBehavior.Restrict);

        #endregion

        #region Courses

        // One-to-One: Course and Social_Link
        modelBuilder.Entity<TblSocial_Links>()
            .HasOne(i => i.TblCourse)
            .WithOne(u => u.TblSocial_Link)
            .HasForeignKey<TblSocial_Links>(i => i.course_id)
            .OnDelete(DeleteBehavior.Cascade);

        // One-to-Many: Course and Lessons 
        modelBuilder.Entity<TblLessons>()
            .HasOne(l => l.TblCourse)
            .WithMany(c => c.TblLessons)
            .HasForeignKey(l => l.course_id)
            .OnDelete(DeleteBehavior.Cascade);

        // One-to-Many: Instructor and Courses 
        modelBuilder.Entity<TblCourses>()
            .HasOne(c => c.TblInstructor)
            .WithMany(i => i.TblCourses)
            .HasForeignKey(c => c.instructor_id)
            .OnDelete(DeleteBehavior.Cascade);

        // ***** Many-to-Many: Courses and Enrollments *****
        // (Many users can enroll in many courses)
        modelBuilder.Entity<TblEnrollments>()
            .HasOne(e => e.TblCourse)
            .WithMany(u => u.TblEnrollments)
            .HasForeignKey(e => e.course_id)
            .OnDelete(DeleteBehavior.Restrict);

        // One-to-Many: Category and Courses 
        modelBuilder.Entity<TblCourses>()
            .HasOne(c => c.TblCategory)
            .WithMany(i => i.TblCourses)
            .HasForeignKey(c => c.category_id)
            .OnDelete(DeleteBehavior.Cascade);

        #endregion

        base.OnModelCreating(modelBuilder);
    }

}


