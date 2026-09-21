using SantaCruz.Domain.Contract;

namespace SantaCruz.Domain.Model
{
    public class Order:IEntity<Guid>
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; } = null;
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public List<OrderItem> Items { get; set; } = [];

    }
}
