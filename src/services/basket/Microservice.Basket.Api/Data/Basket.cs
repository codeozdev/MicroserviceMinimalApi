using System.Text.Json.Serialization;

namespace Microservice.Basket.Api.Data;

public class Basket
{
    public Guid UserId { get; set; } // Sepetin ait olduğu kullanıcı ID'si
    public List<BasketItem> Items { get; set; } = []; // Sepetteki ürünler listesi
    public float? DiscountRate { get; set; } // Uygulanan indirim oranı
    public string? Coupon { get; set; } // Kullanılan kupon kodu (varsa)


    [JsonIgnore] public bool IsApplyDiscount => DiscountRate is > 0 && !string.IsNullOrEmpty(Coupon); // Kupon kodu ve indirim oranı varsa indirim uygulanabilir demektir
    [JsonIgnore] public decimal TotalPrice => Items.Sum(x => x.Price);     // Sepetteki ürünlerin toplam fiyatı (indirim uygulanmadan)


    [JsonIgnore]
    public decimal? TotalPriceWithAppliedDiscount => // İndirim uygulanmış toplam fiyat, indirim yoksa null döner
        !IsApplyDiscount ? null : Items.Sum(x => x.PriceByApplyDiscountRate);


    public Basket(Guid userId, List<BasketItem> items)
    {
        UserId = userId;
        Items = items;
    }

    public Basket() { }



    public void ApplyNewDiscount(string coupon, float discountRate) // Yeni bir indirim uygular (kupon kodu ve indirim oranı)
    {
        Coupon = coupon;
        DiscountRate = discountRate;


        foreach (var basket in Items)
        {
            basket.PriceByApplyDiscountRate = basket.Price * (decimal)(1 - discountRate);
        }
    }

    public void ApplyAvailableDiscount() // Mevcut kupon ve oranla indirim uygular (kupon daha önce set edilmiş olmalı)
    {
        if (!IsApplyDiscount)
        {
            return;
        }

        foreach (var basket in Items)
        {
            basket.PriceByApplyDiscountRate = basket.Price * (decimal)(1 - DiscountRate!); // İndirimli fiyat yeniden hesaplanır
        }
    }

    public void ClearDiscount() // Sepetteki indirimleri sıfırlar (kupon ve oran dahil)
    {
        DiscountRate = null;
        Coupon = null;
        foreach (var basket in Items)
        {
            basket.PriceByApplyDiscountRate = null; // İndirimli fiyat bilgisi temizlenir
        }
    }
}


// Normalde Basket Service yazarız fakat DDD gibi ilgili kodu ilgili yerde yazıyoruz.
// Neden bunları dto içine yazmayıp buraya yazdık çünkü dtolar sadece dönüş tipleri tutulsun diye yapıldı başka espirisi yok.


/*
  DDD’de ise domain (alan) odaklı düşünürüz.
   
       “Bir sepet kendi iç kurallarını kendi bilmeli ve kendi uygulamalı.”
   
   Bu yüzden Basket sınıfı sadece veri değil, aynı zamanda iş kurallarını da içerir.
   Yani indirim uygulama, temizleme gibi davranışlar Basket nesnesinin sorumluluğudur.
 */