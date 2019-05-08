using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication.Infrastructure.DTO;

namespace WebApplication.Infrastructure.Services.Voivodeship
{
    public interface IVoivodeshipService
    {
        Task<VoivodeshipDTO> GetAsync(int id);
        Task<IEnumerable<VoivodeshipDTO>> GetAllAsync();
        Task<CityDTO> GetCityAsync(int id);
        Task<IEnumerable<CityDTO>> GetAllCitiesAsync();
        Task<IEnumerable<CityDTO>> GetCitiesInVoivodeship(int id);
    }
}
