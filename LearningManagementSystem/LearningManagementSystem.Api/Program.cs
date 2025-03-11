using FluentValidation.AspNetCore;
using LearningManagementSystem.Domain.Services.AuthServices;
using LearningManagementSystem.Domain.Services.AuthServices.Requests;
using LearningManagementSystem.Domain.Services.AuthServices.Validators;
using LearningManagementSystem.Domain.Validators;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;



var builder = WebApplication.CreateBuilder(args);

//JWT Services
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

builder.Services.AddAuthorization(options =>
{
    // options.AddPolicy("RequireAdminRole", policy => policy.RequireRole("Admin"));
    // options.AddPolicy("RequireStudentRole", policy => policy.RequireRole("Student,Instructor"));
    // options.AddPolicy("RequireInstructorRole", policy => policy.RequireRole("Instructor,Student"));

    options.AddPolicy("RequireAdminRole", policy => policy.RequireRole("Admin"));
    options.AddPolicy("RequireStudentRole", policy => policy.RequireRole(new[] { "Student", "Instructor" }));
    options.AddPolicy("RequireInstructorRole", policy => policy.RequireRole(new[] { "Instructor", "Student" }));
    options.AddPolicy("RequireWorkerRole", policy => policy.RequireRole(new[] { "Admin", "Instructor" }));


});

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
builder.Services.AddControllers().AddNewtonsoftJson();



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


builder.Services.AddControllers().AddFluentValidation(fv =>
{
    fv.RegisterValidatorsFromAssemblyContaining<RegistrationValidator>();
    fv.RegisterValidatorsFromAssemblyContaining<LoginRequestValidator>();
});

builder.Services.AddScoped<IValidator<UsersViewModels>, RegistrationValidator>();
builder.Services.AddScoped<IValidator<LoginRequest>, LoginRequestValidator>();

builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<IResponseService, ResponseService>();

builder.Services.AddScoped<UserServices>();
builder.Services.AddScoped<IUserServices, UserServices>();


//builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();

builder.Services.AddScoped<ILessonRepository, LessonRepository>();

builder.Services.AddScoped<IStudentRepository, StudentRepository>();

builder.Services.AddScoped<IInstructorRepository, InstructorRepository>();

builder.Services.AddScoped<ICourseService, CourseService>();

builder.Services.AddScoped<ISocial_linksRepository, Social_linksRepository>();

builder.Services.AddScoped<IEnrollmentRepository, EnrollmentRepository>();

builder.Services.AddSwaggerGen(options =>
{
    //options.SwaggerDoc("v1", new OpenApiInfo { Title = "LMS API", Version = "v1" });
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header,
            },
            new List<string>()
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "LMS API V1");
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
app.UseRouting();

//app.UseAuthorization();


// These two MUST be after UseRouting and before MapControllers
app.UseAuthentication(); // Enable authentication middleware
app.UseAuthorization(); // Enable authorization middleware


app.MapControllers();

app.Run();
