using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using LearningManagementSystem.Domain.Services.AuthServices.Requests;

namespace LearningManagementSystem.Domain.Services.AuthServices.Validators
{
    public class RefreshTokenValidator : AbstractValidator<RefreshTokenRequest>
    {
        private readonly AppDbContext _db;
        public RefreshTokenValidator(AppDbContext db)
        {
            _db = db;

            RuleFor(x => x.token)
                .NotEmpty()
                .WithMessage("Token is required");

            RuleFor(x => x.id)
                .NotEmpty()
                .WithMessage("Id is required")
                .Must(BeValidId)
                .WithMessage("User doesn't exist");
        }

        private bool BeValidId(int? id)
        {
            return _db.Users.Any(x => x.id == id);

        }
    }
}