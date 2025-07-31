using MassTransit;
using System.Text;

namespace Microservice.Order.Domain.Entities;

/// <summary>
/// Sipariş nesnesi: Alıcı, adres, ürünler ve ödeme gibi temel sipariş bilgilerini tutar.
/// </summary>
public class Order : BaseEntity<Guid>
{
    // Siparişin sistemdeki rastgele üretilmiş 10 haneli kodu
    public string Code { get; set; } = null!;

    // Siparişin oluşturulduğu tarih
    public DateTime Created { get; set; }

    // Siparişi veren kullanıcının ID'si
    public Guid BuyerId { get; set; }

    // Siparişin güncel durumu (Bekleniyor, Ödendi, vb.)
    public OrderStatus Status { get; set; }

    // Siparişin gönderileceği adresin ID'si
    public int AddressId { get; set; }

    // Siparişin toplam fiyatı (varsa indirim uygulanmış hali)
    public decimal TotalPrice { get; set; }

    // Uygulanacak indirim yüzdesi (isteğe bağlı)
    public float? DiscountRate { get; set; }

    // Ödeme gerçekleştiyse, ilgili ödeme işleminin ID'si
    public Guid? PaymentId { get; set; }

    // Siparişe ait ürün kalemlerinin listesi
    public List<OrderItem> OrderItems { get; set; } = [];

    // Siparişin gönderileceği adres (navigasyon properti)
    public Address Address { get; set; } = null!;


    // 10 haneli rastgele sipariş kodu üretir
    public static string GenerateCode()
    {
        var random = new Random();
        var orderCode = new StringBuilder(10);
        for (var i = 0; i < 10; i++) orderCode.Append(random.Next(0, 10));
        return orderCode.ToString();
    }

    // Adres ID'si belirtilerek henüz ödenmemiş bir sipariş oluşturur
    public static Order CreateUnPaidOrder(Guid buyerId, float? disCountRate, int addressId)
    {
        return new Order
        {
            Id = NewId.NextGuid(),
            Code = GenerateCode(),
            BuyerId = buyerId,
            Created = DateTime.Now,
            Status = OrderStatus.WaitingForPayment,
            AddressId = addressId,
            DiscountRate = disCountRate,
            TotalPrice = 0
        };
    }

    // Adres ID'si belirtilmeden henüz ödenmemiş bir sipariş oluşturur
    public static Order CreateUnPaidOrder(Guid buyerId, float? disCountRate)
    {
        return new Order
        {
            Id = NewId.NextGuid(),
            Code = GenerateCode(),
            BuyerId = buyerId,
            Created = DateTime.Now,
            Status = OrderStatus.WaitingForPayment,
            DiscountRate = disCountRate,
            TotalPrice = 0
        };
    }

    // Siparişe bir ürün kalemi ekler, indirim varsa uygular ve toplam tutarı günceller
    public void AddOrderItem(Guid productId, string productName, decimal unitPrice)
    {
        var orderItem = new OrderItem();

        if (DiscountRate.HasValue)
        {
            unitPrice -= unitPrice * (decimal)DiscountRate.Value / 100;
        }

        orderItem.SetItem(productId, productName, unitPrice);
        OrderItems.Add(orderItem);
        CalculateTotalPrice();
    }

    // Siparişe indirim oranı uygular ve toplam fiyatı günceller
    public void ApplyDiscount(float discountPercentage)
    {
        if (discountPercentage < 0 || discountPercentage > 100)
        {
            throw new ArgumentOutOfRangeException(
                nameof(discountPercentage),
                discountPercentage,
                "Discount percentage must be between 0 and 100");
        }

        DiscountRate = discountPercentage;
        CalculateTotalPrice();
    }

    // Siparişin durumunu "Ödendi" yapar ve ödeme ID'sini kaydeder
    public void SetPaidStatus(Guid paymentId)
    {
        Status = OrderStatus.Paid;
        PaymentId = paymentId;
    }

    // Sipariş kalemlerinin toplam tutarını yeniden hesaplar
    private void CalculateTotalPrice()
    {
        TotalPrice = OrderItems.Sum(x => x.UnitPrice);
    }
}


// Order 1 fakat birden fazla adress alabilir (bire cok iliski)