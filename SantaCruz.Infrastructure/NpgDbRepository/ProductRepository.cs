using Dapper;
using ErrorOr;
using SantaCruz.Domain.Model;
using SantaCruz.Domain.Repository;
using System.Data;

namespace SantaCruz.Infrastructure.NpgDbRepository
{
    public class ProductRepository(IDatabaseContext context) : 
        DatabaseRepository(context), 
        IProductRepository
    {
        public async Task<ErrorOr<Created>> Create(Product entity, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested) cancellationToken.ThrowIfCancellationRequested();

            const string sql = "INSERT INTO Product (id, name, description, price) VALUES (@Id, @Name, @Description, @Price)";

            var result = await Context.Connection
                .ExecuteAsync(
                    sql,
                    new
                    {
                        entity.Id,
                        entity.Name,
                        entity.Description,
                        entity.Price
                    },
                    commandType: CommandType.Text
                );

            return result > 0 ? Result.Created : Error.Conflict("Product already exists");
        }
        public async Task<ErrorOr<Product>> Get(Guid id, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested) cancellationToken.ThrowIfCancellationRequested();

            const string sql = "SELECT id, name, description, price FROM Product WHERE id = @Id";

            var result = await Context.Connection
                .QueryFirstOrDefaultAsync<Product>(
                    sql,
                    new { Id = id },
                    commandType: CommandType.Text
                );

            if (result is null) return Error.NotFound("Product not found"); 

            return result;
        }
        public async Task<ErrorOr<List<Product>>> List(int page, int pageSize, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested) cancellationToken.ThrowIfCancellationRequested();

            const string sql = "SELECT id, name, description, price FROM Product LIMIT @PageSize OFFSET @Page";

            var result = await Context.Connection
                .QueryAsync<Product>(
                    sql,
                    new
                    {
                        Page = (page - 1) * pageSize,
                        PageSize = pageSize
                    },
                    commandType: CommandType.Text
                );

            return result.ToList();
        }
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
