using System;
using FluentValidation;
using LearningManagementSystem.DataBase.Data;
using LearningManagementSystem.Domain.Services.UserServices.Requests;
using Org.BouncyCastle.Math.EC.Rfc7748;

namespace LearningManagementSystem.Domain.Services.UserServices.Validators;

public class UserValidator : AbstractValidator<UserRequest>
{
    private readonly AppDbContext _context;
    public UserValidator(AppDbContext context)
    {

        _context = context;

        RuleFor(x => x.username)
      .NotEmpty()
      .MinimumLength(3)
      .MaximumLength(50);
        //   .Must(BeUniqueUsername).WithMessage("Username does not exists");

        RuleFor(x => x.email)
            .NotEmpty()
            .EmailAddress();
        // .Must(BeUniqueEmail).WithMessage("Email does not exists");

        RuleFor(x => x.password)
            .NotEmpty()
        .MinimumLength(8)
        .Matches("[A-Z]").WithMessage("Password must contain uppercase letter")
        .Matches("[a-z]").WithMessage("Password must contain lowercase letter")
        .Matches("[0-9]").WithMessage("Password must contain number")
        .Matches("[^a-zA-Z0-9]").WithMessage("Password must contain special character");

        RuleFor(x => x.phone)
            .NotEmpty()
            // .Matches(@"^\+?[1-9][0-9]{7,14}$")
            .WithMessage("Invalid phone number format");

        RuleFor(x => x.dob)
            .NotEmpty()
            .Must(BeValidAge).WithMessage("User must be at least 13 years old");

        RuleFor(x => x.address)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.role_id)
            .NotEmpty();
        // .WithMessage("Role must be either 'student', 'instructor', or 'admin'");

        //instructor
        if (RuleFor(x => x.role_id).Equals(2))
        {
            RuleFor(x => x.nrc)
                .NotEmpty()
                .MaximumLength(50);

            RuleFor(x => x.edu_background)
                .NotEmpty()
                .MaximumLength(200);
        }
    }

    private bool BeUniqueUsername(string username)
    {
        return _context.Users.Any(u => u.username == username);
    }

    private bool BeUniqueEmail(string email)
    {
        return _context.Users.Any(u => u.email == email);
    }

    private bool BeValidAge(DateOnly dob)
    {
        var age = DateTime.Today.Year - dob.Year;
        return age >= 13;
    }
}
