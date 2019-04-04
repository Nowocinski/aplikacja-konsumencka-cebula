using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication.Core.Domain
{
    public abstract class EntityGuid
    {
        [Key] [Column(TypeName = "uniqueidentifier")]
        public Guid Id { get; protected set; }
    }
}
