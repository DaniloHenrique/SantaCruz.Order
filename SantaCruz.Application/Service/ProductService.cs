using ErrorOr;
using SantaCruz.Domain.Repository;
using SantaCruz.Domain.Service;
using SantaCruz.Domain.ViewModel.Page;
using SantaCruz.Domain.ViewModel.Product;

namespace SantaCruz.Application.Service
{
    public class ProductService(
        IProductRepository productRepository, 
        IDatabaseContext context
    ) : 
        BaseService(context), 
        IProductService
    {
        protected readonly IProductRepository _productRepository = productRepository;

        public Task<ErrorOr<List<ProductListViewModel>>> List(PageViewModel page, CancellationToken cancellationToken = default)=>
            Context.Execute<List<ProductListViewModel>>(async () =>
            {
                var result = await _productRepository
                    .List(page.Page, page.PageSize, cancellationToken);

                if (result.IsError) return result.FirstError;

                var products = result.Value;

                return products
                    .Select(product => new ProductListViewModel(product))
                    .ToList();
            });
    }
}
