using CleanArch.Core.Interfaces;

namespace CleanArch.Domain.Entities
{
    public class BaseEntity<TKey> : IEntity<TKey>
    {
        public TKey Id { get; set; }
    }
}