namespace Catalog.Api.Features.Courses.Response
{
    public record CourseDto(
        Guid Id,
        string Name,
        string Description,
        decimal Price,
        string ImageUrl,
        string Category,
        FeatureDto Feature
    );
}