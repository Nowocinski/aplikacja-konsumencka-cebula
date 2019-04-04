using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using WebApplication.Core.Domain;
using WebApplication.Core.Domain.Context;
using WebApplication.Core.Repositories;

namespace WebApplication.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DataBaseContext _context;

        public UserRepository(DataBaseContext context)
        {
            _context = context;
        }

        public async Task<User> GetAsync(Guid Id)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.Id == Id);
            return await Task.FromResult(user);
        }

        public async Task<User> GetAsync(string Email)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.Email == Email);
            return await Task.FromResult(user);
        }

        public async Task<User> GetByPhoneAsync(string Phone)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.PhoneNumber == Phone);
            return await Task.FromResult(user);
        }

        public async Task AddAsync(User User)
        {
            _context.Users.Add(User);
            await _context.SaveChangesAsync();
            await Task.CompletedTask;
        }

        public async Task UpdateAsync(User User)
        {
            _context.Users.Update(User);
            await _context.SaveChangesAsync();
            await Task.CompletedTask;
        }

        public async Task DeleteAsync(User User)
        {
            _context.Users.Remove(User);
            await _context.SaveChangesAsync();
            await Task.CompletedTask;
        }
    }
}
