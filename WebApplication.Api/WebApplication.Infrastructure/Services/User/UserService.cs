using AutoMapper;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using WebApplication.Core.Domain;
using WebApplication.Core.Models;
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

        public UserService
            (IUserRepository userRepository, IJwtHandler jwtHandler, IMapper mapper,
            IVoivodeshipRepository voivodeshipRepository)
        {
            _userRepository = userRepository;
            _jwtHandler = jwtHandler;
            _mapper = mapper;
            _voivodeshipRepository = voivodeshipRepository;
        }

        public async Task<AccountDTO> GetAsync(Guid Id)
            => _mapper.Map<AccountDTO>(await _userRepository.GetAsync(Id));

        public async Task RegisterAsync(Register data)
        {
            Core.Domain.User user = await _userRepository.GetAsync(data.Email);
            if (user != null)
                throw new Exception($"User e-mail: '{data.Email}' already exists.");
            user = await _userRepository.GetByPhoneAsync(data.PhoneNumber);
            if (user != null)
                throw new Exception($"User phone number: '{data.PhoneNumber}' already exists.");
            user = new Core.Domain.User(data.FirstName, data.LastName, data.PhoneNumber, data.Email, data.Password.Hash());
            SendEmailExtensions.SendEmail(data.Email);
            await _userRepository.AddAsync(user);
        }

        public async Task<LoginDTO> LoginAsync(string Email, string Password)
        {
            Core.Domain.User user = await _userRepository.GetAsync(Email);
            if (user == null)
                return null;
            if (user.Password != Password.Hash())
                return null;
            string token = _jwtHandler.CreateToken(user.Id);

            return new LoginDTO
            {
                Token = token,
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Role = user.Role
            };
        }

        public async Task DeleteAsync(Guid Id)
        {
            Core.Domain.User user = await _userRepository.GetAsync(Id);
            if (user == null)
                throw new Exception($"Exception id: '{Id}' does not exists");
            await _userRepository.DeleteAsync(user);
        }

        public async Task UpdateAsync(Guid Id, UpdateUser command)
        {
            Core.Domain.User existUser = await _userRepository.GetAsync(Id);
            if (existUser == null)
                throw new Exception($"Exception with id: '{Id}' does not exists");
            if (existUser.Password != command.OldPassword.Hash())
                throw new Exception("Exception password: The old password is incorrect");

            Core.Domain.User user = await _userRepository.GetAsync(command.Email);
            if (user != null && user != existUser)
                throw new Exception($"Exception with e-mail: '{command.Email}' already exists");

            user = await _userRepository.GetByPhoneAsync(command.PhoneNumber);
            if (user != null && user != existUser)
                throw new Exception($"Exception with this phone number: '{command.PhoneNumber}' already exists");
            if(command.FirstName != null)
                existUser.SetFirstName(command.FirstName);
            if(command.LastName != null)
                existUser.SetLastName(command.LastName);
            if(command.PhoneNumber != null)
                existUser.SetPhoneNumber(command.PhoneNumber);
            if(command.Email != null)
                existUser.SetEmail(command.Email);
            if(command.Password != null)
                existUser.SetPassword(command.Password.Hash());
            await _userRepository.UpdateAsync(existUser);
        }

        public async Task<IEnumerable<AdvertismentDTO>> GetAdvertisementsUserAsync(Guid Id)
            => _mapper.Map<IEnumerable<AdvertismentDTO>>(await _userRepository.GetAdvertisementsUserAsync(Id));

        public async Task<AdvertisementDetailsDTO> GetAdvertisementAsync(Guid Id)
        {
            Advertisement advertisement = await _userRepository.GetAdvertisementAsync(Id);
            AdvertisementDetailsDTO advertisementDetailsDTO = _mapper.Map<AdvertisementDetailsDTO>(advertisement);
            if (advertisementDetailsDTO == null)
                throw new Exception($"Id '{Id}' advertisement dose not exist.");
            return advertisementDetailsDTO;
        }

        public async Task<AdvertisementsWithPageToEndDTO> GetFilterAdvertismentsAsync
            (string parameter, string type, int number_page, string text = "")
        {
            if (number_page <= 0)
                throw new Exception("Number page must be greater than zero");
            if (type != "asc" && type != "desc")
                throw new Exception($"Type sort '{type}' do not exist.");

            parameter = char.ToUpper(parameter[0]) + parameter.Substring(1);
            PropertyInfo property = typeof(Advertisement).GetProperty(parameter);
            if (property == null)
                throw new Exception($"Parameter '{parameter}' do not exist.");

            IEnumerable<Advertisement> advertisements = await _userRepository
                .GetFilterAdvertismentsAsync(parameter, type, number_page, text);

            int pagesToEnd = await _userRepository.GetAmountOfAdvertismentsAsync();
            if (pagesToEnd % 10 == 0)
                pagesToEnd = pagesToEnd / 10 - number_page;
            else
                pagesToEnd = pagesToEnd / 10 - number_page + 1;

            return new AdvertisementsWithPageToEndDTO
            {
                Advertisement = _mapper.Map<IEnumerable<AdvertismentDTO>>(advertisements),
                PageToEnd = pagesToEnd
            };
        }
        public async Task AddAdvertisementAsync(CreateAdvertisment Command, Guid UserId)
        {
            Guid advertisement_id = Guid.NewGuid();

            ISet<AdvertisementImage> Images = new HashSet<AdvertisementImage>();
            foreach(var Image in Command.Images)
                Images.Add(new AdvertisementImage(advertisement_id, Image.Image, Image.Name, Image.Description));

            Core.Domain.User user = await _userRepository.GetAsync(UserId);
            Advertisement advertisement = user.AddAdvertisement(advertisement_id, Command.Title,
                Command.Description, Command.Price, Command.City, Command.Street,
                Command.Size, Command.Category, Images, Command.Floor);
            await _userRepository.AddAdvertisementAsync(advertisement);
        }

        public async Task UpdateAdvertisementAsync(CreateAdvertisment Command, Guid Id)
        {
            Advertisement advertisement = await _userRepository.GetAdvertisementAsync(Id);

            ISet<AdvertisementImage> Images = new HashSet<AdvertisementImage>();
            foreach (var Image in Command.Images)
                Images.Add(new AdvertisementImage(advertisement.Id, Image.Image, Image.Name, Image.Description));

            advertisement.SetTitle(Command.Title);
            advertisement.SetDescription(Command.Description);
            advertisement.SetPrice(Command.Price);
            advertisement.SetCity(Command.City);
            advertisement.SetStreet(Command.Street);
            advertisement.SetSize(Command.Size);
            advertisement.SetCategory(Command.Category);
            advertisement.SetFloor(Command.Floor);
            advertisement.SetAddImages(Images);

            await _userRepository.UpdateAdvertisementAsync(advertisement);
        }

        public async Task DeleteAdvertisementAsync(Guid id)
        {
            Advertisement advertisement = await _userRepository.GetAdvertisementAsync(id);
            await _userRepository.DeleteAdvertisementAsync(advertisement);
        }

        public async Task UpdateMessageAsync(Guid sender, Guid recipient, string text)
        {
            var user = await _userRepository.GetAsync(sender);
            user.AddMessage(recipient, text);

            await _userRepository.UpdateMessageAsync(user.Messages);
        }

        public async Task<IEnumerable<MessagesDTO>> GetMessagesAsync(Guid sender, Guid recipient)
            => _mapper.Map<IEnumerable<MessagesDTO>>(await _userRepository.GetMessagesAsync(sender, recipient));

        public async Task<IEnumerable<ListConversations>> GetConversationListAsync(Guid id)
            => await _userRepository.GetConversationListAsync(id);
    }
}
