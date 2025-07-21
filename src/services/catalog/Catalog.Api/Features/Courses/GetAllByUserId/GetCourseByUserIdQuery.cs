using Catalog.Api.Features.Courses.Response;
using Shared;

namespace Catalog.Api.Features.Courses.GetAllByUserId
{
    public record GetCourseByUserIdQuery(Guid Id) : IRequestByServiceResult<List<CourseDto>>;
}