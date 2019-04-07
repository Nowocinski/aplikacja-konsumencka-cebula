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
            advDitDTO.FirstName = advertisement.Relation.FirstName;

            if (advDitDTO == null)
                throw new Exception($"Id '{Id}' advertisement dose not exist.");

            return advDitDTO;
        }

        public async Task<IEnumerable<AdvertismentDTO>> GetAllAdvertismentsAsync()
        {
            var advs = await _userRepository.GetAllAdvertismentsAsync();
            var advsDTO = _mapper.Map<IEnumerable<AdvertismentDTO>>(advs);

            foreach(var a in advsDTO)
                a.City = await _voivodeshipRepository.GetNameCity(int.Parse(a.City));

            return advsDTO;
        }

        public async Task AddAdvertisementAsync(CreateAdv Command, Guid UserId)
        {
            Guid AdvId = Guid.NewGuid();

            ISet<AdvertisementImage> Imgs = new HashSet<AdvertisementImage>();
            foreach(var I in Command.Images)
                Imgs.Add(new AdvertisementImage(AdvId, I.Image, I.Name, I.Description));

            var user = await _userRepository.GetAsync(UserId);
            
            var adv = user.AddAdvertisement(AdvId, Command.Title, Command.Description, Command.Price, Command.City, Command.Street,
                Command.Size, Command.Category, Imgs, Command.Floor);

            await _userRepository.AddAdvertisementAsync(adv);
        }

        public async Task UpdateAdvertisementAsync(Advertisement Advertisement)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteAdvertisementAsync(Advertisement Advertisement)
        {
            throw new NotImplementedException();
        }

        public async Task<AdvertisementsWithPageToEndDTO> GetSortAdvertismentsAsync(string parameter, string type, int page)
        {
            parameter = parameter.ToLower();
            type = type.ToLower();

            var parameters = new string[]{"price","city","size","category","date","title"};
            var types = new string[] { "desc", "asc" };

            if (page <= 0)
                throw new Exception("Number page must be greater than zero");
            if (!types.Contains(type))
                throw new Exception($"Type sort '{type}' do not exist.");
            if (!types.Contains(type))
                throw new Exception($"Type sort '{type}' do not exist.");

            var adv = await GetAllAdvertismentsAsync();

            if(type == "asc")
                switch (parameter)
                {
                    case "price":    adv = adv.OrderBy(x => x.Price).ToList();    break;
                    case "city":     adv = adv.OrderBy(x => x.City).ToList();     break;
                    case "size":     adv = adv.OrderBy(x => x.Size).ToList();     break;
                    case "category": adv = adv.OrderBy(x => x.Category).ToList(); break;
                    case "date":     adv = adv.OrderBy(x => x.Date).ToList();     break;
                    case "title":    adv = adv.OrderBy(x => x.Title).ToList();    break;
                }

            else
                switch (parameter)
                {
                    case "price":    adv = adv.OrderByDescending(x => x.Price).ToList();    break;
                    case "city":     adv = adv.OrderByDescending(x => x.City).ToList();     break;
                    case "size":     adv = adv.OrderByDescending(x => x.Size).ToList();     break;
                    case "category": adv = adv.OrderByDescending(x => x.Category).ToList(); break;
                    case "date":     adv = adv.OrderByDescending(x => x.Date).ToList();     break;
                    case "title":    adv = adv.OrderByDescending(x => x.Title).ToList();    break;
                }

            int pagesToEnd = adv.Count();
            if (pagesToEnd % 10 == 0)
                pagesToEnd = pagesToEnd / 10 - page;
            else
                pagesToEnd = pagesToEnd / 10 - page + 1;

            return new AdvertisementsWithPageToEndDTO
            {
                Advertisement = adv.Skip(page * 10 - 10).Take(10),
                PageToEnd = pagesToEnd
            };
        }
    }
}
