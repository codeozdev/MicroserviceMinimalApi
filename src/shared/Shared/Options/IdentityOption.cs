namespace Shared.Options;

public class IdentityOption
{
    public required string Address { get; set; } // keycloak sunucusunun base url adresidir
    public required string Issuer { get; set; } // Tokeni imzalayan Keycloak
    public required string Audience { get; set; } // Gelen tokenin istek atmaya yetkisi var mi
}

// isimler appsettings.json icindekilerle ayni olmak zorunda
/*

Bu `IdentityOption` sınıfı, **JWT (JSON Web Token) authentication** yapılandırması için gerekli olan ayarları tip güvenli bir şekilde tutmak amacıyla oluşturulmuş bir **configuration class**'ıdır.

## Neden Yapıldığı
Bu sınıfın oluşturulma sebepleri şunlardır:
1. **Tip Güvenliği**: appsettings.json dosyasındaki değerleri string olarak okumak yerine, strongly-typed bir sınıf üzerinden erişim sağlar
2. **IntelliSense Desteği**: IDE'de otomatik tamamlama ve hata kontrolü
3. **Merkezi Yönetim**: Tüm identity-related yapılandırmaları tek bir yerde toplar
4. **Hata Önleme**: Yanlış property isimleri kullanımını engeller

 */