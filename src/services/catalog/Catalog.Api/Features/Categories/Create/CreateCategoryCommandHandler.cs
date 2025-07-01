using Catalog.Api.Repositories;
using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared;
using System.Net;

namespace Catalog.Api.Features.Categories.Create;

public class CreateCategoryCommandHandler(AppDbContext context) : IRequestHandler<CreateCategoryCommand, ServiceResult<CreateCategoryResponse>>
{
    public async Task<ServiceResult<CreateCategoryResponse>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {

        // kullanicinin gonderdigi isimde bir kategori var mi yok mu kontrolu
        var existCategory = await context.Categories.AnyAsync(x => x.Name == request.Name, cancellationToken: cancellationToken);

        if (existCategory)
        {
            ServiceResult<CreateCategoryResponse>.Error("Category Name already exists", $"The category name '{request.Name}' already exists", HttpStatusCode.BadRequest);
        }

        // kullanicinin gonderdigi isimde bir kategori yoksa yeni kategori olusturuyoruz
        var category = new Category
        {
            Name = request.Name,
            Id = NewId.NextSequentialGuid() // indexlemesi daha performansli olmasi adina bunu kullaniyoruz
        };


        await context.Categories.AddAsync(category, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return ServiceResult<CreateCategoryResponse>.SuccessAsCreated(new CreateCategoryResponse(category.Id), "<empty>");
    }
}


// dbcontext buraya gececek bu sayede Repository pattern kullanmaktan kurtulduk fakat dezavantaji yarin birgun ORM degisikligine gidersek burasi bir bussiness oldugu icin buranin tamamini guncellememiz gerekecek.
// efcore degistirmeyeceksek Repository pattern kullanmaya gerek yok, arada katmanlar olunca biraz performans kaybi oluyor.

// mediater ile servis katmani olusturmamiza gerek yok zaten bussiness katmani burasi oluyor
