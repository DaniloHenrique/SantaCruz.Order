using SantaCruz.Domain.Contract;

namespace SantaCruz.Domain.Model
{
    public class OrderItem : ILink<Order, Guid, Product, Guid>
    {
        public decimal Quantity { get; set; } = 0;
        public Order Order { get; set; } = new Order();
        public Product Product { get; set; } = new Product();
        public Guid IdOrder { get=> Order.Id; }
        public Guid IdProduct { get => Product.Id; }

    }
}
