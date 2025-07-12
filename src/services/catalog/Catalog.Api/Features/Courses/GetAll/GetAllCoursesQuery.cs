using Catalog.Api.Features.Courses.Response;
using Shared;

namespace Catalog.Api.Features.Courses.GetAll
{
    public record GetAllCoursesQuery : IRequestByServiceResult<List<CourseDto>>;
}