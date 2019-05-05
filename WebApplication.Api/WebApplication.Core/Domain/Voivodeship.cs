using WebApplication.Core.Domain.Entities;

namespace WebApplication.Core.Domain
{
    public class Voivodeship : EntityInt
    {
        public string Name { get; private set; }

        public Voivodeship(string Name)
        {
            Id = null;
            this.Name = Name;
        }
    }
}
