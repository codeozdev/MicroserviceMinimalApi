using Catalog.Api.Features.Categories.Response;
using Shared;

namespace Catalog.Api.Features.Categories.GetAll.GetById;

public record GetCategoryByIdQuery(Guid Id) : IRequestByServiceResult<CategoryDto>;