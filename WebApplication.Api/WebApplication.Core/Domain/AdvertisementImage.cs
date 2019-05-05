using System;

namespace WebApplication.Core.Domain
{
    public class AdvertisementImage : EntityGuid
    {
        public Guid Advertisement_Id { get; private set; }
        public string Image { get; private set; }
        public string Description { get; private set; }
        public string Name { get; private set; }
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
