using ErrorOr;
using SantaCruz.Domain.ViewModel.Page;
using SantaCruz.Domain.ViewModel.Product;

namespace SantaCruz.Domain.Service
{
    public interface IProductService
    {
        Task<ErrorOr<List<ProductListViewModel>>> List(PageViewModel page, CancellationToken cancellationToken);
    }
}
