using Catalog.Api.Features.Categories.Create;
using Catalog.Api.Features.Categories.GetAll;
using Catalog.Api.Features.Categories.GetAll.GetById;

namespace Catalog.Api.Features.Categories;

public static class CategoryEndpointExt
{
    public static void AddCategoryGroupEndpointExt(this WebApplication app)
    {
        app.MapGroup("api/categories")
            .CreateCategoryGroupItemEndpoint()
            .GetAllCategoryGroupItemEndpoint()
            .GetByIdCategoryGroupItemEndpoint();
    }
}

// endponitlerimizi groupladik bu sayede bir filter veya action eklemek istedigimizde tek tek endpointlere eklemek yerine buraya bir kez yazdigimizda tum endpointlere uygulanmis olacak
// ratelimit, authorizantion, versiyon


//  WebApplication app -> program.cs icindeki app nesnesinin bir sinifi buradan aliyoruz ve bunu parametre olarak aldigimizda sanki program.cs icinde yaziyor gibi oluyor

// Vertical Slice Architecture ile minimal api uyumlu olmasini saglıyor.