using FluentValidation;

namespace Catalog.Api.Features.Courses.Create
{
    public class CreateCourseCommandValidator : AbstractValidator<CreateCourseCommand>
    {
        public CreateCourseCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("{PropertyName} cannot be empty")
                .Length(4, 100).WithMessage("{PropertyName} must be between 4 and 100 characters");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("{PropertyName} cannot be empty")
                .Length(10, 1000).WithMessage("{PropertyName} must be between 10 and 1000 characters");

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("{PropertyName} must be greater than 0");

            RuleFor(x => x.CategoryId)
                .NotEmpty().WithMessage("{PropertyName} cannot be empty")
                .Must(categoryId => categoryId != Guid.Empty)
                .WithMessage("{PropertyName} must be a valid GUID");
        }
    }
}



