using MediatR;
using Shared.Extensions;

namespace Catalog.Api.Features.Courses.GetById;

public static class GetCourseByIdEndpoint
{
    public static RouteGroupBuilder GetByIdCourseGroupItemEndpoint(this RouteGroupBuilder group)
    {
        group.MapGet("/{id:guid}", async (IMediator mediator, Guid id) =>
        {
            var result = await mediator.Send(new GetCourseByIdQuery(id));
            return result.ToGenericResult();

        });


        return group;
    }
}