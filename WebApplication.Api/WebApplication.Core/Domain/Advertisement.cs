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
        public User Relation { get; protected set; }

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
        public IEnumerable<AdvertisementImage> Images => _images;

        // Konstruktory
        public Advertisement(Guid UserId, string Title, string Description, float Price,
            int City, string Street, float Size, string Category, int? Floor=null)
        {
            Id = Guid.NewGuid();
            this.UserId = UserId;
            this.Title = Title;
            this.Description = Description;
            this.Price = Price;
            this.City = City;
            this.Street = Street;
            this.Size = Size;
            this.Category = Category;
            this.Floor = Floor;
        }

        // Metody
        public void LoadImages(List<AdvertisementImage> Images)
        {
            foreach (AdvertisementImage i in Images)
                _images.Add(i);
        }
    }
}
