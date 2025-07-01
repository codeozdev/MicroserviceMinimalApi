namespace Catalog.Api.Features.Courses;

public class Feature
{
    public int Duration { get; set; }
    public float Rating { get; set; }
    public string EducatorFullName { get; set; } = null!;
}

// bir id'si yok çünkü Mongodb kullanıyoruz