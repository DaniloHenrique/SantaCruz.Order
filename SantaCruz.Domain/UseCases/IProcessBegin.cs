using SantaCruz.Domain.Chain;
using SantaCruz.Domain.Model;

namespace SantaCruz.Domain.UseCases
{
    public interface IProcessBegin:IStep
    {
        Order Order { get; set; }
    }
}
