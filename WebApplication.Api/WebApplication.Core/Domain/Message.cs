﻿using System;
using WebApplication.Core.Domain.Entities;

namespace WebApplication.Core.Domain
{
    public class Message : EntityInt
    {
        public virtual User User { get; protected set; }
        public Guid Sender_Id { get; protected set; }
        public Guid Recipient_Id { get; protected set; }
        public string Contents { get; protected set; }
        public DateTime Date { get; protected set; }

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
