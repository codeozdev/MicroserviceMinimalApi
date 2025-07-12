using AutoMapper;
using Catalog.Api.Features.Courses.Response;
using Catalog.Api.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared;

namespace Catalog.Api.Features.Courses.GetAll
{
    public class GetAllCoursesQueryHandler(AppDbContext context, IMapper mapper) : IRequestHandler<GetAllCoursesQuery, ServiceResult<List<CourseDto>>>
    {
        public async Task<ServiceResult<List<CourseDto>>> Handle(GetAllCoursesQuery request, CancellationToken cancellationToken)
        {
            var courses = await context.Courses.ToListAsync(cancellationToken: cancellationToken);

            var categories = await context.Categories.ToListAsync(cancellationToken: cancellationToken);


            foreach (var course in courses)
            {
                course.Category = categories.First(c => c.Id == course.CategoryId);
            }

            var coursesAsDto = mapper.Map<List<CourseDto>>(courses);
            return ServiceResult<List<CourseDto>>.SuccessAsOk(coursesAsDto);

        }
    }
}



// MongoDB.Driver.Linq olani kullanmiyoruz yoksa hata aliriz