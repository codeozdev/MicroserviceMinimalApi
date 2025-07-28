using Microservice.Basket.Api.Dto;
using Shared;

namespace Microservice.Basket.Api.Features.Baskets.GetBasket;

public record GetBasketQuery : IRequestByServiceResult<BasketDto>;