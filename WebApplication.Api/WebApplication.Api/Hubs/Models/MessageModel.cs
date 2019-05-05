using System;

namespace WebApplication.Api.Hubs
{
    public class MessageModel
    {
        public string Msg { get; set; }
        public Guid ReceiverId { get; set; }
        public Guid SenderId { get; set; }
    }
}