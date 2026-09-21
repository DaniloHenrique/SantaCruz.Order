using ErrorOr;
using SantaCruz.Domain.Chain;
using SantaCruz.Domain.Model;

namespace SantaCruz.Domain.UseCases
{
    public interface IProcessingAttempts : IStep
    {
        public OrderProcessingAttempt Attempt { get; set; }
    }
}
