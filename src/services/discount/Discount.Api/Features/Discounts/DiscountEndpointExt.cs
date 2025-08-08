using Asp.Versioning.Builder;
using Discount.Api.Features.Discounts.Create;
using Discount.Api.Features.Discounts.GetDiscountByCode;

namespace Discount.Api.Features.Discounts;

public static class DiscountEndpointExt
{
    public static void AddDiscountGroupEndpointExt(this WebApplication app, ApiVersionSet apiVersionSet)
    {
        app.MapGroup("api/v{version:apiVersion}/discounts").WithTags("Discounts")
            .WithApiVersionSet(apiVersionSet)
            .CreateDiscountGroupItemEndpoint()
            .GetDiscountByCodeGroupItemEndpoint()
            .RequireAuthorization();
    }
}