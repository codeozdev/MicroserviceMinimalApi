using Asp.Versioning.Builder;
using Discount.Api.Features.Discounts.Create;

namespace Discount.Api.Features.Discounts
{
    public static class DiscountEndpointExt
    {
        public static void AddDiscountGroupEndpointExt(this WebApplication app, ApiVersionSet apiVersionSet)
        {
            app.MapGroup("api/v{version:apiVersion}/discounts").WithTags("Discounts")
                .WithApiVersionSet(apiVersionSet)
                .CreateDiscountGroupItemEndpoint();
        }
    }
}