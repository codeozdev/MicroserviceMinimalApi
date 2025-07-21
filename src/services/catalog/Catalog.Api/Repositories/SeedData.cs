using Catalog.Api.Features.Categories;
using Catalog.Api.Features.Courses;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Api.Repositories
{
    public static class SeedData
    {
        public static async Task AddSeedDataExt(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            dbContext.Database.AutoTransactionBehavior = AutoTransactionBehavior.Never; // AutoTransactionBehavior.Never ile transaction'larin otomatik olarak baslatilmasini engelliyoruz (yoksa hata olusuyor)

            if (!dbContext.Categories.Any())
            {
                var categories = new List<Category>
                {
                    new() { Id = NewId.NextSequentialGuid(), Name = "Development" },
                    new() { Id = NewId.NextSequentialGuid(), Name = "Business" },
                    new() { Id = NewId.NextSequentialGuid(), Name = "IT & Software" },
                    new() { Id = NewId.NextSequentialGuid(), Name = "Office Productivity" },
                    new() { Id = NewId.NextSequentialGuid(), Name = "Personal Development" }
                };

                dbContext.Categories.AddRange(categories);
                await dbContext.SaveChangesAsync();
            }

            if (!dbContext.Courses.Any())
            {
                var category = await dbContext.Categories.FirstAsync();

                var randomUserId = NewId.NextGuid();

                List<Course> courses =
                [
                    new()
                    {
                        Id = NewId.NextSequentialGuid(),
                        Name = "C#",
                        Description = "C# Course",
                        Price = 100,
                        UserId = randomUserId,
                        CreatedDate = DateTime.UtcNow,
                        Feature = new Feature { Duration = 10, Rating = 4, EducatorFullName = "Ahmet Yıldız" },
                        CategoryId = category.Id
                    },

                    new()
                    {
                        Id = NewId.NextSequentialGuid(),
                        Name = "Java",
                        Description = "Java Course",
                        Price = 200,
                        UserId = randomUserId,
                        CreatedDate = DateTime.UtcNow,
                        Feature = new Feature { Duration = 10, Rating = 4, EducatorFullName = "Ahmet Yıldız" },
                        CategoryId = category.Id
                    },

                    new()
                    {
                        Id = NewId.NextSequentialGuid(),
                        Name = "Python",
                        Description = "Python Course",
                        Price = 300,
                        UserId = randomUserId,
                        CreatedDate = DateTime.UtcNow,
                        Feature = new Feature { Duration = 10, Rating = 4, EducatorFullName = "Ahmet Yıldız" },
                        CategoryId = category.Id
                    }
                ];


                dbContext.Courses.AddRange(courses);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}


// using kavrami calismasi bittikten sonra kendisini dispose eder (kapatir) 



/*
 *
 *
   Extensions methodlarin cagirimi. Neden SeedData olarak cagirmiyoruzda methodu uzerine ekliyoruz?   

   Yani aslında tam olarak şu oluyor:
   •	Normal çağırım: await SeedData.AddSeedDataExt(app); 
   •	Extension method sayesinde: await app.AddSeedDataExt();
   Extension method'lar C#'ın güzel özelliklerinden biridir ve kodun daha okunabilir olmasını sağlar. Burada da SeedData sınıfını direkt kullanmak yerine, sanki WebApplication'ın kendi methodu gibi kullanabiliyoruz.
   Özetle:
   •	Extension method kullanarak SeedData sınıfını pas geçip direkt methoda erişmedik
   •	Sadece methodun çağrılma syntax'ını daha elegant hale getirdik
   •	Arka planda yine SeedData sınıfı kullanılıyor

  Evet, aynen öyle!
   await SeedData.AddSeedDataExt(app); şeklinde çağırmak teknik olarak çalışır ama C#’ta extension method kullanmak çok daha okunabilir ve modern bir yaklaşımdır.
   Extension method ile:
   •	Kodun daha doğal ve akıcı görünür (await app.AddSeedDataExt();)
   •	Sanki WebApplication’ın kendi metoduymuş gibi kullanırsın
   •	Projenin okunabilirliği ve bakımı kolaylaşır
   Kısacası, extension method ile çağırmak hem best practice hem de kodun profesyonel görünmesini sağlar.
 

 *
 *  
 *
 */