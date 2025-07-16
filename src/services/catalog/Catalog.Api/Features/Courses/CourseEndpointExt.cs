using Catalog.Api.Features.Courses.Create;
using Catalog.Api.Features.Courses.Delete;
using Catalog.Api.Features.Courses.GetAll;
using Catalog.Api.Features.Courses.GetById;
using Catalog.Api.Features.Courses.Update;

namespace Catalog.Api.Features.Courses
{
    public static class CourseEndpointExt
    {
        public static void AddCourseGroupEndpointExt(this WebApplication app)
        {
            app.MapGroup("api/courses").WithTags("Courses")
                .CreateCourseGroupItemEndpoint()
                .GetAllCoursesGroupItemEndpoint()
                .GetByIdCourseGroupItemEndpoint()
                .UpdateCourseGroupItemEndpoint()
                .DeleteCourseGroupItemEndpoint();
        }
    }
}


// Program.cs icine ekliyoruz


// WithTags("Courses") endpointlerimizi gruplayarak swagger'da daha duzenli gorunmesini sagliyor