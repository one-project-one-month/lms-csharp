using LearningManagementSystem.Domain.Services.AuthServices.Requests;

namespace LearningManagementSystem.Domain.Services.AuthServices.Validators;

public class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .WithMessage("A Valid Email Address is Required");
        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("Password is required");
    }

}
