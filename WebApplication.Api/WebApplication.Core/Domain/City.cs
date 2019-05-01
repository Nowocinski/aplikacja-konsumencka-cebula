using WebApplication.Core.Domain.Entities;

namespace WebApplication.Core.Domain
{
    public class City : EntityInt
    {
        // Relacje
        public virtual Voivodeship Relation { get; protected set; }

        //-----------------------------------------

        public string Name { get; protected set; }
        public int Voivodeship { get; protected set; }

        // Konstruktory
        public City(string Name, int Voivodeship)
        {
            Id = null;
            this.Name = Name;
            this.Voivodeship = Voivodeship;
        }

        public virtual Advertisement Advertisement { get; set; }
    }
}
