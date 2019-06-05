using System.Collections.Generic;
using WebApplication.Core.Domain.Entities;

namespace WebApplication.Core.Domain
{
    public class City : EntityInt
    {
        public virtual Voivodeship Voivodeship { get; private set; }
        public string Name { get; private set; }
        public int Voivodeship_Id { get; private set; }

        protected City()
        {

        }

        public City(string name, int voivodeshipId)
        {
            Name = name;
            Voivodeship_Id = voivodeshipId;
        }
        public virtual IEnumerable<Advertisement> Advertisement { get; set; }
    }
}
