using MediatR;
using Microservice.Basket.Api.Const;
using Microservice.Basket.Api.Dto;
using Microsoft.Extensions.Caching.Distributed;
using Shared;
using Shared.Services;
using System.Net;
using System.Text.Json;

namespace Microservice.Basket.Api.Features.Baskets.ApplyDiscountCoupon;

/// <summary>
///  Kullanıcının sepetindeki ürüne kupon ile indirim oranı uygular.
/// </summary>
public class ApplyDiscountCouponCommandHandler(IDistributedCache distributedCache, IIdentityService identityService) : IRequestHandler<ApplyDiscountCouponCommand, ServiceResult>
{
    public async Task<ServiceResult> Handle(ApplyDiscountCouponCommand request, CancellationToken cancellationToken)
    {
        var cacheKey = string.Format(BasketConst.BasketCacheKey, identityService.GetUserId);  // Kullanıcının sepetine özel cache anahtarı oluşturuluyor
        var basketAsJson = await distributedCache.GetStringAsync(cacheKey, token: cancellationToken); // Redis'ten kullanıcının sepet verisi JSON olarak alınır

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
        basketAsJson = JsonSerializer.Serialize(basket);                                          // Güncellenmiş sepet tekrar JSON formatına çevrilir
        await distributedCache.SetStringAsync(cacheKey, basketAsJson, token: cancellationToken);  // Güncellenmiş sepet Redis'e kaydedilir
        return ServiceResult.SuccessAsNoContent();                                                // İşlem başarılıysa, 204 No Content döndürülür
    }
}