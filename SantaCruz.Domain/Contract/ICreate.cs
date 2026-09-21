using ErrorOr;

namespace SantaCruz.Domain.Contract
{
    public interface ICreate<TEntity,TKey> 
        where TKey: unmanaged
        where TEntity : IEntity<TKey>
    {
        Task<ErrorOr<Created>> Create(TEntity entity, CancellationToken cancellationToken = default);
    }
}
