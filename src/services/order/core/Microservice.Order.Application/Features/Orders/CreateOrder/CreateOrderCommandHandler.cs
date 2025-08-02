using MediatR;
using Microservice.Order.Application.Contracts.Repositories;
using Microservice.Order.Application.Contracts.UnitOfWork;
using Microservice.Order.Domain.Entities;
using Shared;
using Shared.Services;
using System.Net;

namespace Microservice.Order.Application.Features.Orders.CreateOrder;

public class CreateOrderCommandHandler(
    IOrderRepository orderRepository,
    IUnitOfWork unitOfWork,
    IIdentityService identityService) : IRequestHandler<CreateOrderCommand, ServiceResult>
{
    public async Task<ServiceResult> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {

        if (request.Items.Count == 0)
        {
            return ServiceResult.Error("Order items not found", "Order must have at least one item", HttpStatusCode.BadRequest);
        }
        
        
        Address newAddress = new Address()
        {
            Province = request.Address.Province,
            District = request.Address.District,
            Line = request.Address.Line,
            ZipCode = request.Address.ZipCode
        };
        
        var order = Domain.Entities.Order.CreateUnPaidOrder(identityService.GetUserId, request.DiscountRate, newAddress.Id);

        foreach (var orderItem in request.Items)
        {
            order.AddOrderItem(orderItem.ProductId, orderItem.ProductName, orderItem.UnitPrice);
        }
        
        order.Address = newAddress;
        orderRepository.Add(order);
        await unitOfWork.CommitAsync(cancellationToken);
        
        // Payment işlemleri yapılacak
        
        var paymentId = Guid.Empty;
        order.SetPaidStatus(paymentId);
        
        orderRepository.Update(order);
        await unitOfWork.CommitAsync(cancellationToken);
        return ServiceResult.SuccessAsNoContent();
    }
}