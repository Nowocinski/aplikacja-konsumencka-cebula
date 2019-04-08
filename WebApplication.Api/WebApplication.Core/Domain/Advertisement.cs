using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication.Core.Domain
{
    public class Advertisement : EntityGuid
    {
        // Relacje
        [ForeignKey("UserId")]
        public virtual User Relation { get; protected set; }

        [ForeignKey("City")]
        public virtual City CityRel { get; protected set; }

        //-----------------------------------------

        // Pola
        [Required] [Column(TypeName = "uniqueidentifier")]
        public Guid UserId { get; private set; }

        [Required] [Column(TypeName = "nvarchar(25)")]
        public string Title { get; private set; }

        [Required] [Column(TypeName = "nvarchar(500)")]
        public string Description { get; private set; }

        [Required] [Column(TypeName = "decimal(20, 2)")]
        public float Price { get; private set; }

        [Required] [Column(TypeName = "int")]
        public int City { get; private set; }

        [Required] [Column(TypeName = "nvarchar(50)")]
        public string Street { get; private set; }

        [Column(TypeName = "int")]
        public int? Floor { get; private set; }

        [Required] [Column(TypeName = "decimal(20, 2)")]
        public float Size { get; private set; }

        [Required] [Column(TypeName = "nvarchar(30)")]
        public string Category { get; private set; }

        [Required] [Column(TypeName = "datetime")]
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
            this.Title = Title;
        }

        public void SetDescription(string Description)
        {
            this.Description = Description;
        }

        public void SetPrice(float Price)
        {
            this.Price = Price;
        }

        public void SetCity(int City)
        {
            this.City = City;
        }

        public void SetStreet(string Street)
        {
            this.Street = Street;
        }

        public void SetSize(float Size)
        {
            this.Size = Size;
        }

        public void SetCategory(string Category)
        {
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
