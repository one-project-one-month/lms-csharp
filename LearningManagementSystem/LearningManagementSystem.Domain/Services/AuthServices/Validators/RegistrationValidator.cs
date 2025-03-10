using FluentValidation;
using LearningManagementSystem.DataBase.Data;
using LearningManagementSystem.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.Domain.Services.AuthServices.Validators
{
    public class RegistrationValidator : AbstractValidator<UsersViewModels>
    {
        private readonly AppDbContext _context;
        public RegistrationValidator(AppDbContext context)
        {
            _context = context;

            RuleFor(x => x.username)
          .NotEmpty()
          .MinimumLength(3)
          .MaximumLength(50)
          .Must(BeUniqueUsername).WithMessage("Username already exists");

            RuleFor(x => x.email)
                .NotEmpty()
                .EmailAddress()
                .Must(BeUniqueEmail).WithMessage("Email already exists");

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

            RuleFor(x => x.nrc)
                .NotEmpty()
                .Matches(@"^\d{1,2}/\w+\(N\w+\)\d{6}$")
                .WithMessage("Invalid NRC format");

            // RuleFor(x => x.profile_photo);

        }

        private bool BeUniqueUsername(string username)
        {
            return !_context.Users.Any(u => u.username == username);
        }

        private bool BeUniqueEmail(string email)
        {
            return !_context.Users.Any(u => u.email == email);
        }

        private bool BeValidAge(DateOnly dob)
        {
            var age = DateTime.Today.Year - dob.Year;
            return age >= 13;
        }
    }

}
