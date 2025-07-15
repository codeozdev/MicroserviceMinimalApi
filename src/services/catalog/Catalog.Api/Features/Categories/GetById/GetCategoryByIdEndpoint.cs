using MediatR;
using Shared.Extensions;

namespace Catalog.Api.Features.Categories.GetById;

public static class GetCourseByIdEndpoint
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