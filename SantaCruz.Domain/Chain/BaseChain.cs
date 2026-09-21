namespace SantaCruz.Domain.Chain
{
    public abstract class BaseChain<TFirstStep>(TFirstStep firstStep) where TFirstStep : IStep
    {
        protected readonly TFirstStep _firstStep = firstStep;

        public virtual async Task<ILastStep> Start(CancellationToken cancellationToken)
        {
            var result = await _firstStep.Next(cancellationToken);

            while(result is not ILastStep) {
                result = await result.Next(cancellationToken);
            }

            return result as ILastStep ?? throw new InvalidOperationException();
        }
    }
}
