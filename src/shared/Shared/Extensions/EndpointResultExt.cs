using Microsoft.AspNetCore.Http;
using System.Net;

namespace Shared.Extensions;

public static class EndpointResultExt
{
    // Create, Get gibi durumlarda kullanilacak
    public static IResult ToGenericResult<T>(this ServiceResult<T> result)
    {
        return result.Status switch
        {
            HttpStatusCode.OK => Results.Ok(result.Data),
            HttpStatusCode.Created => Results.Created(result.UrlAsCreated, result.Data),
            HttpStatusCode.NotFound => Results.NotFound(result.Fail),
            _ => Results.Problem(result.Fail!) // yukardakilerin haricinde farkli bir durum varsa bu kismi calisacak
        };
    }

    // Update, Delete gibi durumlarda kullanilacak
    public static IResult ToGenericResult(this ServiceResult result)
    {
        return result.Status switch
        {
            HttpStatusCode.NoContent => Results.NoContent(),
            HttpStatusCode.NotFound => Results.NotFound(result.Fail),
            _ => Results.Problem(result.Fail!) // yukardakilerin haricinde farkli bir durum varsa bu kismi calisacak
        };
    }
}


// result nesnesi yani ServiceResult sinifinin nesnesi icindeki Status propunun degerine gore hangi durum kodunun donmesi gerektigini belirliyoruz.
// bunu da CreateCategoryGroupItemEndpoint.cs dosyasindaki return result.ToResult(); ile kullandik


// IResult -> geriye donecegim sonucu temsil ediyor.
// ToGenericResult -> methodun ismi, ServiceResult sinifinin nesnesini aliyor ve IResult donduruyor.