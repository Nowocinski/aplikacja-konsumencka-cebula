using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication.Core.Domain;
using WebApplication.Core.Models;

namespace WebApplication.Core.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetAsync(Guid Id);
        Task<User> GetAsync(string Email);
        Task<User> GetByPhoneAsync(string Phone);
        Task AddAsync(User User);
        Task UpdateAsync(User User);
        Task DeleteAsync(User User);
        Task<IEnumerable<Advertisement>> GetAdvertisementsUserAsync(Guid Id);
        Task<Advertisement> GetAdvertisementAsync(Guid Id);
        Task<IEnumerable<Advertisement>> GetFilterAdvertismentsAsync(string parameter, string type, int page, string text = "");
        Task<int> GetAmountOfAdvertismentsAsync();
        Task AddAdvertisementAsync(Advertisement Advertisement);
        Task UpdateAdvertisementAsync(Advertisement Advertisement);
        Task DeleteAdvertisementAsync(Advertisement Advertisement);
        Task UpdateMessageAsync(IEnumerable<Message> message);
        Task<IEnumerable<ListConversations>> GetConversationListAsync(Guid Id);
        Task<IEnumerable<Message>> GetMessagesAsync(Guid Sender, Guid Recipient);
    }
}
