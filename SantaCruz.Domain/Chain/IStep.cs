namespace SantaCruz.Domain.Chain
{
    public interface IStep
    {
        Task<IStep> Next(CancellationToken cancellationToken);
    }
}
