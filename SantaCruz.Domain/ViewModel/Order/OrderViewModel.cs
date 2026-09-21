using Model = SantaCruz.Domain.Model;

namespace SantaCruz.Domain.ViewModel.Order
{
    public record OrderViewModel
    {
        public OrderViewModel(Model.Order order)
        {
            OrderId = order.Id;
            Status = order.Status switch
            {
                Model.OrderStatus.Pending => "Pending",
                Model.OrderStatus.Processing => "Processing",
                Model.OrderStatus.Completed => "Completed",
                Model.OrderStatus.Failed => "Failed",
                _ => "Unknown",
            };
        }
        public Guid OrderId { get; }
        public string Status { get; }
    }
}
