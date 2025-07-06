using AutoMapper;
using Catalog.Api.Repositories;
using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared;
using System.Net;

namespace Catalog.Api.Features.Courses.Create
{
    public class CreateCourseCommandHandler(AppDbContext context, IMapper mapper) : IRequestHandler<CreateCourseCommand, ServiceResult<CreateCourseResponse>>
    {
        public async Task<ServiceResult<CreateCourseResponse>> Handle(CreateCourseCommand request, CancellationToken cancellationToken)
        {
            // Kullanici course eklerken bir kategori id gonderiyor, bu kategori id'nin varligini kontrol ediyoruz
            var hasCategory = context.Categories.AnyAsync(x => x.Id == request.CategoryId, cancellationToken);

            if (!await hasCategory)
            {
                return ServiceResult<CreateCourseResponse>.Error("Category not found", $"Category with id {request.CategoryId} not found", HttpStatusCode.NotFound);
            }

            // business validation -> course kontrolu yapıyoruz
            var hasCourse = await context.Courses.AnyAsync(x => x.Name == request.Name, cancellationToken: cancellationToken);

            if (hasCourse)
            {
                ServiceResult<CreateCourseResponse>.Error("Course Name already exists", $"The course name '{request.Name}' already exists", HttpStatusCode.BadRequest);
            }

            // kullanicinin gonderdigi isimde bir product yoksa yeni product olusturuyoruz
            var newCourse = mapper.Map<Course>(request);
            newCourse.Id = NewId.NextSequentialGuid();
            newCourse.CreatedDate = DateTime.UtcNow;
            newCourse.Feature = new Feature() // ornek olarak yaptik ileride degistirecegiz
            {
                Duration = 10,
                Rating = 0,
                EducatorFullName = "Ahmet Yilmaz", // ileride token'dan gelecek
            };

            await context.Courses.AddAsync(newCourse, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
            return ServiceResult<CreateCourseResponse>.SuccessAsCreated(new CreateCourseResponse(newCourse.Id), $"/api/courses/{newCourse.Id}");
        }
    }
}



