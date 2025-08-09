using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Shared.Options;
using System.Security.Claims;

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

            // Hedef API'nin kimliği (Audience)
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
                RoleClaimType = "roles", // realm_access kismindan cikardigimiz alandir bu sayede rollere erisebilecegiz
                NameClaimType = "preferred_username" // Kullanıcının adı bu claim key'inden alınır
            };
        }).AddJwtBearer("ClientCredentialSchema", options =>
        {
            options.Authority = identityOptions.Address;
            options.Audience = identityOptions.Audience;
            options.RequireHttpsMetadata = false;

            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                ValidateLifetime = true,
                ValidateIssuer = true,
                RoleClaimType = "roles", // realm_access kismindan cikardigimiz alandir bu sayede rollere erisebilecegiz
                NameClaimType = "preferred_username"
            };
        });

        // Yetkilendirme için iki ayrı policy tanımlanıyor:
        // 1. "Password": JWT Bearer ile doğrulanmış, Email claim'i olan kullanıcılar için.
        // 2. "ClientCredential": Özel ClientCredentialSchema ile doğrulanmış, client_id claim'i olan istemciler için.
        services.AddAuthorizationBuilder()
            // "Password" isminde bir yetkilendirme politikası ekleniyor
            .AddPolicy("Password", policy =>
            {
                // Bu politika, JWT Bearer Authentication şemasıyla çalışacak
                policy.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);

                // Kullanıcının kimliğinin doğrulanmış olması gerekiyor
                policy.RequireAuthenticatedUser();

                // Kullanıcının token içinde "Email" claim'ine sahip olması gerekiyor
                policy.RequireClaim(ClaimTypes.Email);
            })

            // "ClientCredential" isminde ikinci bir yetkilendirme politikası ekleniyor
            .AddPolicy("ClientCredential", policy =>
            {
                // Bu politika, "ClientCredentialSchema" isimli özel authentication şemasıyla çalışacak
                policy.AuthenticationSchemes.Add("ClientCredentialSchema");

                // Kimliği doğrulanmış olması gerekiyor
                policy.RequireAuthenticatedUser();

                // Token içinde "client_id" claim'ine sahip olması gerekiyor
                policy.RequireClaim("client_id");
            });


        // IServiceCollection'ı geri döndürüyoruz (method chaining için)
        return services;
    }
}

// Kimlik dogrulama ve yetkilendirmeyi hangi microservislerde kullanacaksak Program.cs icinde bu servisi cagiriyoruz
// Ayrica tum endpoint sinifini tutan EndpointExt kisimlarina  .RequireAuthorization(); ekliyoruz