using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication.Core.Domain;

namespace WebApplication.Core.Repositories
{
    public interface IVoivodeshipRepository
    {
        Task<Voivodeship> GetAsync(int Id);
        Task<IEnumerable<Voivodeship>> GetAllAsync();
        Task<City> GetCityAsync(int Id);
        Task<IEnumerable<City>> GetAllCitiesAsync();
        Task<IEnumerable<City>> GetCitiesInVoivodeship(int Id);

        Task<string> GetNameVoivodeship(int Id);  // Pobieranie nazwy województwa dla wybranego miasta
        Task<string> GetNameCity(int Id);         // Pobranie nazwy miasta po jego id
    }
}
