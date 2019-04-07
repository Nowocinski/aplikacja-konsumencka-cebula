using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication.Core.Domain
{
    public class AdvertisementImage : EntityGuid
    {
        // Relacje
        [ForeignKey("Advertisement")]
        public virtual Advertisement Relation { get; protected set; }

        //-----------------------------------------

        // Pola
        [Required] [Column(TypeName = "uniqueidentifier")]
        public Guid Advertisement { get; protected set; }

        [Required] [Column(TypeName = "nvarchar(MAX)")]
        public string Image { get; protected set; }

        [Required] [Column(TypeName = "nvarchar(100)")]
        public string Description { get; protected set; }

        [Required] [Column(TypeName = "nvarchar(100)")]
        public string Name { get; protected set; }

        // Konstruktory
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
