using MediatR;
using Microservice.Basket.Api.Const;
using Microservice.Basket.Api.Dto;
using Microsoft.Extensions.Caching.Distributed;
using Shared;
using Shared.Services;
using System.Net;
using System.Text.Json;

namespace Microservice.Basket.Api.Features.Baskets.GetBasket;


/// <summary>
/// Kullanıcıya ait sepeti getirir. Sepet bulunamazsa uygun hata mesajı döner.
/// </summary>
public class GetBasketQueryHandler(IDistributedCache distributedCache, IIdentityService identityService) : IRequestHandler<GetBasketQuery, ServiceResult<BasketDto>>
{
    public async Task<ServiceResult<BasketDto>> Handle(GetBasketQuery request, CancellationToken cancellationToken)
    {

        var cacheKey = string.Format(BasketConst.BasketCacheKey, identityService.GetUserId); // Kullanıcıya özel cache anahtarı oluşturuluyor -> dinamik olmasini istedigim yeri format methodu otomatik olarak degistiriyor
        var basketAsString = await distributedCache.GetStringAsync(cacheKey, token: cancellationToken); // Cache'den mevcut sepet alınır (varsa)

        if (string.IsNullOrEmpty(basketAsString)) // Eğer sepet yoksa, uygun hata mesajı ve 404 kodu ile geri dönülür
        {
            return ServiceResult<BasketDto>.Error("Basket not found", HttpStatusCode.NotFound);
        }

        var basket = JsonSerializer.Deserialize<BasketDto>(basketAsString)!; // JSON string, BasketDto nesnesine çevrilir (deserialize edilir)
        return ServiceResult<BasketDto>.SuccessAsOk(basket); // Sepet başarıyla bulunduysa, 200 OK ile birlikte response döndürülür
    }
}