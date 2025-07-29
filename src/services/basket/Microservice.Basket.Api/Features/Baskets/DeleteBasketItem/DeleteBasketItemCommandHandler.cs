using MediatR;
using Shared;
using System.Net;
using System.Text.Json;

namespace Microservice.Basket.Api.Features.Baskets.DeleteBasketItem;


/// <summary>
///  Kullanıcının sepetinden bir ürün silinir. Sepet ya da ürün bulunamazsa uygun hata mesajı döner. Güncel sepet cache'e tekrar yazılır.
/// </summary>
public class DeleteBasketItemCommandHandler(BasketService basketService) : IRequestHandler<DeleteBasketItemCommand, ServiceResult>
{
    public async Task<ServiceResult> Handle(DeleteBasketItemCommand request, CancellationToken cancellationToken)
    {
        var basketAsString = await basketService.GetBasketFromCache(cancellationToken); // Cache'den mevcut sepet alınır (varsa)

        if (string.IsNullOrEmpty(basketAsString)) // Sepet yoksa, silinecek ürün de yok demektir
        {
            return ServiceResult.Error("Basket not found", HttpStatusCode.NotFound);
        }

        var currentBasket = JsonSerializer.Deserialize<Data.Basket>(basketAsString); // Cache'deki JSON, C# nesnesine dönüştürülür (yukardaki if koşuluna girmezse demek ki bir basket var ve onu nesneye çeviriyoruz)

        var basketItemToDelete = currentBasket!.Items.FirstOrDefault(x => x.Id == request.Id); // Silinmek istenen ürün bulunur

        if (basketItemToDelete is null) // Ürün sepette yoksa
        {
            return ServiceResult.Error("Basket item not found", HttpStatusCode.NotFound);
        }

        currentBasket.Items.Remove(basketItemToDelete); // Ürün sepetten çıkarılır
        await basketService.CreateBasketCacheAsync(currentBasket, cancellationToken);
        return ServiceResult.SuccessAsNoContent(); // Başarılı yanıt döndürülür (204 No Content)
    }
}


// basket duruyor basket içindeki userid duruyor fakat basket içindeki basketItem silinmiş oluyor
