namespace SantaCruz.Domain.Repository
{
    public abstract class DatabaseRepository(IDatabaseContext context)
    {
        protected IDatabaseContext Context { get; } = context;
    }
}
