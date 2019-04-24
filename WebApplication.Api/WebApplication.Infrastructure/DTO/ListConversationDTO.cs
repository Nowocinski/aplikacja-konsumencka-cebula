using System;

namespace WebApplication.Infrastructure.DTO
{
    public class ListConversationDTO
    {
        public Guid SenderId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime Date { get; set; }
        public bool IsRead { get; set; } = true;
    }
}
