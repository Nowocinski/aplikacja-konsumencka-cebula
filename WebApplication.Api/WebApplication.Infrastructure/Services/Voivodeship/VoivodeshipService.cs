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
            => _mapper.Map<CityDTO>(await _voivodeshipRepository.GetCityAsync(id));

        public async Task<IEnumerable<CityDTO>> GetAllCitiesAsync()
            => _mapper.Map<IEnumerable<CityDTO>>(await _voivodeshipRepository.GetAllCitiesAsync());

        public async Task<IEnumerable<CityDTO>> GetCitiesInVoivodeship(int id)
            => _mapper.Map<IEnumerable<CityDTO>>(await _voivodeshipRepository.GetCitiesInVoivodeship(id));
    }
}
