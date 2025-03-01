using FluentValidation;
using LearningManagementSystem.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.Domain.Validators
{
    public class LessonValidator : AbstractValidator<LessonViewModel>
    {
        public LessonValidator()
        {
            RuleFor(x => x.course_id)
               .GreaterThan(0).WithMessage("Course ID must be greater than zero.");

            RuleFor(x => x.title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(255).WithMessage("Title cannot exceed 255 characters.");

            RuleFor(x => x.videoUrl)
                .Matches(@"^(http|https):\/\/.*$").When(x => !string.IsNullOrEmpty(x.videoUrl))
                .WithMessage("Video URL must be a valid URL.");

            RuleFor(x => x.lessonDetail)
                .NotEmpty().WithMessage("Lesson details are required.");

            RuleFor(x => x.is_available)
                .NotNull().WithMessage("Availability status is required.");
        }
    }
}
