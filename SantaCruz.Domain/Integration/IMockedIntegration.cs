using ErrorOr;
using SantaCruz.Domain.Model;

namespace SantaCruz.Domain.Integration
{
    public interface IMockedIntegration
    {
        Task<ErrorOr<Success>> Run(Order order);
        bool IsEnabled { get; set; }
    }
}
