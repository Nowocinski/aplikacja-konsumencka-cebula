using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task RegisterAsync(Register data)
        {
            var user = await _userRepository.GetAsync(data.Email);
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
            var existUser = await _userRepository.GetAsync(Id);
            if (existUser == null)
                throw new Exception($"Exception with id: '{Id}' does not exists");

            if (existUser.Password != command.OldPassword.Hash())
                throw new Exception("Exception password: The old password is incorrect");

            var user = await _userRepository.GetAsync(command.Email);

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
        {
            IEnumerable<Advertisement> advertisments = await _userRepository.GetAdvertisementsUserAsync(Id);
            return _mapper.Map<IEnumerable<AdvertismentDTO>>(advertisments);
        }

        public async Task<AdvertisementDetailsDTO> GetAdvertisementAsync(Guid Id)
        {
            Advertisement advertisement = await _userRepository.GetAdvertisementAsync(Id);
            AdvertisementDetailsDTO advertisementDetailsDTO = _mapper.Map<AdvertisementDetailsDTO>(advertisement);

            if (advertisementDetailsDTO == null)
                throw new Exception($"Id '{Id}' advertisement dose not exist.");

            return advertisementDetailsDTO;
        }

        public async Task<IEnumerable<AdvertismentDTO>> GetAllAdvertismentsAsync(string text = "")
        {
            IEnumerable<Advertisement> advertisements = await _userRepository.GetAllAdvertismentsAsync(text);

            return _mapper.Map<IEnumerable<AdvertismentDTO>>(advertisements);
        }

        public async Task AddAdvertisementAsync(CreateAdvertisment Command, Guid UserId)
        {
            Guid advertisement_id = Guid.NewGuid();

            ISet<AdvertisementImage> Imgs = new HashSet<AdvertisementImage>();
            foreach(var I in Command.Images)
                Imgs.Add(new AdvertisementImage(advertisement_id, I.Image, I.Name, I.Description));

            var user = await _userRepository.GetAsync(UserId);
            
            var adv = user.AddAdvertisement(advertisement_id, Command.Title,
                Command.Description, Command.Price, Command.City, Command.Street,
                Command.Size, Command.Category, Imgs, Command.Floor);

            await _userRepository.AddAdvertisementAsync(adv);
        }

        public async Task UpdateAdvertisementAsync(CreateAdvertisment Command, Guid Id)
        {
            Advertisement advertisement = await _userRepository.GetAdvertisementAsync(Id);

            ISet<AdvertisementImage> Imgs = new HashSet<AdvertisementImage>();
            foreach (var I in Command.Images)
                Imgs.Add(new AdvertisementImage(advertisement.Id, I.Image, I.Name, I.Description));

            advertisement.SetTitle(Command.Title);
            advertisement.SetDescription(Command.Description);
            advertisement.SetPrice(Command.Price);
            advertisement.SetCity(Command.City);
            advertisement.SetStreet(Command.Street);
            advertisement.SetSize(Command.Size);
            advertisement.SetCategory(Command.Category);
            advertisement.SetFloor(Command.Floor);
            advertisement.SetAddImages(Imgs);

            await _userRepository.UpdateAdvertisementAsync(advertisement);
        }

        public async Task DeleteAdvertisementAsync(Guid Id)
        {
            var advertisement = await _userRepository.GetAdvertisementAsync(Id);
            await _userRepository.DeleteAdvertisementAsync(advertisement);
        }

        public async Task<AdvertisementsWithPageToEndDTO> GetSortAdvertismentsAsync(string parameter, string type, int page, string text)
        {
            parameter = parameter.ToLower();
            type = type.ToLower();

            var parameters = new string[]{"price","city","size","category","date","title"};
            var types = new string[] { "desc", "asc" };



            if (page <= 0)
                throw new Exception("Number page must be greater than zero");
            if (!types.Contains(type))
                throw new Exception($"Type sort '{type}' do not exist.");
            if (!parameters.Contains(parameter))
                throw new Exception($"Type parameter '{parameter}' do not exist.");

            IEnumerable<AdvertismentDTO> advertisments = await GetAllAdvertismentsAsync(text);

            if(type == "asc")
                switch (parameter)
                {
                    case "price":    advertisments = advertisments.OrderBy(x => x.Price).ToList();    break;
                    case "city":     advertisments = advertisments.OrderBy(x => x.City).ToList();     break;
                    case "size":     advertisments = advertisments.OrderBy(x => x.Size).ToList();     break;
                    case "category": advertisments = advertisments.OrderBy(x => x.Category).ToList(); break;
                    case "date":     advertisments = advertisments.OrderBy(x => x.Date).ToList();     break;
                    case "title":    advertisments = advertisments.OrderBy(x => x.Title).ToList();    break;
                }

            else
                switch (parameter)
                {
                    case "price":    advertisments = advertisments.OrderByDescending(x => x.Price).ToList();    break;
                    case "city":     advertisments = advertisments.OrderByDescending(x => x.City).ToList();     break;
                    case "size":     advertisments = advertisments.OrderByDescending(x => x.Size).ToList();     break;
                    case "category": advertisments = advertisments.OrderByDescending(x => x.Category).ToList(); break;
                    case "date":     advertisments = advertisments.OrderByDescending(x => x.Date).ToList();     break;
                    case "title":    advertisments = advertisments.OrderByDescending(x => x.Title).ToList();    break;
                }

            int pagesToEnd = advertisments.Count();
            if (pagesToEnd % 10 == 0)
                pagesToEnd = pagesToEnd / 10 - page;
            else
                pagesToEnd = pagesToEnd / 10 - page + 1;

            return new AdvertisementsWithPageToEndDTO
            {
                Advertisement = advertisments.Skip(page * 10 - 10).Take(10),
                PageToEnd = pagesToEnd
            };
        }

        public async Task UpdateMessageAsync(Guid sender, Guid recipient, string text)
        {
            var user = await _userRepository.GetAsync(sender);
            user.AddMessage(recipient, text);

            await _userRepository.UpdateMessageAsync(user.Messages);
        }

        public async Task<IEnumerable<MessagesDTO>> GetMessagesAsync(Guid Sender, Guid Recipient)
        {
            IEnumerable<Message> messages = await _userRepository.GetMessagesAsync(Sender, Recipient);
            return _mapper.Map<IEnumerable<MessagesDTO>>(messages);
        }

        public async Task<IEnumerable<ListConversationDTO>> GetConversationListAsync(Guid Id)
        {
            IEnumerable<Message> Conversations = await _userRepository.GetConversationListAsync(Id);
            return _mapper.Map<IEnumerable<ListConversationDTO>>(Conversations);
        }
    }
}
