using Asp.Versioning.Builder;
using Catalog.Api.Features.Courses.Create;
using Catalog.Api.Features.Courses.Delete;
using Catalog.Api.Features.Courses.GetAll;
using Catalog.Api.Features.Courses.GetAllByUserId;
using Catalog.Api.Features.Courses.GetById;
using Catalog.Api.Features.Courses.Update;

namespace Catalog.Api.Features.Courses
{
    public static class CourseEndpointExt
    {
        public static void AddCourseGroupEndpointExt(this WebApplication app, ApiVersionSet apiVersionSet)
        {
            app.MapGroup("api/v{version:apiVersion}/courses").WithTags("Courses")
                .WithApiVersionSet(apiVersionSet)
                .CreateCourseGroupItemEndpoint()
                .GetAllCoursesGroupItemEndpoint()
                .GetByIdCourseGroupItemEndpoint()
                .UpdateCourseGroupItemEndpoint()
                .DeleteCourseGroupItemEndpoint()
                .GetByUserIdCourseGroupItemEndpoint();
        }
    }
}


// Program.cs icine ekliyoruz


// WithTags("Courses") endpointlerimizi gruplayarak swagger'da daha duzenli gorunmesini sagliyor
// url kismindan versiyonlama yaptigimiz icin url kismini ekliyoruz