using MediatR;
using Microservice.Basket.Api.Dto;
using Shared;
using System.Net;
using System.Text.Json;

namespace Microservice.Basket.Api.Features.Baskets.ApplyDiscountCoupon;

/// <summary>
///  Kullanıcının sepetindeki ürüne kupon ile indirim oranı uygular.
/// </summary>
public class ApplyDiscountCouponCommandHandler(BasketService basketService) : IRequestHandler<ApplyDiscountCouponCommand, ServiceResult>
{
    public async Task<ServiceResult> Handle(ApplyDiscountCouponCommand request, CancellationToken cancellationToken)
    {
        var basketAsJson = await basketService.GetBasketFromCache(cancellationToken);

        if (string.IsNullOrEmpty(basketAsJson)) // Sepet bulunamazsa 404 hatası döndürülür
        {
            return ServiceResult<BasketDto>.Error("Basket not found", HttpStatusCode.NotFound);
        }

        var basket = JsonSerializer.Deserialize<Data.Basket>(basketAsJson);  // JSON string, Basket nesnesine deserialize edilir

        if (!basket.Items.Any()) // Sepet boşsa, yani içinde ürün yoksa hata döndürülür
        {
            return ServiceResult<BasketDto>.Error("Basket item not found", HttpStatusCode.NotFound);
        }

        basket.ApplyNewDiscount(request.Coupon, request.DiscountRate);                            // Sepete kupon ve indirim oranı uygulanır
        await basketService.CreateBasketCacheAsync(basket, cancellationToken);
        return ServiceResult.SuccessAsNoContent();                                                // İşlem başarılıysa, 204 No Content döndürülür
    }
}