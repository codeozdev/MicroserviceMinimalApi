using AutoMapper;
using Microservice.Order.Application.Features.Orders.CreateOrder;
using Microservice.Order.Domain.Entities;

namespace Microservice.Order.Application.Features.Orders;

public class OrderMapping : Profile
{
    public OrderMapping()
    {
        CreateMap<OrderItem, OrderItemDto>().ReverseMap();
    }
}