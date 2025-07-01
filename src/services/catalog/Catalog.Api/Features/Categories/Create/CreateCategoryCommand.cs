using MediatR;
using Shared;

namespace Catalog.Api.Features.Categories.Create;

public record CreateCategoryCommand(string Name) : IRequest<ServiceResult<CreateCategoryResponse>>;