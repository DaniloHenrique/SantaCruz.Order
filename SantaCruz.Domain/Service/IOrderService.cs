using ErrorOr;
using SantaCruz.Domain.ViewModel.Order;
using SantaCruz.Domain.ViewModel.Page;

namespace SantaCruz.Domain.Service
{
    public interface IOrderService
    {
        Task<ErrorOr<OrderViewModel>> Create(OrderCreateViewModel create, CancellationToken cancellationToken);
        Task<ErrorOr<OrderGetViewModel>> Get(Guid id, CancellationToken cancellationToken);
        Task<ErrorOr<List<OrderViewModel>>> List(PageViewModel page, CancellationToken cancellationToken);
        Task<ErrorOr<Created>> CreateProcessAttempt(Guid id, CancellationToken cancellationToken);
    }
}
