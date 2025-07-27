using Shared;

namespace Microservice.Basket.Api.Features.Baskets.DeleteBasketItem;

public record DeleteBasketItemCommand(Guid Id) : IRequestByServiceResult;


// basket içindeki ürünlerin id'si ile silinmesini sağlar.