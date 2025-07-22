using MediatR;
using Shared.Extensions;

namespace Catalog.Api.Features.Courses.GetAllByUserId
{
    public static class GetCourseByUserIdEndpoint
    {
        public static RouteGroupBuilder GetByUserIdCourseGroupItemEndpoint(this RouteGroupBuilder group)
        {
            group.MapGet("/user/{userId:guid}",
                    async (IMediator mediator, Guid userId) =>
                        (await mediator.Send(new GetCourseByUserIdQuery(userId))).ToGenericResult())
                .WithName("GetByUserIdCourses").MapToApiVersion(1, 0);

            return group;
        }
    }
}