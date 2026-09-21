using ErrorOr;
using SantaCruz.Domain.Contract;
using SantaCruz.Domain.Model;

namespace SantaCruz.Domain.Repository
{
    public interface IOrderItemRepository: 
        ILinked<OrderItem, Order, Guid, Product, Guid>,
        IDisposable
    {
        Task<ErrorOr<List<OrderItem>>> ListItensByOrder(Order order, CancellationToken cancellationToken = default);
    }
}
