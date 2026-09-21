using SantaCruz.Application.Chain;

namespace SantaCruz.Processing
{
    public class Worker(ILogger<Worker> logger, IServiceScopeFactory scopeFactory) : BackgroundService
    {
        private readonly ILogger<Worker> _logger = logger;
        private readonly IServiceScopeFactory _scopeFactory = scopeFactory;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Worker started at {Time}", DateTimeOffset.Now);

            using var timer = new PeriodicTimer(TimeSpan.FromSeconds(5));

            try
            {
                while (await timer.WaitForNextTickAsync(stoppingToken))
                {
                    await DoWorkAsync(stoppingToken);
                }
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("Worker stopped gracefully.");
            }
        }

        private async Task DoWorkAsync(CancellationToken ct)
        {
            await using var scope = _scopeFactory.CreateAsyncScope();

            var mainProcessChain = scope.ServiceProvider.GetRequiredService<MainProcessChain>();

            _logger.LogInformation("Processing orders at {Time}", DateTimeOffset.Now);
            
            var logged = await mainProcessChain.Start(ct);

            foreach (string message in logged.Messages) _logger.LogInformation(message); 
        }
    }
}
