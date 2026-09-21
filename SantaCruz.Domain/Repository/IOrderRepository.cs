using ErrorOr;
using SantaCruz.Domain.Contract;
using SantaCruz.Domain.Model;

namespace SantaCruz.Domain.Repository
{
    public interface IOrderRepository: 
        IQuery<Order,Guid>, 
        ICreate<Order,Guid>,
        IDisposable
    {
        Task<ErrorOr<Updated>> UpdateStatus(Order order, CancellationToken cancellationToken = default);
        Task<ErrorOr<List<Order>>> ListOrderByStatus(OrderStatus status, CancellationToken cancellationToken = default);
    }
}
