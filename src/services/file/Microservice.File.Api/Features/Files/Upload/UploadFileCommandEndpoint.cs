using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Extensions;

namespace Microservice.File.Api.Features.Files.Upload;

public static class UploadFileCommandEndpoint
{
    public static RouteGroupBuilder UploadFileGroupItemEndpoint(this RouteGroupBuilder group)
    {
        group.MapPost("/",
                async (IFormFile file, IMediator mediator) =>
                    (await mediator.Send(new UploadFileCommand(file))).ToGenericResult())
            .WithName("upload")
            .MapToApiVersion(1, 0)
            .Produces<Guid>(StatusCodes.Status201Created)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError)
            .DisableAntiforgery(); // Antiforgery token kullanmıyoruz çünkü bu endpoint sadece dosya yükleme işlemi yapıyor ve CSRF saldırılarına karşı koruma gerektirmiyor.

        return group;
    }
}


// file  buraya ne isim vermis isek key tarafina bunu yazmamiz lazim. 