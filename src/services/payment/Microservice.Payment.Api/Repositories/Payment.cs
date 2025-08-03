using MassTransit;

namespace Microservice.Payment.Api.Repositories;

/// <summary>
///     Ödeme nesnesi: bir kullanıcıya ait, bir sipariş için yapılan ödeme işlemini temsil eder
/// </summary>
public class Payment
{
    // constructor method
    public Payment(Guid userId, string orderCode, decimal amount)
    {
        Create(userId, orderCode, amount);
    }

    public Guid Id { get; set; } // Ödemeye ait benzersiz ID
    public Guid UserId { get; set; } // Ödemeyi yapan kullanıcının ID'si
    public string OrderCode { get; set; } = null!; // Ödemenin ait olduğu sipariş kodu
    public DateTime Created { get; set; } // Ödemenin oluşturulma tarihi
    public decimal Amount { get; set; } // Ödeme miktarı
    public PaymentStatus Status { get; set; } // Ödeme durumu (Beklemede, Başarılı, Başarısız)


    private void Create(Guid userId, string orderCode, decimal amount)
    {
        Id = NewId.NextSequentialGuid(); // MassTransit ile sıralı GUID oluşturulur
        UserId = userId; // Kullanıcı ID atanır
        OrderCode = orderCode; // Sipariş kodu atanır
        Amount = amount; // Tutar atanır
        Status = PaymentStatus.Pending; // İlk durum olarak "Beklemede" atanır
        Created = DateTime.Now; // Oluşturulma tarihi atanır
    }

    public void SetStatus(PaymentStatus status)
    {
        Status = status;
    }
}

public enum PaymentStatus
{
    Success = 1,
    Failed = 2,
    Pending = 3
}