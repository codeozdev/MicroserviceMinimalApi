using MediatR;
using Shared.Extensions;

namespace Catalog.Api.Features.Courses.GetAll
{
    public static class GetAllCoursesEndpoint
    {
        public static RouteGroupBuilder GetAllCoursesGroupItemEndpoint(this RouteGroupBuilder group)
        {
            group.MapGet("/", async (IMediator mediator) =>
                {
                    var result = await mediator.Send(new GetAllCoursesQuery());
                    return result.ToGenericResult();
                }).WithName("GetAllCourse");

            return group;
        }
    }
}



// Get isleminde bir command olmuyor onlari kaldiriyoruz


