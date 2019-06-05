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
        public Message(Guid senderId, Guid recipientId, string contents)
        {
            Sender_Id = senderId;
            Recipient_Id = recipientId;
            Contents = contents;
            Date = DateTime.UtcNow;
        }
    }
}
