using Shared;

namespace Catalog.Api.Features.Courses.Delete
{
    public record DeleteCourseCommand(Guid Id) : IRequestByServiceResult;
}


// geriye bir sey donmediği için IRequestByServiceResult kullanıldı (generic olani kullanmadik)