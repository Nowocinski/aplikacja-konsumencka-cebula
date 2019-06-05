using System;

namespace WebApplication.Core.Domain
{
    public class AdvertisementImage : EntityGuid
    {
        public Guid Advertisement_Id { get; private set; }
        public string Image { get; private set; }
        public string Description { get; private set; }
        public string Name { get; private set; }

        protected AdvertisementImage()
        {

        }
        public AdvertisementImage(Guid advertisementId, string image, string name, string description="")
        {
            Id = Guid.NewGuid();
            Advertisement_Id = advertisementId;
            Image = image;
            Name = name;
            Description = description;
        }
    }
}
