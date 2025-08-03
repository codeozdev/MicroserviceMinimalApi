using MediatR;
using Microservice.Payment.Api.Repositories;
using Shared;
using Shared.Services;
using System.Net;

namespace Microservice.Payment.Api.Features.Payments.Create;

/// <summary>
///     Amaç: Kullanıcıdan gelen kart bilgileriyle ödeme yapmak ve sonucu dönmek.
/// </summary>
public class CreatePaymentCommandHandler(AppDbContext context, IIdentityService idenIdentityService)
    : IRequestHandler<CreatePaymentCommand, ServiceResult<Guid>>
{
    public async Task<ServiceResult<Guid>> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
    {
        // Dış ödeme servisi simülasyonu (gerçekte bankaya API çağrısı yapılır)
        var (isSuccess, errorMessage) = await ExternalPaymentProcessAsync(
            request.CardNumber,
            request.CardHolderName,
            request.CardExpirationDate,
            request.CardSecurityNumber,
            request.Amount);

        // Ödeme başarısızsa, hata mesajı ile birlikte bad request döner
        if (!isSuccess)
        {
            return ServiceResult<Guid>.Error("Payment Failed", errorMessage!, HttpStatusCode.BadRequest);
        }

        // Kullanıcının ID'si alınır
        Guid userId = idenIdentityService.GetUserId;

        // Yeni ödeme nesnesi oluşturulur
        Repositories.Payment newPayment = new(userId, request.OrderCode, request.Amount);
        // Durumu başarılı olarak ayarlanır
        newPayment.SetStatus(PaymentStatus.Success);

        // Veritabanına eklenir ve kaydedilir
        context.Payments.Add(newPayment);
        await context.SaveChangesAsync(cancellationToken);

        return ServiceResult<Guid>.SuccessAsOk(newPayment.Id);
    }


    // Dış ödeme servisinin simülasyonu
    private static async Task<(bool isSuccess, string? errorMessage)> ExternalPaymentProcessAsync(
        string cardNumber,
        string cardHolderName,
        string cardExpirationDate,
        string cardSecurityNumber,
        decimal amount)
    {
        // Gecikme simülasyonu
        await Task.Delay(1000);
        return (true, null);
    }
}