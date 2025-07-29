using MediatR;
using Microservice.Basket.Api.Const;
using Microservice.Basket.Api.Data;
using Microsoft.Extensions.Caching.Distributed;
using Shared;
using Shared.Services;
using System.Text.Json;

namespace Microservice.Basket.Api.Features.Baskets.AddBasketItem;

public class AddBasketItemCommandHandler(IDistributedCache distributedCache, IIdentityService identityService) : IRequestHandler<AddBasketItemCommand, ServiceResult>
{
    public async Task<ServiceResult> Handle(AddBasketItemCommand request, CancellationToken cancellationToken)
    {
        Guid userId = identityService.GetUserId;
        var cacheKey = string.Format(BasketConst.BasketCacheKey, userId); // Kullanıcıya özel cache anahtarı oluşturuluyor -> dinamik olmasini istedigim yeri format methodu otomatik olarak degistiriyor

        var basketAsString = await distributedCache.GetStringAsync(cacheKey, token: cancellationToken); // Cache'den mevcut sepet alınır (varsa)

        Data.Basket? currentBasket; // Geçerli sepetin tutulacağı değişken

        var newBasketItem = new BasketItem( // Yeni eklenecek ürün oluşturuluyor
            id: request.CourseId,
            name: request.CourseName,
            imageUrl: request.ImageUrl ?? string.Empty,
            price: request.CoursePrice,
            priceByApplyDiscountRate: null // İndirim oranı uygulanmadı
        );

        if (string.IsNullOrEmpty(basketAsString)) // Sepet daha önce oluşturulmamışsa
        {
            currentBasket = new Data.Basket(userId, [newBasketItem]); // Yeni bir sepet oluştur ve ürünü içine ekle
        }
        else
        {
            currentBasket = JsonSerializer.Deserialize<Data.Basket>(basketAsString); // Var olan sepeti JSON'dan nesye dönüştür

            var existingItem = currentBasket!.Items.FirstOrDefault(x => x.Id == request.CourseId); // Sepette aynı ürün var mı kontrol et

            if (existingItem is not null) // Eğer aynı ürün varsa önce sil, sonra güncel haliyle ekle
            {
                currentBasket.Items.Remove(existingItem);
            }

            // Aynı ürün yoksa doğrudan ekle
            currentBasket.Items.Add(newBasketItem);
            currentBasket.ApplyAvailableDiscount();  // Sepetteki ürünlere indirim oranı uygula (varsa) diger eklemelerde de indirim kuponu uygulanmissa indirim oranı uygulanacak
        }

        basketAsString = JsonSerializer.Serialize(currentBasket); // Sepeti tekrar JSON'a çevir
        await distributedCache.SetStringAsync(cacheKey, basketAsString, token: cancellationToken); // Cache'e geri yaz
        return ServiceResult.SuccessAsNoContent(); // Başarılı yanıt dön
    }
}