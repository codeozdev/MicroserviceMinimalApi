using MediatR;
using Shared.Extensions;
using Shared.Filters;

namespace Microservice.Basket.Api.Features.Baskets.AddBasketItem;


public static class AddBasketItemEndpoint
{
    public static RouteGroupBuilder AddBasketItemGroupItemEndpoint(this RouteGroupBuilder group)
    {
        group.MapPost("/item", async (AddBasketItemCommand command, IMediator mediator) =>
        {
            var result = await mediator.Send(command);
            return result.ToGenericResult();
        }).AddEndpointFilter<ValidationFilter<AddBasketItemCommand>>().MapToApiVersion(1, 0);


        return group;
    }
}
