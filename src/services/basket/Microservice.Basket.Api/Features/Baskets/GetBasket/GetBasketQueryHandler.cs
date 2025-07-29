using AutoMapper;
using MediatR;
using Microservice.Basket.Api.Dto;
using Shared;
using System.Net;
using System.Text.Json;

namespace Microservice.Basket.Api.Features.Baskets.GetBasket;


/// <summary>
/// Kullanıcıya ait sepeti getirir. Sepet bulunamazsa uygun hata mesajı döner.
/// </summary>
public class GetBasketQueryHandler(IMapper mapper, BasketService basketService) : IRequestHandler<GetBasketQuery, ServiceResult<BasketDto>>
{
    public async Task<ServiceResult<BasketDto>> Handle(GetBasketQuery request, CancellationToken cancellationToken)
    {
        var basketAsJson = await basketService.GetBasketFromCache(cancellationToken); // Cache'den mevcut sepet alınır (varsa)

        if (string.IsNullOrEmpty(basketAsJson)) // Eğer sepet yoksa, uygun hata mesajı ve 404 kodu ile geri dönülür
        {
            return ServiceResult<BasketDto>.Error("Basket not found", HttpStatusCode.NotFound);
        }

        var basket = JsonSerializer.Deserialize<Data.Basket>(basketAsJson)!; // JSON string, BasketDto nesnesine çevrilir (deserialize edilir)
        var basketDto = mapper.Map<BasketDto>(basket);
        return ServiceResult<BasketDto>.SuccessAsOk(basketDto); // Sepet başarıyla bulunduysa, 200 OK ile birlikte response döndürülür
    }
}