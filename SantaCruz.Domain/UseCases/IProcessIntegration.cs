using SantaCruz.Domain.Chain;
using SantaCruz.Domain.Model;

namespace SantaCruz.Domain.UseCases
{
    public interface IProcessIntegration:IStep
    {
        Order ToProcess { get; set; }
    }
}
