using FluentValidation;

namespace MedicalEdu.Application.Courses.Create;

public sealed class CreateCourseCommandValidator : AbstractValidator<CreateCourseCommand>
{
    public CreateCourseCommandValidator()
    {
        RuleFor(x => x.InstructorId)
            .NotEmpty()
            .WithMessage("Instructor ID is required.");

        RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(200)
            .WithMessage("Title is required and must not exceed 200 characters.");

        RuleFor(x => x.Description)
            .NotEmpty()
            .MaximumLength(2000)
            .WithMessage("Description is required and must not exceed 2000 characters.");

        RuleFor(x => x.ShortDescription)
            .NotEmpty()
            .MaximumLength(500)
            .WithMessage("Short description is required and must not exceed 500 characters.");

        RuleFor(x => x.Price)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Price must be greater than or equal to 0.");

        RuleFor(x => x.Currency)
            .NotEmpty()
            .Length(3)
            .WithMessage("Currency must be a 3-character code (e.g., USD, EUR).");

        RuleFor(x => x.DurationMinutes)
            .GreaterThan(0)
            .LessThanOrEqualTo(1440) // 24 hours
            .WithMessage("Duration must be between 1 and 1440 minutes.");

        RuleFor(x => x.MaxStudents)
            .GreaterThan(0)
            .LessThanOrEqualTo(1000)
            .WithMessage("Maximum students must be between 1 and 1000.");

        RuleFor(x => x.Category)
            .NotEmpty()
            .MaximumLength(100)
            .WithMessage("Category is required and must not exceed 100 characters.");

        RuleFor(x => x.Tags)
            .NotNull()
            .WithMessage("Tags cannot be null.");

        RuleFor(x => x.CreatedBy)
            .NotEmpty()
            .WithMessage("Created by field is required.");

        When(x => !string.IsNullOrEmpty(x.ThumbnailUrl), () =>
        {
            RuleFor(x => x.ThumbnailUrl)
                .Must(BeValidUrl)
                .WithMessage("Thumbnail URL must be a valid URL.");
        });

        When(x => !string.IsNullOrEmpty(x.VideoIntroUrl), () =>
        {
            RuleFor(x => x.VideoIntroUrl)
                .Must(BeValidUrl)
                .WithMessage("Video intro URL must be a valid URL.");
        });
    }

    private static bool BeValidUrl(string? url)
    {
        return !string.IsNullOrEmpty(url) && Uri.TryCreate(url, UriKind.Absolute, out _);
    }
}