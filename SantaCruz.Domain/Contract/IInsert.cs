using ErrorOr;

namespace SantaCruz.Domain.Contract
{
    public interface IInsert<TEntity,TKey>
        where TKey : unmanaged
        where TEntity : class, IEntity<TKey>
    {
        Task<ErrorOr<TEntity>> Insert(TEntity entity, CancellationToken cancellationToken = default);
    }
}
