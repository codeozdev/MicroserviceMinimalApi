using AutoMapper;
using Catalog.Api.Repositories;
using MediatR;
using Shared;

namespace Catalog.Api.Features.Courses.Update
{
    public class UpdateCourseCommandHandler(AppDbContext context, IMapper mapper) : IRequestHandler<UpdateCourseCommand, ServiceResult>
    {
        public async Task<ServiceResult> Handle(UpdateCourseCommand request, CancellationToken cancellationToken)
        {
            var hasCourse = await context.Courses.FindAsync([request.Id], cancellationToken);

            if (hasCourse is null)
            {
                return ServiceResult.ErrorAsNotFound();
            }

            hasCourse.Name = request.Name;
            hasCourse.Description = request.Description;
            hasCourse.Price = request.Price;
            hasCourse.ImageUrl = request.ImageUrl;
            hasCourse.CategoryId = request.CategoryId;

            context.Courses.Update(hasCourse);
            var result = await context.SaveChangesAsync(cancellationToken);

            if (result <= 0)
            {
                return ServiceResult.Error("Update Failed", "Failed to update the course.",
                    System.Net.HttpStatusCode.InternalServerError);
            }

            return ServiceResult.SuccessAsNoContent();
        }
    }
}