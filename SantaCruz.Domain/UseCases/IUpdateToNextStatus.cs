using SantaCruz.Domain.Chain;
using SantaCruz.Domain.Model;

namespace SantaCruz.Domain.UseCases
{
    public interface IUpdateToNextStatus:IStep
    {
        Order Order { get; set; }
    }
}
