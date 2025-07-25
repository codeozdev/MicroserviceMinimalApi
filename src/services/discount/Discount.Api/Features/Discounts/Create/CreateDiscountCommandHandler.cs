using Discount.Api.Repositories;
using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared;
using Shared.Services;
using System.Net;

namespace Discount.Api.Features.Discounts.Create
{
    public class CreateDiscountCommandHandler(AppDbContext context, IIdentityService identityService) : IRequestHandler<CreateDiscountCommand, ServiceResult>
    {
        public async Task<ServiceResult> Handle(CreateDiscountCommand request, CancellationToken cancellationToken)
        {
            // daha önce bu id'li kullanıcı code eklemiş ise hata veriyoruz. Neden 400 dönüyoruz çünkü bu kullanıcının bir hatası yani "client" (code ayni olmamali)
            var hasCodeForUser = await context.Discounts.AnyAsync(x => x.UserId == request.UserId && x.Code == request.Code, cancellationToken: cancellationToken);

            if (hasCodeForUser)
            {
                return ServiceResult.Error("Discount code already exists for this user", HttpStatusCode.BadRequest);
            }

            var discount = new Discount()
            {
                Id = NewId.NextSequentialGuid(),
                Code = request.Code,
                Created = DateTime.UtcNow,
                Rate = request.Rate,
                Expired = request.Expired,
                UserId = request.UserId,
            };

            await context.Discounts.AddAsync(discount, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
            return ServiceResult.SuccessAsNoContent();
        }
    }
}



// token icinde user_id olmak zorunda