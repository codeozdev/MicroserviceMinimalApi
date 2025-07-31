namespace Microservice.Order.Domain.Entities;

public enum OrderStatus
{
    WaitingForPayment = 1,
    Paid = 2,
    Canceled = 3
}