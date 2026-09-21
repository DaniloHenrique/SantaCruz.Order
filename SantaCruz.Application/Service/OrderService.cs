using ErrorOr;
using SantaCruz.Domain.Model;
using SantaCruz.Domain.Repository;
using SantaCruz.Domain.Service;
using SantaCruz.Domain.ViewModel.Order;
using SantaCruz.Domain.ViewModel.Page;

namespace SantaCruz.Application.Service
{
    public class OrderService(
        IOrderRepository orderRepository,
        IOrderItemRepository orderItemRepository,
        IOrderProcessingAttemptRepository orderProcessingAttemptRepository,
        IProductRepository productRepository,
        IDatabaseContext databaseContext
    ) : 
        BaseService(databaseContext), 
        IOrderService
    {
        private readonly IOrderRepository _orderRepository = orderRepository;
        private readonly IOrderItemRepository _orderItemRepository = orderItemRepository;
        private readonly IOrderProcessingAttemptRepository _orderProcessingAttemptRepository = orderProcessingAttemptRepository;
        private readonly IProductRepository _productRepository = productRepository;


        public Task<ErrorOr<OrderViewModel>> Create(OrderCreateViewModel create, CancellationToken cancellationToken) =>
            Context.TransactionedOperation<OrderViewModel>(async () =>
            {
                var items = create.Items;

                if (items.Count == 0) return Error.Validation(description: "Order must have at least one item", code: "EmptyOrder");

                var order = new Order
                {
                    Id = Guid.NewGuid(),
                    Status = OrderStatus.Pending
                };

                var orderResult = await _orderRepository.Create(order, cancellationToken);

                if (orderResult.IsError) return orderResult.FirstError;

                foreach (var item in items)
                {
                    if (item.Quantity<1) return Error.Validation("ItemZeroQuantity",$"Item {item.ProductId} must have at least one quantity");

                    var productResult = await _productRepository.Get(item.ProductId, cancellationToken);

                    if (productResult.IsError) return Error.NotFound(description:$"Product {item.ProductId} not found", code:"ProductNotFound");

                    var product = productResult.Value;

                    var orderItem = new OrderItem{
                        Order = order,
                        Product = product,
                        Quantity = item.Quantity
                    };

                    var orderItemResult = await _orderItemRepository.Create(orderItem, cancellationToken);

                    if (orderItemResult.IsError) return orderItemResult.FirstError;
                }

                return new OrderViewModel(order);
            });
        public Task<ErrorOr<Created>> CreateProcessAttempt(Guid id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
        public Task<ErrorOr<OrderGetViewModel>> Get(Guid id, CancellationToken cancellationToken) =>
            Context.Execute<OrderGetViewModel>(async () =>
            {
                var orderResult = await _orderRepository.Get(id, cancellationToken);

                if (orderResult.IsError) return orderResult.FirstError;

                var order = orderResult.Value;

                var items = await _orderItemRepository.ListItensByOrder(order);

                if (items.IsError) return items.FirstError;

                return new OrderGetViewModel(order, items.Value);
            });
        public Task<ErrorOr<List<OrderViewModel>>> List(PageViewModel page, CancellationToken cancellationToken)=>
            Context.Execute<List<OrderViewModel>>(async () =>
            {
                var result = await _orderRepository.List(page.Page, page.PageSize,cancellationToken);

                if (result.IsError) return result.FirstError;

                var orders = result.Value;

                return orders
                    .Select(order => new OrderViewModel(order))
                    .ToList();
            });
    }
}
