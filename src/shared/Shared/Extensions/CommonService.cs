using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using Shared.Services;

namespace Shared.Extensions;

public static class CommonService
{
    public static IServiceCollection AddCommonServiceExt(this IServiceCollection service, Type assembly)
    {
        service.AddHttpContextAccessor(); // Controller disinda HTTP request bilgilerine ulasabilmek icin gerekli (Controller kullanmiyoruz gerekli burasi)
        service.AddMediatR(x => x.RegisterServicesFromAssemblyContaining(assembly));

        // FluentValidation
        service.AddFluentValidationAutoValidation();
        service.AddValidatorsFromAssemblyContaining(assembly);

        // Mapper
        service.AddAutoMapper(assembly);

        // fake identity service
        service.AddScoped<IIdentityService, IdentityServiceFake>();

        return service;
    }
}

// SHARED'IN EXTENSIONS SINIFIDIR