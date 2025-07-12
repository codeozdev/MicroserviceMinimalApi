using Catalog.Api.Features.Courses.Create;
using Catalog.Api.Features.Courses.GetAll;

namespace Catalog.Api.Features.Courses
{
    public static class CourseEndpointExt
    {
        public static void AddCourseGroupEndpointExt(this WebApplication app)
        {
            app.MapGroup("api/courses").WithTags("Courses")
                .CreateCourseGroupItemEndpoint()
                .GetAllCoursesGroupItemEndpoint();
        }
    }
}


// Program.cs icine ekliyoruz


// WithTags("Courses") endpointlerimizi gruplayarak swagger'da daha duzenli gorunmesini sagliyor