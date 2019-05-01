using System;
using System.ComponentModel.DataAnnotations.Schema;
using WebApplication.Core.Domain.Entities;

namespace WebApplication.Core.Domain
{
    public class Message : EntityInt
    {
        // Relacje
        [ForeignKey("Sender")]
        public virtual User Relation { get; protected set; }

        //-----------------------------------------

        // Pola
        public Guid Sender { get; protected set; }
        public Guid Recipient { get; protected set; }
        public string Contents { get; protected set; }
        public DateTime Date { get; protected set; }

        // Konstruktory
        public Message(Guid Sender, Guid Recipient, string Contents)
        {
            Id = null;
            this.Sender = Sender;
            this.Recipient = Recipient;
            this.Contents = Contents;
            Date = DateTime.UtcNow;
        }
    }
}
