using System;

namespace WebApplication.Core.Domain
{
    public class AdvertisementImage : EntityGuid
    {
        public virtual Advertisement Relation { get; protected set; }
        public Guid Advertisement { get; protected set; }
        public string Image { get; protected set; }
        public string Description { get; protected set; }
        public string Name { get; protected set; }
        public AdvertisementImage(Guid Advertisement, string Image, string Name, string Description="")
        {
            Id = Guid.NewGuid();
            this.Advertisement = Advertisement;
            this.Image = Image;
            this.Name = Name;
            this.Description = Description;
        }
    }
}
