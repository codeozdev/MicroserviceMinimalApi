using Catalog.Api.Features.Categories.Response;
using Shared;

namespace Catalog.Api.Features.Categories.GetById;

public record GetCategoryByIdQuery(Guid Id) : IRequestByServiceResult<CategoryDto>;