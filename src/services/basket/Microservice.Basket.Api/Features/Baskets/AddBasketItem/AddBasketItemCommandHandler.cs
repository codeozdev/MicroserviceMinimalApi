using MediatR;
using Microservice.Basket.Api.Const;
using Microservice.Basket.Api.Dto;
using Microsoft.Extensions.Caching.Distributed;
using Shared;
using System.Text.Json;

namespace Microservice.Basket.Api.Features.Baskets.AddBasketItem;

public class AddBasketItemCommandHandler(IDistributedCache distributedCache) : IRequestHandler<AddBasketItemCommand, ServiceResult>
{
    public async Task<ServiceResult> Handle(AddBasketItemCommand request, CancellationToken cancellationToken)
    {
        Guid userId = Guid.NewGuid();
        var cacheKey = string.Format(BasketConst.BasketCacheKey, userId); // Kullanıcıya özel cache anahtarı oluşturuluyor -> dinamik olmasini istedigim yeri format methodu otomatik olarak degistiriyor

        var basketAsString = await distributedCache.GetStringAsync(cacheKey, token: cancellationToken); // Cache'den mevcut sepet alınır (varsa)

        BasketDto? currentBasket; // Geçerli sepetin tutulacağı değişken

        var newBasketItem = new BasketItemDto( // Yeni eklenecek ürün oluşturuluyor
            Id: request.CourseId,
            Name: request.CourseName,
            ImageUrl: request.ImageUrl ?? string.Empty,
            Price: request.CoursePrice,
            PriceByApplyDiscountRate: null // İndirim oranı uygulanmadı
        );

        if (string.IsNullOrEmpty(basketAsString)) // Sepet daha önce oluşturulmamışsa
        {
            currentBasket = new BasketDto(userId, [newBasketItem]); // Yeni bir sepet oluştur ve ürünü içine ekle
        }
        else
        {
            currentBasket = JsonSerializer.Deserialize<BasketDto>(basketAsString); // Var olan sepeti JSON'dan nesye dönüştür

            var existingItem = currentBasket!.BasketItems.FirstOrDefault(x => x.Id == request.CourseId); // Sepette aynı ürün var mı kontrol et

            if (existingItem is not null) // Eğer aynı ürün varsa önce sil, sonra güncel haliyle ekle
            {
                currentBasket.BasketItems.Remove(existingItem);
            }

            // Aynı ürün yoksa doğrudan ekle
            currentBasket.BasketItems.Add(newBasketItem);
        }

        basketAsString = JsonSerializer.Serialize(currentBasket); // Sepeti tekrar JSON'a çevir
        await distributedCache.SetStringAsync(cacheKey, basketAsString, token: cancellationToken); // Cache'e geri yaz
        return ServiceResult.SuccessAsNoContent(); // Başarılı yanıt dön
    }
}