using Microsoft.Extensions.Options;
using SantaCruz.Application.DI;
using SantaCruz.Domain.Chain;
using SantaCruz.Domain.Integration;
using SantaCruz.Domain.Model;
using SantaCruz.Domain.UseCases;

namespace SantaCruz.Application.UseCases
{
    public class ProcessIntegration(
        IMockedIntegration mockedIntegration,
        IProcessingAttempts processingAttempts,
        ILogging logging,
        IOptions<ApiConfig> config
    ) : Log(logging),IProcessIntegration
    {
        private readonly IMockedIntegration _mockedIntegration = mockedIntegration;
        private readonly IProcessingAttempts _processingAttempts = processingAttempts;
        public Order ToProcess { get; set; } = new Order();

        public async Task<IStep> Next(CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested) return Cancelled(nameof(ProcessIntegration));

            _mockedIntegration.IsEnabled = config.Value.ServiceOk;

            var startedAt = DateTime.Now;

            var result = await _mockedIntegration.Run(ToProcess);

            ToProcess.Status = result.IsError ? OrderStatus.Failed : OrderStatus.Completed;

            _processingAttempts.Attempt = new OrderProcessingAttempt
            {
                Order = ToProcess,
                Success = !result.IsError,
                StartedAt = startedAt,
                FinishedAt = result.IsError ? null : DateTime.Now,
                ErrorMessage = result.IsError ? result.FirstError.Description : null
            };

            return _processingAttempts;
        }
    }
}
