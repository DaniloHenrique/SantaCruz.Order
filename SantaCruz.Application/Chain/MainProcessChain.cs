using SantaCruz.Application.UseCases;
using SantaCruz.Domain.Chain;
using SantaCruz.Domain.UseCases;

namespace SantaCruz.Application.Chain
{
    public class MainProcessChain(IGetPendingOrders firstStep) : BaseChain<IGetPendingOrders>(firstStep)
    {
    }
}
