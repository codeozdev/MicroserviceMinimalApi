using MediatR;
using Shared.Extensions;
using Shared.Filters;

namespace Catalog.Api.Features.Courses.Update
{
    public static class UpdateCourseCommandEndpoint
    {
        public static RouteGroupBuilder UpdateCourseGroupItemEndpoint(this RouteGroupBuilder group)
        {
            group.MapPut("/", async (UpdateCourseCommand command, IMediator mediator) =>
            {
                var result = await mediator.Send(command);
                return result.ToGenericResult();
            }).AddEndpointFilter<ValidationFilter<UpdateCourseCommand>>().WithName("UpdateCourse").MapToApiVersion(1, 0);


            return group;
        }
    }
}


// update ve post da filter kullaniyoruz