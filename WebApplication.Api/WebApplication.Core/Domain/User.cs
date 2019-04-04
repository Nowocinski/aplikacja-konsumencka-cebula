using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication.Core.Domain
{
    public class User : EntityGuid
    {
        // Pola
        [Required] [Column(TypeName = "nvarchar(20)")]
        public string FirstName { get; private set; }

        [Required] [Column(TypeName = "nvarchar(30)")]
        public string LastName { get; protected set; }

        [Required] [Column(TypeName = "nvarchar(11)")]
        public string PhoneNumber { get; protected set; }

        [Required] [Column(TypeName = "nvarchar(40)")]
        public string Email { get; protected set; }

        [Required] [Column(TypeName = "nvarchar(100)")]
        public string Password { get; protected set; }

        // Listy pomocnicze
        private ISet<Advertisement> _advertisements = new HashSet<Advertisement>();
        public IEnumerable<Advertisement> Advertisements => _advertisements;

        // Konstruktory
        public User(string FirstName, string LastName, string PhoneNumber, string Email, string Password)
        {
            Id = Guid.NewGuid();
            this.FirstName = FirstName;
            this.LastName = LastName;
            this.PhoneNumber = PhoneNumber;
            this.Email = Email;
            this.Password = Password;
        }

        // Metody
        public void AddAdvertisement(string Title, string Description, float Price,
            int City, string Street, float Size, string Category, int? Floor = null)
        {
            var Adv = new Advertisement(Id, Title, Description, Price, City, Street, Size, Category, Floor);
            _advertisements.Add(Adv);
        }
    }
}
