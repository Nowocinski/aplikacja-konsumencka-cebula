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
        public string Role { get; private set; }
        public bool Blocked { get; private set; }

        private readonly ISet<Advertisement> _advertisements = new HashSet<Advertisement>();
        public virtual IEnumerable<Advertisement> Advertisements => _advertisements;

        private readonly ISet<Message> _messages = new HashSet<Message>();
        public virtual IEnumerable<Message> Messages => _messages;

        public virtual IEnumerable<Message> Recipient { get; }

        public User(string firstName, string lastName, string phoneNumber, string email, string password)
        {
            Id = Guid.NewGuid();
            SetFirstName(firstName);
            SetLastName(lastName);
            SetPhoneNumber(phoneNumber);
            SetEmail(email);
            SetPassword(password);
            Role = "user";
        }

        public Advertisement AddAdvertisement(Guid advertisementId ,string title, string description, float price,
            int city, string street, float size, string category, ISet<AdvertisementImage> images, int? floor = null)
        {
            Advertisement advertisement = new Advertisement(advertisementId, Id, title, description, price, city, street, size, category, images, floor);
            _advertisements.Add(advertisement);
            return advertisement;
        }

        public void AddMessage(Guid recipient, string text)
        {
            Message message = new Message(Id, recipient, text);
            _messages.Add(message);
        }

        public void SetFirstName(string firstName)
        {
            if (string.IsNullOrWhiteSpace(firstName))
            {
                throw new Exception($"User with id: '{Id}' can not have an empty first name.");
            }

            const int firstNameMaxLength = 20;
            if (firstName.Length > firstNameMaxLength)
            {
                throw new Exception($"User with id: '{Id}' first name can not have more than 20 characters.");
            }

            FirstName = firstName;
        }

        public void SetLastName(string lastName)
        {
            if (string.IsNullOrWhiteSpace(lastName))
            {
                throw new Exception($"User with id: '{Id}' can not have an empty last name.");
            }

            const int maxLengthLastName = 30;
            if (lastName.Length > maxLengthLastName)
            {
                throw new Exception($"User with id: '{Id}' last name can not have more than 30 characters.");
            }

            LastName = lastName;
        }

        public void SetEmail(string email)
        {
            Regex syntax = new Regex("^[0-9a-zA-Z]+([.-]{1}[0-9a-zA-Z]+)*@[a-z]+([.-]{1}[0-9a-z]+)*.[a-z]*$");
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new Exception($"User with id: '{Id}' can not have an empty e-mail.");
            }

            if (email.Length > 40)
            {
                throw new Exception($"User with id: '{Id}' e-mail can not have more than 40 characters.");
            }

            if (!syntax.IsMatch(email))
            {
                throw new Exception($"User with id: '{Id}' the e-mail syntax is incorrect.");
            }

            Email = email;
        }

        public void SetPhoneNumber(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
            {
                throw new Exception($"User with id: '{Id}' can not have an empty phone number.");
            }

            if (phoneNumber.Length > 11)
            {
                throw new Exception($"User with id: '{Id}' phone number can not have more than 11 characters.");
            }

            PhoneNumber = phoneNumber;
        }

        public void SetPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new Exception($"User with id: '{Id}' can not have an empty password.");
            }

            if (password.Length > 100)
            {
                throw new Exception($"User with id: '{Id}' password can not have more than 100 characters.");
            }

            Password = password;
        }

        public void ChangeStatus()
        {
            Blocked = !Blocked;
        }
    }
}
