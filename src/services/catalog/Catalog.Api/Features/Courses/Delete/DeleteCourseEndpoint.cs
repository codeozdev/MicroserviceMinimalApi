using MediatR;
using Shared.Extensions;

namespace Catalog.Api.Features.Courses.Delete
{
    public static class DeleteCourseEndpoint
    {
        public static RouteGroupBuilder DeleteCourseGroupItemEndpoint(this RouteGroupBuilder group)
        {
            group.MapDelete("/{id:guid}", async (IMediator mediator, Guid id) =>
            {
                var result = await mediator.Send(new DeleteCourseCommand(id));
                return result.ToGenericResult();
            }).WithName("DeleteCourse").MapToApiVersion(1, 0);


            return group;
        }
    }
}


// Restfull delete endpointlerinde genelde body'den veri almayiz, sadece route veya query parametrelerinden aliriz.

/*
 
 Bu farkın nedeni, endpoint’e parametreyi nasıl aldığınla ilgilidir:
   •	Eğer endpoint’te parametreyi doğrudan bir command nesnesi olarak alıyorsan (ör: MapPost("/", (CreateCourseCommand command, ...) => ... )), framework otomatik olarak body’den gelen veriyi command nesnesine bind eder. Bu durumda await mediator.Send(command) kullanırsın.
   •	Eğer endpoint’te parametreyi tek tek (ör: id gibi) alıyorsan (ör: MapDelete("/{id}", (Guid id, ...) => ... )), o zaman command nesnesini kendin oluşturman gerekir:
   await mediator.Send(new DeleteCourseCommand(id))
   Özet:
   •	Body’den komple bir command nesnesi alıyorsan: mediator.Send(command)
   •	Route veya query’den tekil parametre alıyorsan: mediator.Send(new CommandType(parametreler))
  

 */