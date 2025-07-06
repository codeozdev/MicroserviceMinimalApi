using Catalog.Api.Features.Courses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MongoDB.EntityFrameworkCore.Extensions;

namespace Catalog.Api.Repositories;

public class CourseEntityConfiguration : IEntityTypeConfiguration<Course>
{
    public void Configure(EntityTypeBuilder<Course> builder)
    {
        builder.ToCollection("courses");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever(); // MongoDB'de Id'ler kendiliğinden oluşturulmaz bunu zaten NewId paketi oluşturacak
        builder.Property(x => x.Name).HasMaxLength(100).HasElementName("name").IsRequired();
        builder.Property(x => x.Description).HasElementName("description").HasMaxLength(1000);
        builder.Property(x => x.CreatedDate).HasElementName("createdDate");
        builder.Property(x => x.UserId).HasElementName("userId");
        builder.Property(x => x.CategoryId).HasElementName("categoryId");
        builder.Property(x => x.ImageUrl).HasElementName("imageurl").HasMaxLength(200);
        builder.Ignore(x => x.Category);  // navigation propertyler alan olamazlar bunları mongodb kullanırken elle kapatmamız gerekiyor (ilişkisel veritabanlarında otomatik algılar)


        // course bağlı id'si olmayan bir entitymiz var ise onu OwnsOne ile bu şekilde tanımlıyoruz
        builder.OwnsOne(c => c.Feature, feature =>
        {
            feature.HasElementName("features");
            feature.Property(f => f.Duration).HasElementName("duration");
            feature.Property(f => f.Rating).HasElementName("rating");
            feature.Property(f => f.EducatorFullName).HasElementName("educatorFullName").HasMaxLength(100);
        });
    }
}