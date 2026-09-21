using SantaCruz.Domain.Contract;
using SantaCruz.Domain.Model;

namespace SantaCruz.Domain.Repository
{
    public interface IProductRepository: 
        IQuery<Product, Guid>, 
        ICreate<Product, Guid>,
        IDisposable
    {
    }
}
