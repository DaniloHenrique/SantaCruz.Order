using ErrorOr;

namespace SantaCruz.Domain.Contract
{
    public interface ILinked<TLink, TAbscissa, TAbscissaKey, TOrdinate, TOrdinateKey>
        where TAbscissaKey : unmanaged
        where TOrdinateKey : unmanaged
        where TAbscissa : IEntity<TAbscissaKey>
        where TOrdinate : IEntity<TOrdinateKey>
        where TLink : ILink<TAbscissa,TAbscissaKey, TOrdinate, TOrdinateKey>
    {
        Task<ErrorOr<Created>> Create(TLink link, CancellationToken cancellationToken = default);
        Task<ErrorOr<Deleted>> Delete(TLink link, CancellationToken cancellationToken = default);
        Task<ErrorOr<TLink>> Get(TAbscissa abscissa, TOrdinate ordinate, CancellationToken cancellationToken = default);
        Task<ErrorOr<List<TAbscissa>>> ListByOrdinate(TOrdinate ordinate, CancellationToken cancellationToken = default);
        Task<ErrorOr<List<TOrdinate>>> ListByAbscissa(TAbscissa abscissa, CancellationToken cancellationToken = default);
    }
}
