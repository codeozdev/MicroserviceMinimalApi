using Discount.Api.Repositories;
using MassTransit;
using MediatR;
using Shared;
using Shared.Services;

namespace Discount.Api.Features.Discounts.Create
{
    public class CreateDiscountCommandHandler(AppDbContext context, IIdentityService identityService) : IRequestHandler<CreateDiscountCommand, ServiceResult>
    {
        public async Task<ServiceResult> Handle(CreateDiscountCommand request, CancellationToken cancellationToken)
        {
            var discount = new Discount()
            {
                Id = NewId.NextSequentialGuid(),
                Code = request.Code,
                Created = DateTime.Now,
                Rate = request.Rate,
                Expired = request.Expired,
                UserId = identityService.GetUserId
            };

            await context.Discounts.AddAsync(discount, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
            return ServiceResult.SuccessAsNoContent();
        }
    }
}



// token icinde user_id olmak zorunda