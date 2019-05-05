using System;
using WebApplication.Infrastructure.DTO;

namespace WebApplication.Api.Hubs.Models
{
    public class UserModel
    {
        public Guid Id { get; set; }
        public string SocketId { get; set; }
        public AccountDTO User { get; set; }
    }
}
