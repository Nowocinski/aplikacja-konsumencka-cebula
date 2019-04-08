using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication.Core.Domain;
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
        Task<IEnumerable<AdvertismentDTO>> GetAllAdvertismentsAsync();
        Task AddAdvertisementAsync(CreateAdv Command, Guid UserId);
        Task UpdateAdvertisementAsync(CreateAdv Advertisement, Guid Id);
        Task DeleteAdvertisementAsync(Guid Id);
        Task<AdvertisementsWithPageToEndDTO> GetSortAdvertismentsAsync(string parameter, string type, int page);

        Task UpdateMessageAsync(Guid sender, Guid recipient, string text);
    }
}
