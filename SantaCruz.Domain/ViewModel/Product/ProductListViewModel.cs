using Model = SantaCruz.Domain.Model;

namespace SantaCruz.Domain.ViewModel.Product
{
    public record ProductListViewModel
    {
        public ProductListViewModel(Model.Product product)
        {
            ProductId = product.Id;
            Name = product.Name;
            Description = product.Description;
            Price = product.Price;
        }

        public Guid ProductId { get; }
        public string Name { get; } = string.Empty;
        public string Description { get; } = string.Empty;
        public decimal Price { get; } = decimal.Zero;

    }
}
