using LearningManagementSystem.DataBase.Data;
using LearningManagementSystem.Domain.Services.CategoryServices;
using LearningManagementSystem.Domain.Services.UsersServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks.Dataflow;


var builder = WebApplication.CreateBuilder(args);

#region Read DB type from config and Default is MSSQL

// Get database type from configuration (or environment variable)


//builder.Services.AddDbContext<AppDbContext>(options =>
//        options.UseSqlServer(builder.Configuration
//        .GetConnectionString("MSSQLConnection")));

//builder.Services.AddDbContext<AppDbContext>(options =>
//        options.UseMySql(builder.Configuration
//        .GetConnectionString("MySQLConnection"),
//        ServerVersion.AutoDetect(builder.Configuration
//        .GetConnectionString("MySQLConnection"))));

var databaseType = builder.Configuration["DatabaseType"] ?? "MSSQL"; // Default to MSSQL

if (databaseType == "MSSQL")
{
    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("MSSQLConnection")));
}
else
if (databaseType == "MySQL")
{
    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseMySql(builder.Configuration.GetConnectionString("MySQLConnection"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("MySQLConnection"))));
    // Please don`t delete server.AutoDetect
}

#endregion


//Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// I add some folders in here
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddScoped<CategoryRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();


//builder.Services.AddTransient<IUserRepository, UserRepository>();
//builder.Services.AddSingleton<IUserRepository, UserRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
