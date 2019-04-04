using System;
using System.Threading.Tasks;
using WebApplication.Core.Repositories;
using WebApplication.Infrastructure.Commands;
using WebApplication.Infrastructure.DTO;
using WebApplication.Infrastructure.Services.User.JwtToken;

namespace WebApplication.Infrastructure.Services.User
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtHandler _jwtHandler;

        public UserService(IUserRepository userRepository, IJwtHandler jwtHandler)
        {
            _userRepository = userRepository;
            _jwtHandler = jwtHandler;
        }

        public async Task RegisterAsync(string FirstName, string LastName, string PhoneNumber, string Email, string Password)
        {
            var user = await _userRepository.GetAsync(Email);
            if (user != null)
                throw new Exception($"User e-mail: '{Email}' already exists.");

            user = await _userRepository.GetByPhoneAsync(PhoneNumber);
            if (user != null)
                throw new Exception($"User phone number: '{PhoneNumber}' already exists.");

            user = new Core.Domain.User(FirstName, LastName, PhoneNumber, Email, Password);
            await _userRepository.AddAsync(user);
        }

        public async Task<AccountDTO> LoginAsync(string Email, string Password)
        {
            var user = await _userRepository.GetAsync(Email);
            if (user == null)
                throw new Exception("Invalid credentials.");

            if (user.Password != Password)
                throw new Exception("Invalid credentials.");

            var token = _jwtHandler.CreateToken(user.Id);

            return new AccountDTO
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
            user.SetPassword(command.Password);

            await _userRepository.UpdateAsync(user);
        }
    }
}
