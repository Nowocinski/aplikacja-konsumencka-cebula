using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication.Core.Domain.Entities
{
    public abstract class EntityInt
    {
        [Key] [Column(TypeName = "int")]
        public int? Id { get; protected set; }
    }
}
