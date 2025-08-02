namespace Microservice.Order.Application.Contracts.UnitOfWork;

public interface IUnitOfWork
{
    // Veritabanı değişikliklerini kaydeder
    Task<int> CommitAsync(CancellationToken cancellationToken = default);
    
    
    
    // BeginTransactionAsync + CommitTransactionAsync** - Karmaşık İşlemler İçin
    // Yeni bir transaction başlatır
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);
    
    // Transaction'ı commit eder
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);
}

