using AutoMapper;
using Catalog.Api.Features.Courses.Response;
using Catalog.Api.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared;

namespace Catalog.Api.Features.Courses.GetById
{
    public class GetCourseByIdHandler(AppDbContext context, IMapper mapper) : IRequestHandler<GetCourseByIdQuery, ServiceResult<CourseDto>>
    {
        public async Task<ServiceResult<CourseDto>> Handle(GetCourseByIdQuery request, CancellationToken cancellationToken)
        {
            var hasCourse = await context.Courses.FirstOrDefaultAsync(cancellationToken: cancellationToken);

            if (hasCourse is null)
            {
                return ServiceResult<CourseDto>.Error("Course not found", $"Course with id {request.Id} not found", System.Net.HttpStatusCode.NotFound);
            }

            var hasCategory = await context.Categories.FindAsync(hasCourse.CategoryId, cancellationToken);

            hasCourse.Category = hasCategory!;

            var courseAsDto = mapper.Map<CourseDto>(hasCourse);
            return ServiceResult<CourseDto>.SuccessAsOk(courseAsDto);
        }
    }
}