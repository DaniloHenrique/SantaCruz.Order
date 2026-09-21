using Dapper;
using ErrorOr;
using SantaCruz.Domain.Model;
using SantaCruz.Domain.Repository;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace SantaCruz.Infrastructure.NpgDbRepository
{
    public class OrderProcessingAttemptRepository(IDatabaseContext context) : 
        DatabaseRepository(context), 
        IOrderProcessingAttemptRepository
    {
        public async Task<ErrorOr<Created>> Create(OrderProcessingAttempt entity, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested) cancellationToken.ThrowIfCancellationRequested();

            const string sql = @"INSERT INTO 
            OrderProcessingAttempt (idOrder, attemptNumber, startedAt, finishedAt, success, errorMessage) 
            VALUES (@IdOrder, @AttemptNumber, @StartedAt, @FinishedAt, @Success, @ErrorMessage)";

            var result = await Context.Connection
                .ExecuteAsync(
                    sql,
                    new
                    {
                        IdOrder = entity.Order.Id,
                        entity.AttemptNumber,
                        entity.StartedAt,
                        entity.FinishedAt,
                        entity.Success,
                        entity.ErrorMessage
                    },
                    commandType: CommandType.Text
                );

            return result > 0 
                ? Result.Created 
                : Error.Conflict("ProcessingConflit","OrderProcessingAttempt already exists");
        }
        public async Task<ErrorOr<OrderProcessingAttempt>> Get(int id, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested) cancellationToken.ThrowIfCancellationRequested();

            const string sql = @"SELECT 
                opa.Id, opa.AttemptNumber, opa.StartedAt, opa.FinishedAt, opa.Success,
                o.id, o.createdAt, o.updatedAt, o.status    
            FROM OrderProcessingAttempt opa 
            LEFT JOIN ""order"" o ON o.id = opa.idOrder
            WHERE opa.Id = @Id";

            var list =await Context.Connection
                .QueryAsync<OrderProcessingAttempt, Order,OrderProcessingAttempt>(
                    sql,
                    (opa, o) => { 
                        opa.Order = o; 

                        return opa; 
                    },
                    new { Id = id },
                    commandType: CommandType.Text
                );

            var result = list.FirstOrDefault();

            if (result is null) return Error.NotFound("NotFound","OrderProcessingAttempt not found");

            return result;
        }
        public async Task<ErrorOr<List<OrderProcessingAttempt>>> List(int page, int pageSize, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested) cancellationToken.ThrowIfCancellationRequested();

            const string sql = @"SELECT 
                opa.Id, opa.AttemptNumber, opa.StartedAt, opa.FinishedAt, opa.Success,
                o.id, o.createdAt, o.updatedAt, o.status    
            FROM OrderProcessingAttempt opa 
            LEFT JOIN ""order"" o ON o.id = opa.idOrder
            LIMIT @PageSize OFFSET @Page";

            var result = await Context.Connection
                .QueryAsync<OrderProcessingAttempt, Order,OrderProcessingAttempt>(
                    sql,
                    (opa, o) => { 
                        opa.Order = o; 

                        return opa; 
                    },
                    new { Page = (page - 1) * pageSize, PageSize = pageSize },
                    commandType: CommandType.Text
                );  

            return result.ToList();
        }
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public async Task<ErrorOr<List<OrderProcessingAttempt>>> ListByOrder(Guid id, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested) cancellationToken.ThrowIfCancellationRequested();

            const string sql = @"SELECT 
                opa.Id, opa.AttemptNumber, opa.StartedAt, opa.FinishedAt, opa.Success,
                o.id, o.createdAt, o.updatedAt, o.status    
            FROM OrderProcessingAttempt opa 
            LEFT JOIN ""order"" o ON o.id = opa.idOrder
            WHERE opa.idOrder = @Id";

            var result = await Context.Connection
                .QueryAsync<OrderProcessingAttempt, Order,OrderProcessingAttempt>(
                    sql,
                    (opa, o) => {
                        opa.Order = o;

                        return opa;
                    },
                    new { Id = id },
                    commandType: CommandType.Text
                );

            return result.ToList();
        }

        public async Task<ErrorOr<int>> GetMaxAttemptByOrder(Guid id, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested) cancellationToken.ThrowIfCancellationRequested();

            const string sql = @"SELECT COALESCE(MAX(attemptNumber),0) FROM OrderProcessingAttempt WHERE idOrder = @Id";

            var max = await Context.Connection
                .QueryFirstOrDefaultAsync<int>(
                    sql,
                    new { Id = id },
                    commandType: CommandType.Text
                );

            return max;
        }
    }
}
