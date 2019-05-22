using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication.Infrastructure.DTO;

namespace WebApplication.Infrastructure.Services.User
{
    public interface IAdminService
    {
        Task<IEnumerable<AccountDTO>> GetUsersAsync();
        Task ChangeOfUserStatus(Guid id);
        Task<IEnumerable<AdvertismentDTO>> GetAdvertisementsAsync();
        Task ChangeOfAdvertismentStatus(Guid id);
    }
}
