using ErrorOr;
using SantaCruz.Domain.Integration;
using SantaCruz.Domain.Model;

namespace SantaCruz.Infrastructure.Integrations
{
    public class MockedIntegration : IMockedIntegration
    {
        public bool IsEnabled { get; set; } = true;

        public async Task<ErrorOr<Success>> Run(Order order)
        {
            if (!IsEnabled) return Error.Failure(description: "Integration is disabled", code: "IntegrationDisabled");

            var duration = new Random().Next(5, 10);

            await Task.Delay(duration * 1000);

            return Result.Success;
        }
    }
}
