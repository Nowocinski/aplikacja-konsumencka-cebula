using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApplication.Core.Domain.Entities;

namespace WebApplication.Core.Domain
{
    public class City : EntityInt
    {
        // Relacje
        [ForeignKey("Voivodeship")]
        public Voivodeship Relation { get; protected set; }

        //-----------------------------------------

        [Required] [Column(TypeName = "nvarchar(40)")]
        public string Name { get; protected set; }

        [Required] [Column(TypeName = "int")]
        public int Voivodeship { get; protected set; }

        // Konstruktory
        public City(string Name, int Voivodeship)
        {
            Id = null;
            this.Name = Name;
            this.Voivodeship = Voivodeship;
        }
    }
}
