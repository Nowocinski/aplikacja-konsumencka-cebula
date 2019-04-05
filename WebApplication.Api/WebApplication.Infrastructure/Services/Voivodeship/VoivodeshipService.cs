using AutoMapper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication.Core.Domain;
using WebApplication.Core.Repositories;
using WebApplication.Infrastructure.DTO;

namespace WebApplication.Infrastructure.Services.Voivodeship
{
    public class VoivodeshipService : IVoivodeshipService
    {
        private readonly IVoivodeshipRepository _voivodeshipRepository;
        private readonly IMapper _mapper;

        public VoivodeshipService(IVoivodeshipRepository voivodeshipRepository, IMapper mapper)
        {
            _voivodeshipRepository = voivodeshipRepository;
            _mapper = mapper;
        }

        public async Task<VoivodeshipDTO> GetAsync(int Id)
        {
            var voivodeship = await _voivodeshipRepository.GetAsync(Id);
            return _mapper.Map<VoivodeshipDTO>(voivodeship);
        }

        public async Task<IEnumerable<VoivodeshipDTO>> GetAllAsync()
        {
            var voivodeships = await _voivodeshipRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<VoivodeshipDTO>>(voivodeships);
        }

        public async Task<CityDTO> GetCityAsync(int Id)
        {
            var city = await _voivodeshipRepository.GetCityAsync(Id);
            var cityDTO = _mapper.Map<CityDTO>(city);

            cityDTO.Voivodeship = await _voivodeshipRepository.GetNameVoivodeship(Id);

            return cityDTO;
        }

        public async Task<IEnumerable<CityDTO>> GetAllCitiesAsync()
        {
            var cities = await _voivodeshipRepository.GetAllCitiesAsync();
            var citiesDTO = _mapper.Map<IEnumerable<CityDTO>>(cities);

            foreach(CityDTO c in citiesDTO)
                c.Voivodeship = await _voivodeshipRepository.GetNameVoivodeship(c.Id);

            return citiesDTO;
        }

        public async Task<IEnumerable<CityDTO>> GetCitiesInVoivodeship(int Id)
        {
            var cities = await _voivodeshipRepository.GetCitiesInVoivodeship(Id);
            var citiesDTO = _mapper.Map<IEnumerable<CityDTO>>(cities);

            foreach (CityDTO c in citiesDTO)
                c.Voivodeship = await _voivodeshipRepository.GetNameVoivodeship(c.Id);

            return citiesDTO;
        }
    }
}
