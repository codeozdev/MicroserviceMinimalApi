using MediatR;
using Shared.Extensions;

namespace Catalog.Api.Features.Categories.GetAll;

public static class GetAllCategoryEndpoint
{
    public static RouteGroupBuilder GetAllCategoryGroupItemEndpoint(this RouteGroupBuilder group)
    {
        group.MapGet("/", async (IMediator mediator) =>
        {
            var result = await mediator.Send(new GetAllCategoryQuery());  // kullaniciya hicbir sey gostermiyeceksek bile bos bir query gondermek zorundayiz (MediatR'in ozelligi)
            return result.ToGenericResult();
        }).MapToApiVersion(1, 0);

        return group;
    }
}


// Okumada FluentValidation filtera ihtiyac yok gibi



/*

 Vertical Slice Architecture kullandigimiz icin artik tum yapi bir arada ve bu sayede kodlarin okunabilirligi daha da artmis oluyor.
  
 GetAllCategoryQuery.cs -> service katmaninda olurdu (dto)        
 GetAllCategoryEndpoint.cs -> api katmaninda olurdu  (controller)
 GetAllCategoryQueryHandler.cs -> service katmaninda olurdu (ProductService.cs gibi)
 
  
 */