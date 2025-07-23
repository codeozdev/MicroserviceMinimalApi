using MediatR;
using Shared.Extensions;
using Shared.Filters;

namespace Discount.Api.Features.Discounts.Create
{
    public static class CreateDiscountCommandEndpoint
    {
        public static RouteGroupBuilder CreateDiscountGroupItemEndpoint(this RouteGroupBuilder group)
        {
            group.MapPost("/", async (CreateDiscountCommand command, IMediator mediator) =>
            {
                var result = await mediator.Send(command);
                return result.ToGenericResult();
            }).AddEndpointFilter<ValidationFilter<CreateDiscountCommand>>().MapToApiVersion(1, 0);


            return group;
        }
    }
}