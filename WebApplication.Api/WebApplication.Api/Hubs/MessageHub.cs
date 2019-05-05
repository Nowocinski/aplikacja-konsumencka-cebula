using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using WebApplication.Api.Hubs.Models;
using WebApplication.Infrastructure.DTO;
using WebApplication.Infrastructure.Services.User;

namespace WebApplication.Api.Hubs
{
    public static class UserHandler
    {
        public static List<UserModel> ConnectedUsers = new List<UserModel>();
    }

    public class MessageHub : Hub
    {
        private readonly IUserService _userService;

        public MessageHub(IUserService userService)
        {
            _userService = userService;
        }

        public override Task OnConnectedAsync()
        {
            string socketId = Context.ConnectionId;
            Clients.Client(socketId).SendAsync("receiveUserId", socketId);
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception e)
        {
            UserHandler.ConnectedUsers.RemoveAll(user => user.SocketId == Context.ConnectionId);
            return base.OnDisconnectedAsync(e);
        }

        public async Task UpdateUsers(string socketId, Guid userId)
        {
            AccountDTO account = await _userService.GetAsync(userId);
            UserModel user = new UserModel {Id = userId, SocketId = socketId, User = account};
            UserHandler.ConnectedUsers.Add(user);
        }

        public async Task SendMessage(MessageModel message)
        {
            UserModel receiver = UserHandler.ConnectedUsers.FirstOrDefault(user => user.Id == message.ReceiverId);
            UserModel sender = UserHandler.ConnectedUsers.First(user => user.Id == message.SenderId);

            if (receiver != null && sender != null)
            {
                var messageData = new { receiver, sender, message = message.Msg };
                await Clients.Client(receiver.SocketId).SendAsync("receiveMessage", messageData);
            }
        }
    }
}
