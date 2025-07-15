using Catalog.Api.Features.Courses.Response;
using Shared;

namespace Catalog.Api.Features.Courses.GetById
{
    public record GetCourseByIdQuery(Guid Id) : IRequestByServiceResult<CourseDto>;
}