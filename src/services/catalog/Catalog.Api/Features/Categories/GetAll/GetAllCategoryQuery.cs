using Catalog.Api.Features.Categories.Response;
using Shared;

namespace Catalog.Api.Features.Categories.GetAll;

public record GetAllCategoryQuery : IRequestByServiceResult<List<CategoryDto>>;