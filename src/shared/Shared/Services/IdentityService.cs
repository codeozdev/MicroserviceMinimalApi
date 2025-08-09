using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Shared.Services;

/// <summary>
///     HTTP context üzerinden erişilen kullanıcının kimlik bilgilerini sağlayan servis.
///     Kullanıcının doğrulanıp doğrulanmadığını kontrol eder ve kullanıcıya ait
///     Id, kullanıcı adı ve roller gibi bilgileri döner.
/// </summary>
public class IdentityService(IHttpContextAccessor httpContextAccessor) : IIdentityService
{
    // Kullanıcının benzersiz kimlik (GUID) bilgisini döner
    public Guid UserId
    {
        get
        {
            // Kullanıcı doğrulanmamışsa hata fırlatılır
            if (!httpContextAccessor.HttpContext!.User.Identity!.IsAuthenticated)
            {
                throw new UnauthorizedAccessException("User is not authenticated.");
            }

            // UserId bilgisi, NameIdentifier claim'inden alınır ve GUID formatına çevrilir
            return Guid.Parse(
                httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(c =>
                    c.Type == ClaimTypes.NameIdentifier)!.Value!);
        }
    }

    // Kullanıcının kullanıcı adını döner
    public string UserName
    {
        get
        {
            // Kullanıcı doğrulanmamışsa hata fırlatılır
            if (!httpContextAccessor.HttpContext!.User.Identity!.IsAuthenticated)
            {
                throw new UnauthorizedAccessException("User is not authenticated.");
            }

            // Identity içindeki Name property’si kullanıcı adını verir
            return httpContextAccessor.HttpContext!.User.Identity!.Name!;
        }
    }

    // Kullanıcının rollerini içeren listeyi döner
    public List<string> Roles
    {
        get
        {
            // Kullanıcı doğrulanmamışsa hata fırlatılır
            if (!httpContextAccessor.HttpContext!.User.Identity!.IsAuthenticated)
            {
                throw new UnauthorizedAccessException("User is not authenticated.");
            }

            // Claim tipi Role olan tüm claimlerin değerleri toplanır ve liste olarak döner
            return httpContextAccessor.HttpContext!.User.Claims
                .Where(x => x.Type == ClaimTypes.Role)
                .Select(x => x.Value!)
                .ToList();
        }
    }
}

// IHttpContextAccessor -> scope yasam dongusunde oldugu icin buda scope yasam dongusunde olmali