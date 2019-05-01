using WebApplication.Core.Domain.Entities;

namespace WebApplication.Core.Domain
{
    public class City : EntityInt
    {
        public virtual Voivodeship Voivodeship { get; protected set; }
        public string Name { get; protected set; }
        public int Voivodeship_Id { get; protected set; }
        public City(string Name, int Voivodeship_Id)
        {
            Id = null;
            this.Name = Name;
            this.Voivodeship_Id = Voivodeship_Id;
        }
        public virtual Advertisement Advertisement { get; set; }
    }
}
