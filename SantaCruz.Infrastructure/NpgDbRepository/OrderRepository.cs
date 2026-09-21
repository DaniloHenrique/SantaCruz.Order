using Dapper;
using ErrorOr;
using SantaCruz.Domain.Model;
using SantaCruz.Domain.Repository;
using System.Data;

namespace SantaCruz.Infrastructure.NpgDbRepository
{
    public class OrderRepository(IDatabaseContext context) : 
        DatabaseRepository(context), 
        IOrderRepository
    {
        public async Task<ErrorOr<Created>> Create(Order entity, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested) cancellationToken.ThrowIfCancellationRequested();

            const string sql = "INSERT INTO \"order\" (id, status) VALUES (@Id, @Status)";

            var result = await Context.Connection
                .ExecuteAsync(
                    sql,
                    new
                    {
                        entity.Id,
                        entity.Status
                    },
                    commandType: CommandType.Text
                );

            return result > 0 
                ? Result.Created 
                : Error.Conflict(description:$"Order {entity.Id} already exists", code:"OrderAlreadyExists");
        }
        public async Task<ErrorOr<Order>> Get(Guid id, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested) cancellationToken.ThrowIfCancellationRequested();

            const string sql = "SELECT id,createdAt, updatedAt, status FROM \"order\" WHERE id = @Id";

            var result = await Context.Connection
                .QueryFirstOrDefaultAsync<Order>(
                    sql,
                    new { Id = id },
                    commandType: CommandType.Text
                );  

            if (result is null) return Error.NotFound(description: $"Order {id} not found", code: "OrderNotFound");

            return result;
        }
        public async Task<ErrorOr<List<Order>>> List(int page, int pageSize, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested) cancellationToken.ThrowIfCancellationRequested();

            const string sql = "SELECT id, createdAt, updatedAt, status FROM \"order\" LIMIT @PageSize OFFSET @Page";

            var result = await Context.Connection
                .QueryAsync<Order>(
                    sql,
                    new { Page = (page - 1) * pageSize, PageSize = pageSize },
                    commandType: CommandType.Text
                );

            return result.ToList();
        }


        public async Task<ErrorOr<Updated>> UpdateStatus(Order order, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested) cancellationToken.ThrowIfCancellationRequested();

            const string sql = "UPDATE \"order\" SET status = @Status, updatedAt = @UpdatedAt WHERE id = @Id";

            var result = await Context.Connection
                .ExecuteAsync(
                    sql,
                    new
                    {
                        order.Status,
                        order.UpdatedAt,
                        order.Id
                    },
                    commandType: CommandType.Text
                );

            return result > 0 
                ? Result.Updated 
                : Error.NotFound(description:$"Order {order.Id} not found", code:"OrderNotFound");
        }
        public async Task<ErrorOr<List<Order>>> ListOrderByStatus(OrderStatus status, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested) cancellationToken.ThrowIfCancellationRequested();

            const string sql = "SELECT id FROM \"order\" WHERE status = @Status";

            var result = await Context.Connection
                .QueryAsync<Order>(
                    sql,
                    new { Status = status },
                    commandType: CommandType.Text
                );

            if (result is null) return Error.NotFound(description: $"Order {status} not found", code: "OrderNotFound");

            return result.ToList();
        }
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }


    }
}
