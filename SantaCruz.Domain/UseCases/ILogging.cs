using SantaCruz.Domain.Chain;

namespace SantaCruz.Domain.UseCases
{
    public interface ILogging:ILastStep
    {
        string Step { get; set; }
        bool Success { get; set; }
    }
}
