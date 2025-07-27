namespace Microservice.Basket.Api.Dto;

public record BasketItemDto(
    Guid Id,
    string Name,                      // Kursun adı
    string ImageUrl,
    decimal Price,
    decimal? PriceByApplyDiscountRate // İndirim oranına göre hesaplanmış fiyat
);


// Kurs ile ilgili bilgileri tutan DTO
// kullanicinin gonderdigi AddBasketItemCommand'da gelen bilgileri buradaki DTO icine dolduruyoruz