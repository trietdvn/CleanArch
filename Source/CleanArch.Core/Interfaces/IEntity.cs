namespace CleanArch.Core.Interfaces
{
    public interface IEntity<TKey>
    {
        TKey Id { get; set; }
    }
}