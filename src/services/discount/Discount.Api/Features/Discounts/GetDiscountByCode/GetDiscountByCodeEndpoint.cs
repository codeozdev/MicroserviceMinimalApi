using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Extensions;

namespace Discount.Api.Features.Discounts.GetDiscountByCode
{
    public static class GetDiscountByCodeEndpoint
    {
        public static RouteGroupBuilder GetDiscountByCodeGroupItemEndpoint(this RouteGroupBuilder group)
        {
            group.MapGet("/{code:length(10)}", async (string code, IMediator mediator) =>
                {
                    var result = await mediator.Send(new GetDiscountByCodeQuery(code));
                    return result.ToGenericResult();
                })
                .WithName("GetDiscountByCode")
                .MapToApiVersion(1, 0)
                .Produces<GetDiscountByCodeQueryResponse>(StatusCodes.Status200OK)
                .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
                .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

            return group;
        }
    }
}