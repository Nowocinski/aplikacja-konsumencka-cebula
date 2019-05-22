using AutoMapper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication.Core.Domain;
using WebApplication.Core.Repositories;
using WebApplication.Infrastructure.DTO;
using WebApplication.Infrastructure.Services.User;

namespace WebApplication.Infrastructure.Services.Admin
{
    public class AdminService : IAdminService
    {
        private readonly IAdminRepository _adminRepository;
        private readonly IMapper _mapper;
        public AdminService(IAdminRepository adminRepository, IMapper mapper)
        {
            _adminRepository = adminRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<AccountDTO>> GetUsersAsync()
            => _mapper.Map<IEnumerable<AccountDTO>>(await _adminRepository.GetUsersAsync());

        public async Task ChangeOfUserStatus(Guid id)
        {
            Core.Domain.User user = await _adminRepository.GetUserAsync(id);
            if (user == null)
            {
                return;
            }

            user.ChangeStatus();
            await _adminRepository.UpdateUserAsync(user);
        }

        public async Task<IEnumerable<AdvertismentDTO>> GetAdvertisementsAsync()
            => _mapper.Map<IEnumerable<AdvertismentDTO>>(await _adminRepository.GetAdvertisementsAsync());

        public async Task ChangeOfAdvertismentStatus(Guid id)
        {
            Advertisement advertisement = await _adminRepository.GetAdvertismentsAsync(id);

            if(advertisement == null)
            {
                return;
            }

            advertisement.ChangeSatusVerification();
            await _adminRepository.UpdateAdvertismentAsync(advertisement);
        }
    }
}
