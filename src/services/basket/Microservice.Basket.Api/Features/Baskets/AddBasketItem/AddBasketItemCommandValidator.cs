using FluentValidation;

namespace Microservice.Basket.Api.Features.Baskets.AddBasketItem;

public class AddBasketItemCommandValidator : AbstractValidator<AddBasketItemCommand>
{
    public AddBasketItemCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Id is required.");
        RuleFor(x => x.CourseName)
            .NotEmpty()
            .WithMessage("Course name is required.")
            .MaximumLength(100)
            .WithMessage("Course name must not exceed 100 characters.");
        RuleFor(x => x.CoursePrice)
            .GreaterThan(0)
            .WithMessage("Course price must be greater than zero.");
    }
}