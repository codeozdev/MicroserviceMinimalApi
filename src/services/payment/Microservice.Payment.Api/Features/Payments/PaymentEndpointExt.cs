using Asp.Versioning.Builder;
using Microservice.Payment.Api.Features.Payments.Create;
using Microservice.Payment.Api.Features.Payments.GetAllPaymentsByUserId;

namespace Microservice.Payment.Api.Features.Payments;

public static class PaymentEndpointExt
{
    public static void AddPaymentGroupEndpointExt(this WebApplication app, ApiVersionSet apiVersionSet)
    {
        app.MapGroup("api/v{version:apiVersion}/payments").WithTags("payments").WithApiVersionSet(apiVersionSet)
            .CreatePaymentGroupItemEndpoint()
            .GetAllPaymentsByUserIdGroupItemEndpoint();
    }
}