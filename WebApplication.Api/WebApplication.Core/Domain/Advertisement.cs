using System;
using System.Collections.Generic;

namespace WebApplication.Core.Domain
{
    public class Advertisement : EntityGuid
    {
        public virtual User User { get; private set; }
        public virtual City City { get; private set; }
        public Guid User_Id { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public float Price { get; private set; }
        public int City_id { get; private set; }
        public string Street { get; private set; }
        public int? Floor { get; private set; }
        public float Size { get; private set; }
        public string Category { get; private set; }
        public DateTime Date { get; private set; }
        public bool Verification { get; private set; }

        private ISet<AdvertisementImage> _images = new HashSet<AdvertisementImage>();
        public virtual IEnumerable<AdvertisementImage> Images => _images;

        protected Advertisement()
        {
        }

        public Advertisement(Guid id, Guid user_id, string title, string description, float price,
            int city_id, string street, float size, string category, ISet<AdvertisementImage> images, int? floor)
        {
            Id = id;
            User_Id = user_id;
            Title = title;
            Description = description;
            Price = price;
            City_id = city_id;
            Street = street;
            Size = size;
            Category = category;
            Floor = floor;
            Date = DateTime.UtcNow;
            AddImages(images, id);
        }

        private void AddImages(ISet<AdvertisementImage> images, Guid id)
        {
            foreach (AdvertisementImage image in images)
            {
                _images.Add(new AdvertisementImage(id, image.Image, image.Name, image.Description));
            }
        }

        public void SetTitle(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                throw new Exception($"Exception title: The title can not have an empty description.");
            }

            const int titleMaxLength = 25;
            if (title.Length > titleMaxLength)
            {
                throw new Exception($"Exception title {title}: The title can be up to 25 characters long");
            }

            Title = title;
        }

        public void SetDescription(string description)
        {
            if (string.IsNullOrWhiteSpace(description))
            {
                throw new Exception($"Exception description: The description can not have an empty decription.");
            }

            const int descriptionMaxLength = 500;
            if (description.Length > descriptionMaxLength)
            {
                throw new Exception($"Exception description '{description}': The description can be up to 500 characters long");
            }

            Description = description;
        }

        public void SetPrice(float price)
        {
            const int minPrice = 0;
            if (price < minPrice)
            {
                throw new Exception($"Exception price '{price}': Price must be greater than zero");
            }

            Price = price;
        }

        public void SetCity(int city_id)
        {
            const int minCityId = 0;
            if (city_id < minCityId)
            {
                throw new Exception($"Exception city's id '{City}': City's id must be greater than zero");
            }

            City_id = city_id;
        }

        public void SetStreet(string street)
        {
            if (string.IsNullOrWhiteSpace(street))
            {
                throw new Exception($"Exception street: The street can not have an empty description.");
            }

            const int streetNameMaxLength = 500;
            if (street.Length > streetNameMaxLength)
            {
                throw new Exception($"Exception street '{street}': The street can be up to 50 characters long");
            }

            Street = street;
        }

        public void SetSize(float size)
        {
            const int minSize = 0;
            if (size <= minSize)
            {
                throw new Exception($"Exception size '{Size}': Size must be greater than zero");
            }

            Size = size;
        }

        public void SetCategory(string category)
        {
            if (string.IsNullOrWhiteSpace(category))
            {
                throw new Exception($"Exception category: The category can not have an empty description.");
            }

            const int categoryMinLength = 30;
            if (category.Length > categoryMinLength)
            {
                throw new Exception($"Exception category '{category}': The category can be up to 30 characters long");
            }

            Category = category;
        }

        public void SetFloor(int? floor)
        {
            Floor = floor;
        }

        public void SetAddImages(ISet<AdvertisementImage> images)
        {
            _images.Clear();

            foreach (AdvertisementImage image in images)
            {
                _images.Add(image);
            }
        }

        public void ChangeSatusVerification()
        {
            Verification = !Verification;
        }
    }
}
