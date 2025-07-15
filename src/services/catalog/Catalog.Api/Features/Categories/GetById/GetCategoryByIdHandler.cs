using AutoMapper;
using Catalog.Api.Features.Categories.Response;
using Catalog.Api.Repositories;
using MediatR;
using Shared;

namespace Catalog.Api.Features.Categories.GetById;

public class GetCategoryByIdHandler(AppDbContext context, IMapper mapper) : IRequestHandler<GetCategoryByIdQuery, ServiceResult<CategoryDto>>
{
    public async Task<ServiceResult<CategoryDto>> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        var hasCategory = await context.Categories.FindAsync(request.Id, cancellationToken);

        if (hasCategory is null)
        {
            return ServiceResult<CategoryDto>.Error("Category not found", $"Category with id {request.Id} not found", System.Net.HttpStatusCode.NotFound);
        }

        var categoryAsDto = mapper.Map<CategoryDto>(hasCategory);
        return ServiceResult<CategoryDto>.SuccessAsOk(categoryAsDto);
    }
}




// Query ile kullanicidan bir Id istiyoruz bu kullanicidan istedigimiz veriyi de request ile alabiliyoruz
// Istek Endpoint tarafindan karsilanacak oradan MediatR ile bu istegi handler'a gonderiyoruz