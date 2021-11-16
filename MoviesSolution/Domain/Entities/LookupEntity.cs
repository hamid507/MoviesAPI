using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public abstract class LookupEntity : BaseEntity
    {
        [Key]
        public Guid Id { get; set; }
    }
}
