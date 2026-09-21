namespace SantaCruz.Domain.ViewModel.Order
{
    public record OrderGetViewModel
    {
        public OrderGetViewModel(Model.Order order,List<Model.OrderItem> items)
        {
            Status = order.Status switch
            {
                Model.OrderStatus.Pending => "Pending",
                Model.OrderStatus.Processing => "Processing",
                Model.OrderStatus.Completed => "Completed",
                Model.OrderStatus.Failed => "Failed",
                _ => "Unknown",
            };
            Id = order.Id;
            CreatedAt = order.CreatedAt;
            UpdatedAt = order.UpdatedAt;
            Items = [.. items.Select(
                item => new OrderItemListViewModel(
                    item.IdProduct,
                    item.Product.Name,
                    item.Product.Description,
                    item.Product.Price,
                    item.Quantity
                )
            )];
        }

        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string Status { get; set; }
        public List<OrderItemListViewModel> Items { get; set; }
    }
}
