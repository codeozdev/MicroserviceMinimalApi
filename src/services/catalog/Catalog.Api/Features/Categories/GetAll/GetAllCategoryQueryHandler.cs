using AutoMapper;
using Catalog.Api.Features.Categories.Response;
using Catalog.Api.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared;

namespace Catalog.Api.Features.Categories.GetAll;

public class GetAllCategoryQueryHandler(AppDbContext context, IMapper mapper) : IRequestHandler<GetAllCategoryQuery, ServiceResult<List<CategoryDto>>>
{
    public async Task<ServiceResult<List<CategoryDto>>> Handle(GetAllCategoryQuery request, CancellationToken cancellationToken)
    {
        var categories = await context.Categories.ToListAsync(cancellationToken: cancellationToken);
        var categoriesAsDto = mapper.Map<List<CategoryDto>>(categories);
        return ServiceResult<List<CategoryDto>>.SuccessAsOk(categoriesAsDto);
    }
}