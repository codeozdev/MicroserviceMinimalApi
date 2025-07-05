using Catalog.Api.Features.Categories.Response;
using MediatR;
using Shared;

namespace Catalog.Api.Features.Categories.GetAll.GetById;

public record GetCategoryByIdQuery(Guid Id) : IRequest<ServiceResult<CategoryDto>>;