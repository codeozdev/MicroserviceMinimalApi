using AutoMapper;
using MediatR;
using Microservice.Order.Application.Contracts.Repositories;
using Microservice.Order.Application.Features.Orders.CreateOrder;
using Shared;
using Shared.Services;

namespace Microservice.Order.Application.Features.Orders.GetOrders;

/// <summary>
///     Bu handler, giriş yapan kullanıcının verdiği tüm siparişleri listelemek için kullanılır
/// </summary>
public class GetOrdersQueryHandler(IIdentityService identityService, IOrderRepository orderRepository, IMapper mapper)
    : IRequestHandler<GetOrdersQuery, ServiceResult<List<GetOrdersResponse>>>
{
    public async Task<ServiceResult<List<GetOrdersResponse>>> Handle(GetOrdersQuery request,
        CancellationToken cancellationToken)
    {
        // Kullanıcının ID'sine göre siparişleri getir
        List<Domain.Entities.Order> orders = await orderRepository.GetOrderByBuyerId(identityService.UserId);

        // Siparişleri DTO'ya dönüştür ve liste haline getir
        List<GetOrdersResponse> response = orders.Select(o =>
            new GetOrdersResponse(
                o.Created, // Siparişin oluşturulma tarihi
                o.TotalPrice, // Toplam tutar
                mapper.Map<List<OrderItemDto>>(o.OrderItems) // Sipariş kalemlerini DTO'ya map et
            )
        ).ToList();

        return ServiceResult<List<GetOrdersResponse>>.SuccessAsOk(response);
    }
}