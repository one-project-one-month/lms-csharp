using LearningManagementSystem.DataBase.Models;
using Microsoft.EntityFrameworkCore;

namespace LearningManagementSystem.DataBase.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions options) : base(options)
    {

    }

    public DbSet<Users> Users { get; set; }
}
