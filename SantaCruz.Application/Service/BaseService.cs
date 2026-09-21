using SantaCruz.Domain.Repository;

namespace SantaCruz.Application.Service
{
    public abstract class BaseService(IDatabaseContext context)
    {
        public IDatabaseContext Context { get; } = context;
    }
}
