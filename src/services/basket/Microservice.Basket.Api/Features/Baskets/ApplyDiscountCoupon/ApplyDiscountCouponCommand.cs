using Shared;

namespace Microservice.Basket.Api.Features.Baskets.ApplyDiscountCoupon;

public record ApplyDiscountCouponCommand(string Coupon, float DiscountRate) : IRequestByServiceResult;