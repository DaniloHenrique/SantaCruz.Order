using SantaCruz.Domain.Chain;
using SantaCruz.Domain.Model;
using SantaCruz.Domain.UseCases;

namespace SantaCruz.Application.Chain
{
    public class OrderProcessChain(IProcessBegin processBegin):BaseChain<IProcessBegin>(processBegin)
    {
        public Task<ILastStep> Start(Order order, CancellationToken cancellationToken)
        {
            order.Status = OrderStatus.Processing;

            _firstStep.Order= order;

            return base.Start(cancellationToken);
        }
    }
}
