namespace SantaCruz.Domain.Chain
{
    public interface ILastStep:IStep
    {
        List<string> Messages { get; set; }
    }
}
