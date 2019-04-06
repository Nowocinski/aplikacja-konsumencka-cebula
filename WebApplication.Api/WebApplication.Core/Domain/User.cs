using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;

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
            SetFirstName(FirstName);
            SetLastName(LastName);
            SetPhoneNumber(PhoneNumber);
            SetEmail(Email);
            SetPassword(Password);
        }

        // Metody
        /*public void AddAdvertisement(string Title, string Description, float Price,
            int City, string Street, float Size, string Category, int? Floor = null)
        {
            var Adv = new Advertisement(Id, Title, Description, Price, City, Street, Size, Category, Floor);
            _advertisements.Add(Adv);
        }*/

        // Walidacja pól
        public void SetFirstName(string FirstName)
        {
            if (string.IsNullOrWhiteSpace(FirstName))
                throw new Exception($"User with id: '{Id}' can not have an empty first name.");
            if (FirstName.Length > 20)
                throw new Exception($"User with id: '{Id}' first name can not have more than 20 characters.");

            this.FirstName = FirstName;
        }

        public void SetLastName(string LastName)
        {
            if (string.IsNullOrWhiteSpace(LastName))
                throw new Exception($"User with id: '{Id}' can not have an empty last name.");
            if (LastName.Length > 30)
                throw new Exception($"User with id: '{Id}' last name can not have more than 30 characters.");

            this.LastName = LastName;
        }

        public void SetEmail(string Email)
        {
            if (string.IsNullOrWhiteSpace(Email))
                throw new Exception($"User with id: '{Id}' can not have an empty e-mail.");
            if (Email.Length > 40)
                throw new Exception($"User with id: '{Id}' e-mail can not have more than 40 characters.");

            Regex syntax = new Regex("^[0-9a-zA-Z]+([.-]{1}[0-9a-zA-Z]+)*@[a-z]+([.-]{1}[0-9a-z]+)*.[a-z]$");
            if (!syntax.IsMatch(Email))
                throw new Exception($"User with id: '{Id}' the e-mail syntax is incorrect.");

            this.Email = Email;
        }

        public void SetPhoneNumber(string PhoneNumber)
        {
            if (string.IsNullOrWhiteSpace(PhoneNumber))
                throw new Exception($"User with id: '{Id}' can not have an empty phone number.");
            if (PhoneNumber.Length > 11)
                 throw new Exception($"User with id: '{Id}' phone number can not have more than 11 characters.");

            this.PhoneNumber = PhoneNumber;
        }

        public void SetPassword(string Password)
        {
            if (string.IsNullOrWhiteSpace(Password))
                throw new Exception($"User with id: '{Id}' can not have an empty password.");
            if (Password.Length > 100)
                throw new Exception($"User with id: '{Id}' password can not have more than 100 characters.");

            this.Password = Password;
        }
    }
}
