using MediatR;
using Microservice.Payment.Api.Repositories;
using Microsoft.EntityFrameworkCore;
using Shared;
using Shared.Services;

namespace Microservice.Payment.Api.Features.Payments.GetAllPaymentsByUserId;

/// <summary>
///     Bu handler, giriş yapan kullanıcının tüm ödeme geçmişini listelemek için kullanılır.
/// </summary>
public class GetAllPaymentsByUserIdQueryHandler(AppDbContext context, IIdentityService identityService)
    : IRequestHandler<GetAllPaymentsByUserIdQuery, ServiceResult<List<GetAllPaymentsByUserIdResponse>>>
{
    public async Task<ServiceResult<List<GetAllPaymentsByUserIdResponse>>> Handle(GetAllPaymentsByUserIdQuery request,
        CancellationToken cancellationToken)
    {
        // Kullanıcının ID'si alınır
        Guid userId = identityService.GetUserId;

        // Veritabanında ilgili kullanıcıya ait tüm ödemeler sorgulanır ve response modeline dönüştürülür
        List<GetAllPaymentsByUserIdResponse> payments = await context.Payments
            .Where(x => x.UserId == userId) // Sadece giriş yapan kullanıcıya ait kayıtlar
            .Select(x => new GetAllPaymentsByUserIdResponse(
                x.Id, // Ödeme ID
                x.OrderCode, // Sipariş kodu
                x.Amount.ToString("C"), // Tutar (para birimi formatında)
                x.Created, // Oluşturulma tarihi
                x.Status // Ödeme durumu
            )).ToListAsync(cancellationToken); // Asenkron şekilde listeye dönüştürülür

        return ServiceResult<List<GetAllPaymentsByUserIdResponse>>.SuccessAsOk(payments);
    }
}