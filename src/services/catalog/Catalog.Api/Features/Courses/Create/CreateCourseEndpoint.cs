using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Extensions;
using Shared.Filters;

namespace Catalog.Api.Features.Courses.Create
{
    public static class CreateCourseEndpoint
    {
        public static RouteGroupBuilder CreateCourseGroupItemEndpoint(this RouteGroupBuilder group)
        {
            // http://localhost:5000/api/categories -> group kisminda ne yaziyorsak "/" ona denk gelir /a dersek /api/categories/a olarak erisebiliriz
            group.MapPost("/", async (CreateCourseCommand command, IMediator mediator) =>
                {
                    var result = await mediator.Send(command);
                    return result.ToGenericResult();
                })
                .MapToApiVersion(1, 0)
                .AddEndpointFilter<ValidationFilter<CreateCourseCommand>>()
                .WithName("CreateCourse")
                .Produces<CreateCourseResponse>(StatusCodes.Status201Created)
                .Produces(StatusCodes.Status404NotFound)
                .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
                .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);


            return group;
        }
    }
}


// WithName ile endpoint'e isim veriyoruz, swagger'da daha anlamli bir isim gorunuyor fakat bunu swagger da filan goremiyoruz (gerekli degil)

// Produces ile bu endpointin 201 olustugunda id donecegini, 404 oldugunda ise hata donecegini belirtiyoruz. (endpointin neler donecegini belirtiyoruz)
// Client bu sayede 201 veya 404 ihtimallerini alacagini gorur
// bunlari diger endpointlerede best practice olarak ekleyebiliriz bu sayede client neler donebilecegini acik acik gorur