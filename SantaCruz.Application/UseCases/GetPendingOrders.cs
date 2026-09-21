using ErrorOr;
using SantaCruz.Application.Chain;
using SantaCruz.Domain.Chain;
using SantaCruz.Domain.Model;
using SantaCruz.Domain.Repository;
using SantaCruz.Domain.UseCases;

namespace SantaCruz.Application.UseCases
{
    public class GetPendingOrders(
        IDatabaseContext databaseContext,
        IOrderRepository repository, 
        ILogging logging,
        OrderProcessChain processChain
    ) :Log(logging), IGetPendingOrders
    {
        private readonly IDatabaseContext _databaseContext = databaseContext;
        private readonly IOrderRepository _repository = repository;
        private readonly OrderProcessChain _processChain = processChain;
        

        private ILogging NoOrderFound()
        {
            Logging.Step = nameof(GetPendingOrders);
            Logging.Success = false;
            Logging.Messages.Add("No orders found");

            return Logging;
        }

        private ILogging DatabaseError(List<Error> errors)
        {
            Logging.Step = nameof(GetPendingOrders);
            Logging.Success = false;
            Logging.Messages.AddRange([.. errors.Select(e => e.Description)]);

            return Logging;
        }


        public async Task<IStep> Next(CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested) return Cancelled(nameof(GetPendingOrders));
            
            var result = await _databaseContext.Execute(() => _repository.ListOrderByStatus(OrderStatus.Pending, cancellationToken)); ;

            if (result.IsError)
            {
                return result.FirstError.Type == ErrorType.NotFound
                    ? NoOrderFound()
                    : DatabaseError(result.Errors);
            }

            Logging.Messages.Add($"Pending orders found: {result.Value.Count}");
            
            var orders = result.Value;

            foreach (var order in orders)
            {
                var completed = await _processChain.Start(order, cancellationToken);

                Logging.Messages.AddRange(completed.Messages);
            }

            return Logging;
        }
    }
}
