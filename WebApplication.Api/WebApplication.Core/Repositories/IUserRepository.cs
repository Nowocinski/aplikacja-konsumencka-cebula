using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication.Core.Domain;

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

        Task<Advertisement> GetAdvertisementAsync(Guid Id);
        Task<IEnumerable<Advertisement>> GetAllAdvertismentsAsync();
        Task AddAdvertisementAsync(Advertisement Advertisement);
        Task UpdateAdvertisementAsync(Advertisement Advertisement);
        Task DeleteAdvertisementAsync(Advertisement Advertisement);
        Task AddImagesAsync(List<AdvertisementImage> Images);
    }
}
