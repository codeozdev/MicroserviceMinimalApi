using MediatR;
using Shared;
using System.Net;
using System.Text.Json;

namespace Microservice.Basket.Api.Features.Baskets.RemoveDiscountCoupon;

/// <summary>
/// Kullanıcının sepetinden kupon ve indirim oranını kaldırır.
/// </summary>
public class RemoveDiscountCouponCommandHandler(BasketService basketService) : IRequestHandler<RemoveDiscountCouponCommand, ServiceResult>
{
    public async Task<ServiceResult> Handle(RemoveDiscountCouponCommand request, CancellationToken cancellationToken)
    {
        var basketAsJson = await basketService.GetBasketFromCache(cancellationToken);  // Cache'den mevcut sepet alınır (varsa)


        if (string.IsNullOrEmpty(basketAsJson)) // Sepet bulunamazsa 404 hatası döndürülür
        {
            return ServiceResult.Error("Basket not found", HttpStatusCode.NotFound);
        }

        var basket = JsonSerializer.Deserialize<Data.Basket>(basketAsJson);                       // JSON string, Basket nesnesine deserialize edilir
        basket!.ClearDiscount();                                                                  // Sepetten kupon ve indirim oranı kaldırılır
        await basketService.CreateBasketCacheAsync(basket, cancellationToken);                    // Güncellenmiş sepet cache'e kaydedilir
        return ServiceResult.SuccessAsNoContent();                                                // İşlem başarılıysa, 204 No Content döndürülür
    }
}