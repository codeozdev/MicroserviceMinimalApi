﻿using MediatR;
using Shared.Extensions;

namespace Microservice.Basket.Api.Features.Baskets.GetBasket;

public static class GetBasketQueryEndpoint
{
    public static RouteGroupBuilder GetBasketGroupItemEndpoint(this RouteGroupBuilder group)
    {
        group.MapGet("/user",
                async (IMediator mediator) =>
                    (await mediator.Send(new GetBasketQuery())).ToGenericResult())
            .WithName("GetBasket")
            .MapToApiVersion(1, 0);

        return group;
    }
}