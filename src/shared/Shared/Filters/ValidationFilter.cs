using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Shared.Filters;

public class ValidationFilter<T> : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {

        // Burdaki kodlar endpointe girmed once input validation yapar.
        var validator = context.HttpContext.RequestServices.GetService<IValidator<T>>(); // CreateCategoryCommandValidator.cs sinifi var mi kontrolunu saglar varsa endpoint calismadan once bu input kontrollerini saglar

        if (validator is null)
        {
            return await next(context);
        }

        var requestModel = context.Arguments.OfType<T>().FirstOrDefault();


        if (requestModel is null)
        {
            return await next(context);
        }

        var validateResult = await validator.ValidateAsync(requestModel);

        if (!validateResult.IsValid)
        {
            return Results.ValidationProblem(validateResult.ToDictionary());
        }


        return await next(context);
    }
}



// Minimal api de 5 tane filter yoktur daha azdir
// endpointimize girmeden once filter devreye girebilir veya endpointimizden ciktiktan sonra filter devreye girebilir.

// CreateCategoryEndpoint.cs ekledik ve filterimiz tum category endpointlerine uygulanmis oldu. minimal api olmasaydi direk kendisi devreye girecekti.


// FluentValidation kullanicinin yani input ile alakali dogrulamalar oldugu icin sadece Post, Put gibi endpointlerde kullanilir.