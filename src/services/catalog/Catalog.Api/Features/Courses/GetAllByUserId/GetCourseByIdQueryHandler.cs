using AutoMapper;
using Catalog.Api.Features.Courses.Response;
using Catalog.Api.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared;

namespace Catalog.Api.Features.Courses.GetAllByUserId
{
    public class GetCourseByIdQueryHandler(AppDbContext context, IMapper mapper) : IRequestHandler<GetCourseByUserIdQuery, ServiceResult<List<CourseDto>>>
    {
        public async Task<ServiceResult<List<CourseDto>>> Handle(GetCourseByUserIdQuery request, CancellationToken cancellationToken)
        {

            var courses = await context.Courses.Where(x => x.UserId == request.Id).ToListAsync(cancellationToken: cancellationToken);
            var categories = await context.Categories.ToListAsync(cancellationToken: cancellationToken);


            // mongodb kullandigimiz icin boyle doldurmak zorundayiz cunku join methodu yok
            foreach (var course in courses)
            {
                course.Category = categories.First(x => x.Id == course.CategoryId);
            }

            var coursesAsDto = mapper.Map<List<CourseDto>>(courses);
            return ServiceResult<List<CourseDto>>.SuccessAsOk(coursesAsDto);
        }
    }
}


// tum kurslari donuyoruz