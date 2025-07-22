using MediatR;
using Shared.Extensions;
using Shared.Filters;

namespace Catalog.Api.Features.Categories.Create;

public static class CreateCategoryEndpoint
{
    public static RouteGroupBuilder CreateCategoryGroupItemEndpoint(this RouteGroupBuilder group)
    {
        // http://localhost:5000/api/categories -> group kisminda ne yaziyorsak "/" ona denk gelir /a dersek /api/categories/a olarak erisebiliriz
        group.MapPost("/", async (CreateCategoryCommand command, IMediator mediator) =>
        {
            var result = await mediator.Send(command);
            return result.ToGenericResult();
        }).AddEndpointFilter<ValidationFilter<CreateCategoryCommand>>().MapToApiVersion(1, 0);


        return group;
    }
}



/*

group.MapPost("/", async (CreateCategoryCommand command, IMediator mediator) =>
    {
        var result = await mediator.Send(command);

        Results.Ok();  // boyle hep yazmak yerine extension method yaptik (Shared.Extensions.EndpointResultExt.cs)
    });

 */