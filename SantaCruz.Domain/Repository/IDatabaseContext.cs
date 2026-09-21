using ErrorOr;
using System.Data;

namespace SantaCruz.Domain.Repository
{
    public interface IDatabaseContext
    {
        Task<ErrorOr<T>> Execute<T>(Func<Task<ErrorOr<T>>> action);
        Task<ErrorOr<T>> TransactionedOperation<T>(Func<Task<ErrorOr<T>>> action);
        IDbConnection Connection { get; }
    }
}
