using Shared;

namespace Discount.Api.Features.Discounts.Create
{
    public record CreateDiscountCommand(string Code, float Rate, Guid UserId, DateTime Expired) : IRequestByServiceResult;
}