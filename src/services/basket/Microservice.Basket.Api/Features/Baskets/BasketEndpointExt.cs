using Asp.Versioning.Builder;
using Microservice.Basket.Api.Features.Baskets.AddBasketItem;
using Microservice.Basket.Api.Features.Baskets.ApplyDiscountCoupon;
using Microservice.Basket.Api.Features.Baskets.DeleteBasketItem;
using Microservice.Basket.Api.Features.Baskets.GetBasket;
using Microservice.Basket.Api.Features.Baskets.RemoveDiscountCoupon;

namespace Microservice.Basket.Api.Features.Baskets;

public static class BasketEndpointExt
{
    public static void AddBasketGroupEndpointExt(this WebApplication app, ApiVersionSet apiVersionSet)
    {
        app.MapGroup("api/v{version:apiVersion}/baskets").WithTags("Baskets")
            .WithApiVersionSet(apiVersionSet)
            .AddBasketItemGroupItemEndpoint()
            .DeleteBasketItemGroupItemEndpoint()
            .GetBasketGroupItemEndpoint()
            .ApplyDiscountCouponGroupItemEndpoint()
            .RemoveDiscountCouponGroupItemEndpoint();
    }
}