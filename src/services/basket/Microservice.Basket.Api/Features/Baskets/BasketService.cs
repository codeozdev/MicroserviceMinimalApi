using Microservice.Basket.Api.Const;
using Microsoft.Extensions.Caching.Distributed;
using Shared.Services;
using System.Text.Json;

namespace Microservice.Basket.Api.Features.Baskets;

public class BasketService(IIdentityService identityService, IDistributedCache distributedCache)
{
    private string GetCacheKey()
    {
        return string.Format(BasketConst.BasketCacheKey, identityService.GetUserId);
    }

    public async Task<string?> GetBasketFromCache(CancellationToken cancellationToken)
    {
        return await distributedCache.GetStringAsync(GetCacheKey(), token: cancellationToken);
    }

    public async Task CreateBasketCacheAsync(Data.Basket basket, CancellationToken cancellationToken)
    {
        var basketAsString = JsonSerializer.Serialize(basket);
        await distributedCache.SetStringAsync(GetCacheKey(), basketAsString, token: cancellationToken);
    }
}

// Basket 65.ders
// scope yasam ömrü olacak
// handler içerisinde tekrar eden kodların önüne geçmek adına yardımcı bir servis oluşturuldu





/*

***** GetBasketFromCache *****

Guid userId = identityService.GetUserId;
var cacheKey = string.Format(BasketConst.BasketCacheKey, userId); // Kullanıcıya özel cache anahtarı oluşturuluyor -> dinamik olmasini istedigim yeri format methodu otomatik olarak degistiriyor

var basketAsJson = await distributedCache.GetStringAsync(cacheKey, token: cancellationToken); // Cache'den mevcut sepet alınır (varsa)


 */