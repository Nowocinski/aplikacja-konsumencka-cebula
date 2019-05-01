using System;
using System.Collections.Generic;

namespace WebApplication.Core.Domain
{
    public class Advertisement : EntityGuid
    {
        // Relacje
        public virtual User Relation { get; protected set; }
        public virtual City CityRel { get; protected set; }

        //-----------------------------------------

        // Pola
        public Guid UserId { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public float Price { get; private set; }
        public int City { get; private set; }
        public string Street { get; private set; }
        public int? Floor { get; private set; }
        public float Size { get; private set; }
        public string Category { get; private set; }
        public DateTime Date { get; private set; }

        // Listy pomocnicze
        private ISet<AdvertisementImage> _images = new HashSet<AdvertisementImage>();
        public virtual IEnumerable<AdvertisementImage> Images => _images;

        // Konstruktory
        protected Advertisement() { }

        public Advertisement(Guid Id, Guid UserId, string Title, string Description, float Price,
            int City, string Street, float Size, string Category, ISet<AdvertisementImage> Images, int? Floor)
        {
            this.Id = Id;
            this.UserId = UserId;
            this.Title = Title;
            this.Description = Description;
            this.Price = Price;
            this.City = City;
            this.Street = Street;
            this.Size = Size;
            this.Category = Category;
            this.Floor = Floor;
            Date = DateTime.UtcNow;
            AddImages(Images, Id);
        }

        private void AddImages(ISet<AdvertisementImage> Imgs, Guid Id)
        {
            foreach (var I in Imgs)
                _images.Add(new AdvertisementImage(Id, I.Image, I.Name, I.Description));
        }

        // Metody do obsługi pól

        public void SetTitle(string Title)
        {
            if (string.IsNullOrWhiteSpace(Title))
                throw new Exception($"Exception title: The title can not have an empty decription.");

            if (Title.Length > 25)
                throw new Exception($"Exception title {Title}: The title can be up to 25 characters long");

            this.Title = Title;
        }

        public void SetDescription(string Description)
        {
            if (string.IsNullOrWhiteSpace(Description))
                throw new Exception($"Exception description: The description can not have an empty decription.");

            if (Description.Length > 500)
                throw new Exception($"Exception description '{Description}': The description can be up to 500 characters long");

            this.Description = Description;
        }

        public void SetPrice(float Price)
        {
            if (Price <= 0)
                throw new Exception($"Exception price '{Price}': Price must be greater than zero");

            this.Price = Price;
        }

        public void SetCity(int City)
        {
            if (City <= 0)
                throw new Exception($"Exception city's id '{City}': City's id must be greater than zero");

            this.City = City;
        }

        public void SetStreet(string Street)
        {
            if (string.IsNullOrWhiteSpace(Street))
                throw new Exception($"Exception street: The street can not have an empty decription.");

            if (Street.Length > 50)
                throw new Exception($"Exception street '{Street}': The street can be up to 50 characters long");

            this.Street = Street;
        }

        public void SetSize(float Size)
        {
            if (Size <= 0)
                throw new Exception($"Exception size '{Size}': Size must be greater than zero");

            this.Size = Size;
        }

        public void SetCategory(string Category)
        {
            if (string.IsNullOrWhiteSpace(Category))
                throw new Exception($"Exception category: The category can not have an empty decription.");

            if (Category.Length > 30)
                throw new Exception($"Exception category '{Category}': The category can be up to 30 characters long");

            this.Category = Category;
        }

        public void SetFloor(int? Floor)
        {
            this.Floor = Floor;
        }

        public void SetAddImages(ISet<AdvertisementImage> Images)
        {
            _images.Clear();

            foreach (var I in Images)
                _images.Add(I);

        }
    }
}
