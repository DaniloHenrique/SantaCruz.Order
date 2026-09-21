namespace SantaCruz.Domain.Contract
{
    public interface ILink<TAbscissa, TAbscissaKey, TOrdinate, TOrdinateKey>
        where TAbscissaKey: unmanaged
        where TOrdinateKey: unmanaged
        where TAbscissa: IEntity<TAbscissaKey>
        where TOrdinate: IEntity<TOrdinateKey>
    {
    }
}
