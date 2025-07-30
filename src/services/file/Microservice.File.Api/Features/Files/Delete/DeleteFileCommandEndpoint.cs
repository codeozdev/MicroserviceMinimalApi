using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Extensions;

namespace Microservice.File.Api.Features.Files.Delete;

public static class DeleteFileCommandEndpoint
{
    public static RouteGroupBuilder DeleteFileGroupItemEndpoint(this RouteGroupBuilder group)
    {
        group.MapDelete("",
                async ([FromBody] DeleteFileCommand deleteFileCommand, IMediator mediator) =>
                (await mediator.Send(deleteFileCommand)).ToGenericResult())
            .WithName("delete")
            .MapToApiVersion(1, 0)
            .Produces<Guid>(StatusCodes.Status201Created)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        return group;
    }
}

// Neden body uzerinden silme yapıyoruz? Cunku cok fazla dosya silme istegi yapacaksak eger body uzerinden yapmamiz daha mantikli olur. Fakat asagidaki route ile de yapabiliriz.


/*    II. yaklasim

 *   group.MapDelete("/{fileName}",  fileName = 70fccc31-c693-4769-a223-42730bc58c92.jpg (kendi olusturdugumuz dosya ismi)
         async (string fileName, IMediator mediator) =>
         (await mediator.Send(new DeleteFileCommand)).ToGenericResult())
 *
       /api/v1/files/{fileName} olarak silecektik 
 *   
 *
 */