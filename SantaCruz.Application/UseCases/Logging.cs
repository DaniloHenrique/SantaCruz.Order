using SantaCruz.Domain.Chain;
using SantaCruz.Domain.UseCases;

namespace SantaCruz.Application.UseCases
{
    public class Logging : ILogging
    {
        public string Step { get; set; } = nameof(Logging);
        public bool Success { get; set; } = true;

        public List<string> Messages { get; set; } = [];

        public Task<IStep> Next(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
