using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication.Infrastructure.Commands;
using WebApplication.Infrastructure.DTO;

namespace WebApplication.Infrastructure.Services.User
{
    public interface IUserService
    {
        Task<AccountDTO> GetAsync(Guid Id);
        Task RegisterAsync(Register Data);
        Task<LoginDTO> LoginAsync(string Email, string Password);
        Task DeleteAsync(Guid Id);
        Task UpdateAsync(Guid Id, UpdateUser User);
        Task<IEnumerable<AdvertismentDTO>> GetAdvertisementsUserAsync(Guid Id);
        Task<AdvertisementDetailsDTO> GetAdvertisementAsync(Guid Id);
        Task AddAdvertisementAsync(CreateAdvertisment Command, Guid UserId);
        Task UpdateAdvertisementAsync(CreateAdvertisment Advertisement, Guid Id);
        Task DeleteAdvertisementAsync(Guid Id);
        Task<AdvertisementsWithPageToEndDTO> GetFilterAdvertismentsAsync(string parameter, string type, int page, string text="");
        Task UpdateMessageAsync(Guid sender, Guid recipient, string text);
        Task<IEnumerable<ListConversationDTO>> GetConversationListAsync(Guid Id);
        Task<IEnumerable<MessagesDTO>> GetMessagesAsync(Guid Sender, Guid Recipient);
    }
}
