namespace Catalog.Api.Features.Courses.Response
{
    public class FeatureDto
    {
        public int Duration { get; set; }
        public float Rating { get; set; }
        public string EducatorFullName { get; set; } = null!;
    };
}