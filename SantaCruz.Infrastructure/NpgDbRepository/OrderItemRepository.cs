using Dapper;
using ErrorOr;
using SantaCruz.Domain.Model;
using SantaCruz.Domain.Repository;
using System.Data;

namespace SantaCruz.Infrastructure.NpgDbRepository
{
    public class OrderItemRepository(IDatabaseContext context) : 
        DatabaseRepository(context), 
        IOrderItemRepository
    {
        public async Task<ErrorOr<Created>> Create(OrderItem link, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested) cancellationToken.ThrowIfCancellationRequested();

            const string sql = "INSERT INTO OrderItem (idOrder,idProduct, quantity) VALUES (@IdOrder, @IdProduct, @Quantity)";

            var result = await Context.Connection
                .ExecuteAsync(
                    sql,
                    new
                    {
                        link.IdOrder,
                        link.IdProduct,
                        link.Quantity
                    },
                    commandType:CommandType.Text
                );

            return result > 0 ? Result.Created : Error.Conflict("OrderItem already exists");
        }
        public async Task<ErrorOr<Deleted>> Delete(OrderItem link, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested) cancellationToken.ThrowIfCancellationRequested();

            const string sql = "DELETE FROM orderItem WHERE idOrder = @IdOrder AND idProduct = @IdProduct";

            var result = await Context.Connection
                .ExecuteAsync (
                    sql, 
                    new { 
                        link.IdOrder,
                        link.IdProduct
                    }, 
                    commandType: CommandType.Text
                );
            return result > 0 ? Result.Deleted : Error.NotFound("OrderItem not found");
        }
        public async Task<ErrorOr<OrderItem>> Get(Order abscissa, Product ordinate, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested) cancellationToken.ThrowIfCancellationRequested();

            const string sql = @"SELECT 
            OrderItem.IdOrder as Id,
            OrderItem.Quantity,
            Product.Id as Id,
            Product.Name as Name,
            Product.Description as Description,
            Product.Price as Price,
            Order.Id as IdOrder,
            Order.CreatedAt as CreatedAt,
            Order.UpdatedAt as UpdatedAt,
            Order.Status as Status
            FROM OrderItem 
            JOIN Product ON Product.Id = OrderItem.IdProduct
            JOIN Order ON Order.Id = OrderItem.IdOrder
            WHERE 
            OrderItem.IdOrder = @IdOrder 
            AND OrderItem.IdProduct   = @IdProduct";

            var result = await Context.Connection
                .QueryAsync<OrderItem,Product,Order, OrderItem>(
                    sql,
                    (orderItem, product, order) =>
                    {
                        orderItem.Product = product;
                        orderItem.Order = order;

                        return orderItem;
                    },
                    new
                    {
                        IdOrder = abscissa.Id,
                        IdProduct = ordinate.Id
                    },
                    commandType: CommandType.Text
                );

            var orderItemResult = result.FirstOrDefault();

            if ( orderItemResult is null ) return Error.NotFound("OrderItem not found");

            return orderItemResult;
        }
        public async Task<ErrorOr<List<Product>>> ListByAbscissa(Order abscissa, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested) cancellationToken.ThrowIfCancellationRequested();

            const string sql = @"SELECT 
            Product.Id as Id,
            Product.Name as Name,
            Product.Description as Description,
            Product.Price as Price,
            FROM Product 
            JOIN OrderItem ON OrderItem.IdProduct = Product.Id      
            WHERE 
            OrderItem.IdOrder = @IdOrder";

            var result = await Context.Connection
                .QueryAsync<Product>(
                    sql,
                    new{ IdProduct = abscissa.Id },
                    commandType: CommandType.Text
                );

            return result.ToList();
        }
        public async Task<ErrorOr<List<OrderItem>>> ListItensByOrder(Order order, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested) cancellationToken.ThrowIfCancellationRequested();

            const string sql = @"SELECT 
                OrderItem.Quantity,
                Product.Id as Id,
                Product.Name as Name,
                Product.Description as Description,
                Product.Price as Price
            FROM OrderItem 
            JOIN Product ON Product.Id = OrderItem.IdProduct
            WHERE idOrder = @IdOrder";

            var result = await Context.Connection
                .QueryAsync<OrderItem, Product, OrderItem>(
                    sql,
                    (orderItem, product) =>
                    {
                        orderItem.Product = product;

                        return orderItem;
                    },
                    new { IdOrder = order.Id },
                    commandType: CommandType.Text
                );

            return result.ToList();
        }
        public async Task<ErrorOr<List<Order>>> ListByOrdinate(Product ordinate, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested) cancellationToken.ThrowIfCancellationRequested();

            const string sql = @"SELECT 
            ""order"".Id as Id, 
            ""order"".CreatedAt as CreatedAt,
            ""order"".UpdatedAt as UpdatedAt,
            ""order"".Status as Status
            FROM ""order""
            JOIN OrderItem ON Order.Id = OrderItem.IdOrder
            WHERE 
            OrderItem.IdProduct = @IdProduct";

            var result = await Context.Connection
                .QueryAsync<Order>(
                    sql,
                    new{ IdProduct = ordinate.Id },
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
