using Shared;

namespace Microservice.Payment.Api.Features.Payments.GetAllPaymentsByUserId;

public record GetAllPaymentsByUserIdQuery : IRequestByServiceResult<List<GetAllPaymentsByUserIdResponse>>;