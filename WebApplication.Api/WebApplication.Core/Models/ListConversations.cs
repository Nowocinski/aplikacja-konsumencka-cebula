using System;

namespace WebApplication.Core.Models
{
    public class ListConversations
    {
        public Guid? UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime Date { get; set; }
    }
}
