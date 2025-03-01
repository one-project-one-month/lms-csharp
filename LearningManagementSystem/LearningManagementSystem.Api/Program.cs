using FluentValidation.AspNetCore;
using FluentValidation;
using LearningManagementSystem.DataBase.Data;
using LearningManagementSystem.Domain.Services.CategoryServices;
using LearningManagementSystem.Domain.Services.LessonServices;
using LearningManagementSystem.Domain.Services.UsersServices;
using LearningManagementSystem.Domain.Validators;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks.Dataflow;
using LearningManagementSystem.Domain.Services.StudentsServieces;


var builder = WebApplication.CreateBuilder(args);

#region Read DB type from config and Default is MSSQL

// Get database type from configuration (or environment variable)


//Configuration API Version

builder.Services.AddApiVersioning(options =>
{
    options.ReportApiVersions = true;
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);
});

//Configuration API Version
builder.Services.AddVersionedApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});


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

//Add FluentValidation
builder.Services
    .AddFluentValidationAutoValidation()
    .AddFluentValidationClientsideAdapters();

builder.Services.AddValidatorsFromAssemblyContaining<LessonValidator>();




// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    var provider = builder.Services.BuildServiceProvider()
     .GetRequiredService<IApiVersionDescriptionProvider>();

    foreach (var description in provider.ApiVersionDescriptions)
    {
        options.SwaggerDoc(description.GroupName,
            new Microsoft.OpenApi.Models.OpenApiInfo
            {
                Title = $"Learning Management System API {description.ApiVersion}",
                Version = description.ApiVersion.ToString()
            });
    }
});


// I add some folders in here
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();

builder.Services.AddScoped<ILessonRepository, LessonRepository>();

builder.Services.AddScoped<IStudentRepository, StudentRepository>();


//builder.Services.AddTransient<IUserRepository, UserRepository>();
//builder.Services.AddSingleton<IUserRepository, UserRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        var provider = app.Services
            .GetRequiredService<IApiVersionDescriptionProvider>();
        foreach (var description in provider.ApiVersionDescriptions)
        {
            options.SwaggerEndpoint(
                $"/swagger/{description.GroupName}/swagger.json",
                $"Learning Management System API {description.GroupName.ToUpperInvariant()}");
        }
    }
    );
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
