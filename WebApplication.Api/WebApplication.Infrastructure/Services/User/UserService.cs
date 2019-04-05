using AutoMapper;
using System;
using System.Threading.Tasks;
using WebApplication.Core.Domain;
using WebApplication.Core.Repositories;
using WebApplication.Infrastructure.Commands;
using WebApplication.Infrastructure.DTO;
using WebApplication.Infrastructure.Extensions;
using WebApplication.Infrastructure.Services.User.JwtToken;

namespace WebApplication.Infrastructure.Services.User
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtHandler _jwtHandler;
        private readonly IMapper _mapper;
        private readonly IVoivodeshipRepository _voivodeshipRepository;

        public UserService(IUserRepository userRepository, IJwtHandler jwtHandler, IMapper mapper, IVoivodeshipRepository voivodeshipRepository)
        {
            _userRepository = userRepository;
            _jwtHandler = jwtHandler;
            _mapper = mapper;
            _voivodeshipRepository = voivodeshipRepository;
        }

        public async Task<AccountDTO> GetAsync(Guid Id)
        {
            var user = await _userRepository.GetAsync(Id);
            return _mapper.Map<AccountDTO>(user);
        }

        public async Task RegisterAsync(string FirstName, string LastName, string PhoneNumber, string Email, string Password)
        {
            var user = await _userRepository.GetAsync(Email);
            if (user != null)
                throw new Exception($"User e-mail: '{Email}' already exists.");

            user = await _userRepository.GetByPhoneAsync(PhoneNumber);
            if (user != null)
                throw new Exception($"User phone number: '{PhoneNumber}' already exists.");

            user = new Core.Domain.User(FirstName, LastName, PhoneNumber, Email, Password.Hash());
            await _userRepository.AddAsync(user);
        }

        public async Task<LoginDTO> LoginAsync(string Email, string Password)
        {
            var user = await _userRepository.GetAsync(Email);
            if (user == null)
                throw new Exception("Invalid credentials.");

            if (user.Password != Password.Hash())
                throw new Exception("Invalid credentials.");

            var token = _jwtHandler.CreateToken(user.Id);

            return new LoginDTO
            {
                Token = token,
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber
            };
        }

        public async Task DeleteAsync(Guid Id)
        {
            var user = await _userRepository.GetAsync(Id);
            if (user == null)
                throw new Exception($"Exception id: '{Id}' does not exists");

            await _userRepository.DeleteAsync(user);
        }

        public async Task UpdateAsync(Guid Id, UpdateUser command)
        {
            var user = await _userRepository.GetAsync(command.Email);

            if (user != null)
                throw new Exception($"Exception with e-mail: '{command.Email}' already exists");

            user = await _userRepository.GetByPhoneAsync(command.PhoneNumber);
            if (user != null)
                throw new Exception($"Exception with this phone number: '{command.PhoneNumber}' already exists");

            user = await _userRepository.GetAsync(Id);
            if (user == null)
                throw new Exception($"Exception with id: '{Id}' does not exists");

            user.SetFirstName(command.FirstName);
            user.SetLastName(command.LastName);
            user.SetPhoneNumber(command.PhoneNumber);
            user.SetEmail(command.Email);
            user.SetPassword(command.Password.Hash());

            await _userRepository.UpdateAsync(user);
        }

        public async Task<AdvertisementDetailsDTO> GetAdvertisementAsync(Guid Id)
        {
            var advertisement = await _userRepository.GetAdvertisementAsync(Id);
            var advDitDTO = _mapper.Map<AdvertisementDetailsDTO>(advertisement);

            advDitDTO.City = await _voivodeshipRepository.GetNameCity(advertisement.City);

            var user = await _userRepository.GetAsync(advertisement.UserId);

            advDitDTO = _mapper.Map(user, advDitDTO);

            return advDitDTO;
        }

        public async Task AddAdvertisementAsync(Advertisement Advertisement)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateAdvertisementAsync(Advertisement Advertisement)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteAdvertisementAsync(Advertisement Advertisement)
        {
            throw new NotImplementedException();
        }
    }
}
