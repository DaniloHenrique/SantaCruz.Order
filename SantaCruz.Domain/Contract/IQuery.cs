using ErrorOr;
using SantaCruz.Domain.ViewModel.Page;

namespace SantaCruz.Domain.Contract
{
    public interface IQuery<TEntity, TKey> 
        where TKey: unmanaged
        where TEntity : IEntity<TKey>
    {
        Task<ErrorOr<List<TEntity>>> List(int page, int pageSize, CancellationToken cancellationToken);
        Task<ErrorOr<TEntity>> Get(TKey id, CancellationToken cancellationToken);
    }
}
