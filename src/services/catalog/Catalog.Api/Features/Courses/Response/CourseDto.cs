using Catalog.Api.Features.Categories.Response;

namespace Catalog.Api.Features.Courses.Response
{
    public record CourseDto(
        Guid Id,
        string Name,
        string Description,
        decimal Price,
        string ImageUrl,
        CategoryDto Category,
        FeatureDto Feature
    );
}