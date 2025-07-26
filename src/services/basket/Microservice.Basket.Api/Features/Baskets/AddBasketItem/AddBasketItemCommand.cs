namespace Microservice.Basket.Api.Features.Baskets.AddBasketItem;

public record AddBasketItemCommand(Guid Id, string CourseName, decimal CoursePrice, string? ImageUrl);
