using SantaCruz.Domain.Contract;

namespace SantaCruz.Domain.Model
{
    public class Product: IEntity<Guid>
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; } = 0;
    }
}
