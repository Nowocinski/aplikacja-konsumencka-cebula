using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication.Core.Domain;
using WebApplication.Core.Domain.Context;
using WebApplication.Core.Repositories;

namespace WebApplication.Infrastructure.Repositories
{
    public class AdminRepository : IAdminRepository
    {
        private readonly DataBaseContext _context;

        public AdminRepository(DataBaseContext context)
        {
            _context = context;
        }

        public async Task<User> GetUserAsync(Guid id)
            => await _context.Users.SingleOrDefaultAsync(x => x.Id == id);

        public async Task<IEnumerable<User>> GetUsersAsync()
            => await _context.Users.ToListAsync();
        public async Task UpdateUserAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            await Task.CompletedTask;
        }

        public async Task<Advertisement> GetAdvertismentsAsync(Guid id)
            => await _context.Advertisements.SingleOrDefaultAsync(x => x.Id == id);

        public async Task<IEnumerable<Advertisement>> GetAdvertisementsAsync()
            => await _context.Advertisements.ToListAsync();
        public async Task UpdateAdvertismentAsync(Advertisement advertisement)
        {
            _context.Advertisements.Update(advertisement);
            await _context.SaveChangesAsync();
            await Task.CompletedTask;
        }
    }
}
