using Catalog.Api.Features.Courses.Response;
using Catalog.Api.Repositories;
using MediatR;
using Shared;

namespace Catalog.Api.Features.Courses.Delete
{
    public class DeleteCourseHandler(AppDbContext context) : IRequestHandler<DeleteCourseCommand, ServiceResult>
    {
        public async Task<ServiceResult> Handle(DeleteCourseCommand request, CancellationToken cancellationToken)
        {
            var hasCourse = await context.Courses.FindAsync([request.Id], cancellationToken);

            if (hasCourse is null)
            {
                return ServiceResult<CourseDto>.Error("Course not found", $"Course with id {request.Id} not found", System.Net.HttpStatusCode.NotFound);
            }

            context.Courses.Remove(hasCourse);
            await context.SaveChangesAsync(cancellationToken);
            return ServiceResult.SuccessAsNoContent();
        }
    }
}