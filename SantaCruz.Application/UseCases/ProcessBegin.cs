using SantaCruz.Domain.Chain;
using SantaCruz.Domain.Repository;
using SantaCruz.Domain.UseCases;

namespace SantaCruz.Application.UseCases
{
    public class ProcessBegin(
        IDatabaseContext databaseContext,
        IProcessIntegration processIntegration,
        IOrderRepository orderRepository,
        ILogging logging
    )
        :UpdateToNextStatus(databaseContext, orderRepository, logging), IProcessBegin
    {
        private readonly IProcessIntegration _processIntegration = processIntegration;

        public override async Task<IStep> Next(CancellationToken cancellationToken)
        {
            var logging = await base.Next(cancellationToken);

            _processIntegration.ToProcess = Order;

            return _cancelled ? logging : _processIntegration;
        }

    }
}
