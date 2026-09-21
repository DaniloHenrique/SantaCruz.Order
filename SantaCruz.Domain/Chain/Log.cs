using SantaCruz.Domain.UseCases;

namespace SantaCruz.Domain.Chain
{
    public abstract class Log(ILogging logging)
    {
        protected ILogging Logging { get; } = logging;

        protected ILogging Cancelled(string step)
        {
            Logging.Step = step;
            Logging.Success = false;
            Logging.Messages.Add("Cancelled operation");

            return Logging;
        }
    }
}
