using Catalog.Api.Features.Categories;
using Catalog.Api.Repositories;
using MongoDB.Driver.Core.Misc;

namespace Catalog.Api.Features.Courses;

public class Course : BaseEntity
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public decimal Price { get; set; }
    public string? Picture { get; set; }

    public Guid CategoryId { get; set; }
    public Category Category { get; set; } = null!;

    public Guid UserId { get; set; }
    public Feature Feature { get; set; } = null!;

    public DateTime CreatedDate { get; set; }
}


// Course tablosunu çektiğimizde bize Feature bilgileride gelmiş olacak