using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication.Core.Domain;

namespace WebApplication.Core.Repositories
{
    public interface IAdminRepository
    {
        Task<User> GetUserAsync(Guid id);
        Task<IEnumerable<User>> GetUsersAsync();
        Task UpdateUserAsync(User user);
        Task<Advertisement> GetAdvertismentsAsync(Guid id);
        Task<IEnumerable<Advertisement>> GetAdvertisementsAsync();
        Task UpdateAdvertismentAsync(Advertisement advertisement);
    }
}
