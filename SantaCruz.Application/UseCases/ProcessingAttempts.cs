using ErrorOr;
using Microsoft.Extensions.Options;
using SantaCruz.Application.DI;
using SantaCruz.Domain.Chain;
using SantaCruz.Domain.Model;
using SantaCruz.Domain.Repository;
using SantaCruz.Domain.UseCases;
using System.Threading;

namespace SantaCruz.Application.UseCases
{
    public class ProcessingAttempts(
        IDatabaseContext databaseContext,
        IOrderProcessingAttemptRepository repository,
        IUpdateToNextStatus updateToNextStatus,
        IOptions<ApiConfig> config,
        ILogging logging
    ) : Log(logging), IProcessingAttempts
    {
        private readonly IDatabaseContext _databaseContext = databaseContext;
        private readonly IOrderProcessingAttemptRepository _repository = repository;
        private readonly IUpdateToNextStatus _updateToNextStatus = updateToNextStatus;
        private readonly IOptions<ApiConfig> _config = config;

        public OrderProcessingAttempt Attempt { get; set; } = new OrderProcessingAttempt();

        private ILogging DatabaseFailure(List<Error> errors)
        {
            Logging.Step = nameof(ProcessingAttempts);
            Logging.Success = false;
            Logging.Messages.AddRange(errors.Select(e => e.Description));

            return Logging;
        }

        private async Task HandleAttemptNumber(CancellationToken cancellationToken)
        {
            var attemptsCountResult = await _repository.GetMaxAttemptByOrder(Attempt.Order.Id, cancellationToken);

            Attempt.AttemptNumber = attemptsCountResult.Value + 1;
        }

        private void HandleStatus()
        {
            if (!Attempt.Success)
            {
                Logging.Messages.Add("Processing attempt failed.");

                if (Attempt.AttemptNumber >= _config.Value.MaxAttempts)
                {
                    Logging.Messages.Add("Processing attempt exceeded maximum attempts.");
                    Attempt.Order.Status = OrderStatus.Failed;
                }
                else
                {
                    Attempt.Order.Status = OrderStatus.Pending;
                }
            }
            else
            {
                Attempt.Order.Status = OrderStatus.Completed;
            }
        }

        public async Task<IStep> Next(CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested) return Cancelled(nameof(ProcessingAttempts));

            await _databaseContext.Execute<Success>(async () => { 
                await HandleAttemptNumber(cancellationToken);

                return Result.Success;
            });

            var result = await _databaseContext.TransactionedOperation(() =>_repository.Create(Attempt));

            if (result.IsError) return DatabaseFailure(result.Errors);

            HandleStatus();

            _updateToNextStatus.Order = Attempt.Order;

            return _updateToNextStatus;
        }
    }
}
