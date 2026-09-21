namespace SantaCruz.Domain.Contract
{
    public interface IEntity<TKey> where TKey : unmanaged
    {
        TKey Id { get; set; }
    }
}
