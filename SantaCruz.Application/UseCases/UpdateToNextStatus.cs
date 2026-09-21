using SantaCruz.Domain.Chain;
using SantaCruz.Domain.Model;
using SantaCruz.Domain.Repository;
using SantaCruz.Domain.UseCases;

namespace SantaCruz.Application.UseCases
{
    public class UpdateToNextStatus(
        IDatabaseContext databaseContext,
        IOrderRepository orderRepository,
        ILogging logging
    ) : Log(logging), IUpdateToNextStatus
    {
        protected readonly IDatabaseContext _databaseContext = databaseContext;
        protected readonly IOrderRepository _orderRepository = orderRepository;
        protected bool _cancelled = false;

        public Order Order { get; set; } = new Order();

        private string GetOrderStatus()
        {
            return Order.Status switch
            {
                OrderStatus.Processing => "Processing",
                OrderStatus.Completed => "Completed",
                OrderStatus.Pending => "Pending",
                _ => "Failed",
            };
        }

        public virtual async Task<IStep> Next(CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                _cancelled = true;
                return Cancelled(nameof(UpdateToNextStatus));
            }

            var status = GetOrderStatus();

            Order.UpdatedAt = DateTime.Now;

            await _databaseContext.Execute(() => _orderRepository.UpdateStatus(Order, cancellationToken));

            Logging.Messages.Add($"Order {Order.Id} moved to {status}");

            return Logging;
        }
    }
}
