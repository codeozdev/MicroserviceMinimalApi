using Catalog.Api.Features.Categories.Response;
using MediatR;
using Shared;

namespace Catalog.Api.Features.Categories.GetAll;

public record GetAllCategoryQuery : IRequest<ServiceResult<List<CategoryDto>>>;