using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace WebApplication.Core.Domain
{
    public class User : EntityGuid
    {
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string PhoneNumber { get; private set; }
        public string Email { get; private set; }
        public string Password { get; private set; }

        private ISet<Advertisement> _advertisements = new HashSet<Advertisement>();
        public virtual IEnumerable<Advertisement> Advertisements => _advertisements;

        private ISet<Message> _messages = new HashSet<Message>();
        public virtual IEnumerable<Message> Messages => _messages;

        public User(string FirstName, string LastName, string PhoneNumber, string Email, string Password)
        {
            Id = Guid.NewGuid();
            SetFirstName(FirstName);
            SetLastName(LastName);
            SetPhoneNumber(PhoneNumber);
            SetEmail(Email);
            SetPassword(Password);
        }

        public Advertisement AddAdvertisement(Guid AdvId ,string Title, string Description, float Price,
            int City, string Street, float Size, string Category, ISet<AdvertisementImage> Images, int? Floor = null)
        {
            var Adv = new Advertisement(AdvId, Id, Title, Description, Price, City, Street, Size, Category, Images, Floor);
            _advertisements.Add(Adv);

            return Adv;
        }

        public void AddMessage(Guid recipient, string text)
        {
            var msg = new Message(Id, recipient, text);
            _messages.Add(msg);
        }

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

            Regex syntax = new Regex("^[0-9a-zA-Z]+([.-]{1}[0-9a-zA-Z]+)*@[a-z]+([.-]{1}[0-9a-z]+)*.[a-z]*$");
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
