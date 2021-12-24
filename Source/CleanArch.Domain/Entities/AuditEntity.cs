using System;

namespace CleanArch.Domain.Entities
{
    public class AuditEntity<TKey> : BaseEntity<TKey>
    {
        public string CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
    }
}