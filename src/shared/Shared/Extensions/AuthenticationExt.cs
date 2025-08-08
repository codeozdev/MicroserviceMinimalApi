using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Shared.Options;

namespace Shared.Extensions;

/// <summary>
///     Authentication (kimlik doğrulama) ve Authorization (yetkilendirme) ayarlarını
/// </summary>
public static class AuthenticationExt
{
    /// <summary>
    ///     Uygulamaya JWT tabanlı Authentication ve Authorization ekler.
    /// </summary>
    public static IServiceCollection AddAuthenticationAndAuthorizationExt(this IServiceCollection services,
        IConfiguration configuration)
    {
        // appsettings.json içindeki "IdentityOption" bölümünü alır
        // ve bunu IdentityOption tipindeki bir nesneye dönüştürür.
        // Örn: appsettings.json → "Address", "Audience" gibi değerler bu sınıfa maplenir.
        IdentityOption? identityOptions = configuration.GetSection(nameof(IdentityOption)).Get<IdentityOption>();

        // Authentication (kimlik doğrulama) servisini ekliyoruz.
        // Varsayılan olarak JWT Bearer kullanılacağını belirtiyoruz.
        services.AddAuthentication().AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
        {
            // Token doğrulaması için yetkili kimlik sağlayıcısının adresi (Authority)
            options.Authority = identityOptions.Address;

            // Hedef API’nin kimliği (Audience)
            options.Audience = identityOptions.Audience;

            // HTTPS zorunluluğunu devre dışı bırakıyoruz
            options.RequireHttpsMetadata = false;

            // Token doğrulama parametreleri (giris bileti)
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = true, // Token'ın Audience değeri kontrol edilsin
                ValidateIssuerSigningKey = true, // Token imza anahtarı doğrulansın
                ValidateLifetime = true, // Token süresi kontrol edilsin
                ValidateIssuer = true, // Token'ın hangi issuer'dan geldiği kontrol edilsin
                RoleClaimType = "roles", // Kullanıcının rol bilgisi bu claim key'inden alınır
                NameClaimType = "preferred_username" // Kullanıcının adı bu claim key'inden alınır
            };
        });

        // Authorization (yetkilendirme) servisini ekliyoruz
        services.AddAuthorization();

        // IServiceCollection'ı geri döndürüyoruz (method chaining için)
        return services;
    }
}

// Kimlik dogrulama ve yetkilendirmeyi hangi microservislerde kullanacaksak Program.cs icinde bu servisi cagiriyoruz
// Ayrica tum endpoint sinifini tutan EndpointExt kisimlarina  .RequireAuthorization(); ekliyoruz