using MediatR;
using Microservice.Basket.Api.Const;
using Microsoft.Extensions.Caching.Distributed;
using Shared;
using Shared.Services;
using System.Net;
using System.Text.Json;

namespace Microservice.Basket.Api.Features.Baskets.RemoveDiscountCoupon;

/// <summary>
/// Kullanıcının sepetinden kupon ve indirim oranını kaldırır.
/// </summary>
public class RemoveDiscountCouponCommandHandler(IDistributedCache distributedCache, IIdentityService identityService) : IRequestHandler<RemoveDiscountCouponCommand, ServiceResult>
{
    public async Task<ServiceResult> Handle(RemoveDiscountCouponCommand request, CancellationToken cancellationToken)
    {
        var cacheKey = string.Format(BasketConst.BasketCacheKey, identityService.GetUserId);  // Kullanıcının sepetine özel cache anahtarı oluşturuluyor
        var basketAsJson = await distributedCache.GetStringAsync(cacheKey, token: cancellationToken); // Redis'ten kullanıcının sepet verisi JSON olarak alınır


        if (string.IsNullOrEmpty(basketAsJson)) // Sepet bulunamazsa 404 hatası döndürülür
        {
            return ServiceResult.Error("Basket not found", HttpStatusCode.NotFound);
        }

        var basket = JsonSerializer.Deserialize<Data.Basket>(basketAsJson);  // JSON string, Basket nesnesine deserialize edilir
        basket!.ClearDiscount();                                             // Sepetten kupon ve indirim oranı kaldırılır
        basketAsJson = JsonSerializer.Serialize(basket);
        await distributedCache.SetStringAsync(cacheKey, basketAsJson, token: cancellationToken);  // Güncellenmiş sepet Redis'e kaydedilir
        return ServiceResult.SuccessAsNoContent();                                                // İşlem başarılıysa, 204 No Content döndürülür
    }
}