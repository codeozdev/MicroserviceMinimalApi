using Microservice.Payment.Api.Repositories;

namespace Microservice.Payment.Api.Features.Payments.GetAllPaymentsByUserId;

public record GetAllPaymentsByUserIdResponse(
    Guid Id,
    string OrderCode,
    string Amount,
    DateTime Created,
    PaymentStatus Status
);