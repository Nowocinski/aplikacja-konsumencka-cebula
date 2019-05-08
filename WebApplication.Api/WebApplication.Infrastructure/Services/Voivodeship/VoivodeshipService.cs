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

        public async Task<VoivodeshipDTO> GetAsync(int id)
            => _mapper.Map<VoivodeshipDTO>(await _voivodeshipRepository.GetAsync(id));

        public async Task<IEnumerable<VoivodeshipDTO>> GetAllAsync()
            => _mapper.Map<IEnumerable<VoivodeshipDTO>>(await _voivodeshipRepository.GetAllAsync());

        public async Task<CityDTO> GetCityAsync(int id)
        {
            CityDTO cityDTO = _mapper.Map<CityDTO>(await _voivodeshipRepository.GetCityAsync(id));
            cityDTO.Voivodeship = await _voivodeshipRepository.GetNameVoivodeship(id); //###
            return cityDTO;
        }

        public async Task<IEnumerable<CityDTO>> GetAllCitiesAsync()
        { 
            IEnumerable<CityDTO> citiesDTO = _mapper.Map<IEnumerable<CityDTO>>
                (await _voivodeshipRepository.GetAllCitiesAsync());
            foreach(CityDTO city in citiesDTO)
                city.Voivodeship = await _voivodeshipRepository.GetNameVoivodeship(city.Id); //###
            return citiesDTO;
        }

        public async Task<IEnumerable<CityDTO>> GetCitiesInVoivodeship(int id)
        {
            IEnumerable<CityDTO> citiesDTO = _mapper.Map<IEnumerable<CityDTO>>
                (await _voivodeshipRepository.GetCitiesInVoivodeship(id));
            foreach (CityDTO city in citiesDTO)
                city.Voivodeship = await _voivodeshipRepository.GetNameVoivodeship(city.Id); //###
            return citiesDTO;
        }
    }
}
