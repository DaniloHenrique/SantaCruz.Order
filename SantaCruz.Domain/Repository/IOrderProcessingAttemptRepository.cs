using ErrorOr;
using SantaCruz.Domain.Contract;
using SantaCruz.Domain.Model;

namespace SantaCruz.Domain.Repository
{
    public interface IOrderProcessingAttemptRepository: 
        IQuery<OrderProcessingAttempt, int>, 
        ICreate<OrderProcessingAttempt, int>,
        IDisposable
    {
        Task<ErrorOr<List<OrderProcessingAttempt>>> ListByOrder(Guid id, CancellationToken cancellationToken);
        Task<ErrorOr<int>> GetMaxAttemptByOrder(Guid id, CancellationToken cancellationToken);
    }
}
