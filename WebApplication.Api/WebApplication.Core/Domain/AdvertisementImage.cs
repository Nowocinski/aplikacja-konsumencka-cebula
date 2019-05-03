using System;

namespace WebApplication.Core.Domain
{
    public class AdvertisementImage : EntityGuid
    {
        public Guid Advertisement_Id { get; protected set; }
        public string Image { get; protected set; }
        public string Description { get; protected set; }
        public string Name { get; protected set; }
        public AdvertisementImage(Guid Advertisement_Id, string Image, string Name, string Description="")
        {
            Id = Guid.NewGuid();
            this.Advertisement_Id = Advertisement_Id;
            this.Image = Image;
            this.Name = Name;
            this.Description = Description;
        }
    }
}
