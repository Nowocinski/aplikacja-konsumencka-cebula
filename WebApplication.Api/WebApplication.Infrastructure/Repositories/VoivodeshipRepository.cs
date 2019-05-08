using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Core.Domain;
using WebApplication.Core.Domain.Context;
using WebApplication.Core.Repositories;

namespace WebApplication.Infrastructure.Repositories
{
    public class VoivodeshipRepository : IVoivodeshipRepository
    {
        private readonly DataBaseContext _context;

        public VoivodeshipRepository(DataBaseContext context)
        {
            _context = context;
        }

        public async Task<Voivodeship> GetAsync(int Id)
            => await Task.FromResult(await _context.Voivodeships.SingleOrDefaultAsync(x => x.Id == Id));

        public async Task<IEnumerable<Voivodeship>> GetAllAsync()
            => await Task.FromResult(await _context.Voivodeships.ToListAsync());

        public async Task<City> GetCityAsync(int Id)
            => await Task.FromResult(await _context.Cities.SingleOrDefaultAsync(x => x.Id == Id));

        public async Task<IEnumerable<City>> GetAllCitiesAsync()
            => await Task.FromResult(await _context.Cities.ToListAsync());

        public async Task<IEnumerable<City>> GetCitiesInVoivodeship(int Id)
            => await Task.FromResult(await _context.Cities.Where(x => x.Voivodeship_Id == Id).ToListAsync());

        public async Task<string> GetNameVoivodeship(int Id)
        {
            City city = await _context.Cities.SingleOrDefaultAsync(x => x.Id == Id);
            Voivodeship voivodeship = await _context.Voivodeships.SingleOrDefaultAsync(x => x.Id == city.Voivodeship_Id);
            return await Task.FromResult(voivodeship.Name);
        }

        public async Task<string> GetNameCity(int Id)
        {
            City city = await _context.Cities.SingleOrDefaultAsync(x => x.Id == Id);
            return await Task.FromResult(city.Name);
        }
    }
}
