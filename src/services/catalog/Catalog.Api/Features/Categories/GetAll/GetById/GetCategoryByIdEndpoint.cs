using MediatR;
using Shared.Extensions;

namespace Catalog.Api.Features.Categories.GetAll.GetById;

public static class GetCategoryByIdEndpoint
{
    public static RouteGroupBuilder GetByIdCategoryGroupItemEndpoint(this RouteGroupBuilder group)
    {
        group.MapGet("/{id:guid}", async (IMediator mediator, Guid id) =>
        {
            var result = await mediator.Send(new GetCategoryByIdQuery(id));
            return result.ToGenericResult();

        });


        return group;
    }
}