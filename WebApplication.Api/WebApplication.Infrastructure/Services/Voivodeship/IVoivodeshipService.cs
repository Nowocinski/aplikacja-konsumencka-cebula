using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication.Infrastructure.DTO;

namespace WebApplication.Infrastructure.Services.Voivodeship
{
    public interface IVoivodeshipService
    {
        Task<VoivodeshipDTO> GetAsync(int Id);
        Task<IEnumerable<VoivodeshipDTO>> GetAllAsync();
        Task<CityDTO> GetCityAsync(int Id);
        Task<IEnumerable<CityDTO>> GetAllCitiesAsync();
        Task<IEnumerable<CityDTO>> GetCitiesInVoivodeship(int Id);
    }
}
