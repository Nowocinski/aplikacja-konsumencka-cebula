using System.Collections.Generic;
using WebApplication.Core.Domain.Entities;

namespace WebApplication.Core.Domain
{
    public class City : EntityInt
    {
        public virtual Voivodeship Voivodeship { get; private set; }
        public string Name { get; private set; }
        public int Voivodeship_Id { get; private set; }
        public City(string Name, int Voivodeship_Id)
        {
            Id = null;
            this.Name = Name;
            this.Voivodeship_Id = Voivodeship_Id;
        }
        public virtual IEnumerable<Advertisement> Advertisement { get; set; }
    }
}
