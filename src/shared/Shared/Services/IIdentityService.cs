namespace Shared.Services;

public interface IIdentityService
{
    public Guid UserId { get; }
    public string UserName { get; }
    List<string> Roles { get; }
}

// kullaniciya ait extra datalar gerekirse burada tanimlamalarini yapabiliriz