namespace SantaCruz.Domain.ViewModel.Order
{
    public record OrderItemListViewModel(Guid ProductId, string Name, string Description, decimal Price, decimal Quantity);
}
