using Asp.Versioning;
using Asp.Versioning.Builder;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Shared.Extensions
{
    public static class VersioningExt
    {
        // servis kisminin kodlari
        public static IServiceCollection AddVersioningExt(this IServiceCollection services)
        {
            services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;           // belirtilmemişse varsayılan versiyonu kullan (header kisminda versiyon belirtilmemişse)
                options.ReportApiVersions = true;                             // response header'da versiyon bilgisini göster
                options.ApiVersionReader = new UrlSegmentApiVersionReader();  // URL, Query veya Header'dan versiyon bilgisini al olarak degistirebiliriz
            }).AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'V";
                options.SubstituteApiVersionInUrl = true;
            });


            return services;
        }


        // middleware kisminin kodlari
        public static ApiVersionSet AddVersionSetExt(this WebApplication app)
        {
            var apiVersionSet = app.NewApiVersionSet()
                .HasApiVersion(new ApiVersion(1, 0))                           // endpointlerimizin kullanacagi versiyonlari belirliyoruz
                .HasApiVersion(new ApiVersion(1, 2))
                .ReportApiVersions()
                .Build();
            return apiVersionSet;
        }
    }
}

// AddApiExplorer -> kismi Swagger ile ilgili kısım

// group endpointlerine ekledikten sonra artik endpointlerimiz 1.0 ve 1.2 versiyonlarini destekliyor oldu
// fakat endpointlerimizin hangi versiyonu kullanacagini belirtmemiz gerekiyor bunun icinde tek tek hepsine hangi versiyonun kullanilacagini belirtiyoruz
// tum endpointlerimize versiyonunu eklemek zorundayiz yoksa calismaz cunku grup kisminda url arasina artik bir versiyon bilgisi gelecek diye belirttik