using System;
using WebApplication.Core.Domain.Entities;

namespace WebApplication.Core.Domain
{
    public class Message : EntityInt
    {
        public virtual User Sender { get; private set; }
        public virtual User Recipient { get; private set; }
        public Guid? Sender_Id { get; private set; }
        public Guid? Recipient_Id { get; private set; }
        public string Contents { get; private set; }
        public DateTime Date { get; private set; }
        protected Message()
        {
        }
        public Message(Guid Sender_Id, Guid Recipient_Id, string Contents)
        {
            Id = null;
            this.Sender_Id = Sender_Id;
            this.Recipient_Id = Recipient_Id;
            this.Contents = Contents;
            Date = DateTime.UtcNow;
        }
    }
}
