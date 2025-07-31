namespace Microservice.Order.Domain.Entities;

/// <summary>
/// Sipariş içerisindeki tek bir ürün kalemini temsil eder.
/// </summary>
public class OrderItem : BaseEntity<int>
{
    // Ürüne ait benzersiz ID
    public Guid ProductId { get; set; }

    // Ürün adı (boş olamaz)
    public string ProductName { get; set; } = null!;

    // Birim fiyat (indirimli ya da indirimsiz)
    public decimal UnitPrice { get; set; }

    // Bu ürünün ait olduğu siparişin ID'si (foreign key)
    public Guid OrderId { get; set; }

    // Navigasyon properti: bu ürünün ait olduğu sipariş
    public Order Order { get; set; } = null!;


    // Ürüne ait bilgileri set ederken doğrulama da yapar
    public void SetItem(Guid productId, string productName, decimal unitPrice)
    {
        if (string.IsNullOrEmpty(productName))
        {
            throw new ArgumentNullException(nameof(productName), "Product name cannot be null or empty");
        }

        if (unitPrice <= 0)
        {
            throw new ArgumentException("Unit price must be greater than zero", nameof(unitPrice));
        }

        ProductId = productId;
        ProductName = productName;
        UnitPrice = unitPrice;
    }

    // Ürün fiyatını günceller (fiyat sıfır ya da negatif olamaz)
    public void UpdatePrice(decimal unitPrice)
    {
        if (unitPrice <= 0)
        {
            throw new ArgumentException("Unit price must be greater than zero", nameof(unitPrice));
        }

        UnitPrice = unitPrice;
    }

    // Ürüne indirim uygular (direkt fiyat düşürülür)
    public void ApplyDiscount(decimal discount)
    {
        if (discount <= 0)
        {
            throw new ArgumentException("Discount must be greater than zero", nameof(discount));
        }

        UnitPrice -= discount;
    }

    // Verilen başka bir OrderItem ile aynı ürünü temsil edip etmediğini kontrol eder
    public bool IsSameItem(OrderItem otherItem)
    {
        return ProductId == otherItem.ProductId;
    }
}


// Anomic Model ⇒ Rich Domain Model (Domain Divine Design)