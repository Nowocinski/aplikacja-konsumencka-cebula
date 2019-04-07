﻿using System;
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
        Task RegisterAsync(string FirstName, string LastName, string PhoneNumber, string Email, string Password);
        Task<LoginDTO> LoginAsync(string Email, string Password);
        Task DeleteAsync(Guid Id);
        Task UpdateAsync(Guid Id, UpdateUser User);

        Task<AdvertisementDetailsDTO> GetAdvertisementAsync(Guid Id);
        Task<IEnumerable<AdvertismentDTO>> GetAllAdvertismentsAsync();
        Task AddAdvertisementAsync(CreateAdv Command, Guid UserId);
        Task UpdateAdvertisementAsync(CreateAdv Advertisement, Guid Id);
        Task DeleteAdvertisementAsync(Advertisement Advertisement);
        Task<AdvertisementsWithPageToEndDTO> GetSortAdvertismentsAsync(string parameter, string type, int page);
    }
}
