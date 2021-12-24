using System;

namespace CleanArch.Domain.Entities
{
    public class Customer : AuditEntity<Guid>
    {
        public string Name { get; set; }
        public string Phone { get; set; }
    }
}