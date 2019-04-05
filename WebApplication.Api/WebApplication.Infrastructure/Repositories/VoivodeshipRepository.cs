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
        {
            var voivodeship = await _context.Voivodeships.SingleOrDefaultAsync(x => x.Id == Id);
            return await Task.FromResult(voivodeship);
        }

        public async Task<IEnumerable<Voivodeship>> GetAllAsync()
        {
            var voivodeships = await _context.Voivodeships.ToListAsync();
            return await Task.FromResult(voivodeships);
        }

        public async Task<City> GetCityAsync(int Id)
        {
            var city = await _context.Cities.SingleOrDefaultAsync(x => x.Id == Id);
            return await Task.FromResult(city);
        }

        public async Task<IEnumerable<City>> GetAllCitiesAsync()
        {
            var cites = await _context.Cities.ToListAsync();
            return await Task.FromResult(cites);
        }

        public async Task<IEnumerable<City>> GetCitiesInVoivodeship(int Id)
        {
            var cites = await _context.Cities.Where(x => x.Voivodeship == Id).ToListAsync();
            return await Task.FromResult(cites);
        }

        public async Task<string> GetNameVoivodeship(int Id)
        {
            var city = await _context.Cities.SingleOrDefaultAsync(x => x.Id == Id);
            var voivodeship = await _context.Voivodeships.SingleOrDefaultAsync(x => x.Id == city.Voivodeship);
            return await Task.FromResult(voivodeship.Name);
        }

        public async Task<string> GetNameCity(int Id)
        {
            var city = await _context.Cities.SingleOrDefaultAsync(x => x.Id == Id);
            return await Task.FromResult(city.Name);
        }
    }
}
