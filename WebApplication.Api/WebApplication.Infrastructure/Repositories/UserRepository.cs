using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication.Core.Domain;
using WebApplication.Core.Domain.Context;
using WebApplication.Core.Repositories;
using WebApplication.Infrastructure.Extensions;

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

        public async Task<IEnumerable<Advertisement>> GetAdvertisementsUserAsync(Guid Id)
        {
            var advs = await _context.Advertisements
                .Include(x => x.Images)
                .Include(x => x.Relation)
                .Include(x => x.CityRel)
                    .ThenInclude(x => x.Relation)
                .Where(x => x.UserId == Id).ToListAsync();

            return await Task.FromResult(advs);
        }

        public async Task<Advertisement> GetAdvertisementAsync(Guid Id)
        {
            var adv = await _context.Advertisements
                .Include(x => x.Images)
                .Include(x => x.Relation)
                .Include(x => x.CityRel)
                .SingleOrDefaultAsync(x => x.Id == Id);

            return await Task.FromResult(adv);
        }

        public async Task<IEnumerable<Advertisement>> GetAllAdvertismentsAsync(string text="")
        {
            var advs = await _context.Advertisements.Where(x => x.Title.ToLower().RemoveDiacritics().Contains(text.ToLower()))
                .Include(x => x.Images)
                .Include(x => x.Relation)
                .ToListAsync();

            return await Task.FromResult(advs);
        }

        public async Task AddAdvertisementAsync(Advertisement Advertisement)
        {
            _context.Advertisements.Add(Advertisement);
            await _context.SaveChangesAsync();
            await Task.CompletedTask;
        }

        public async Task UpdateAdvertisementAsync(Advertisement Advertisement)
        {
            _context.Advertisements.Update(Advertisement);
            await _context.SaveChangesAsync();
            await Task.CompletedTask;
        }

        public async Task DeleteAdvertisementAsync(Advertisement Advertisement)
        {
            _context.Advertisements.Remove(Advertisement);
            await _context.SaveChangesAsync();
            await Task.CompletedTask;
        }

        public async Task UpdateMessageAsync(IEnumerable<Message> message)
        {
            foreach(var msg in message)
                _context.Messages.Update(msg);
            await _context.SaveChangesAsync();
            await Task.CompletedTask;
        }

        public async Task<IEnumerable<Message>> GetMessagesAsync(Guid Sender, Guid Recipient)
        {
            var messages = await _context.Messages.Where(x =>
                (x.Sender == Sender && x.Recipient == Recipient) || (x.Sender == Recipient && x.Recipient == Sender))
                .Include(x => x.Relation)
                .OrderByDescending(x => x.Date)
                .ToListAsync();

            return await Task.FromResult(messages);
        }

        public async Task<IEnumerable<Message>> GetConversationListAsync(Guid Id)
        {
            // Jest błąd - trzeba poprawić

            var listConversation = await _context.Messages.Where(x => x.Sender == Id || x.Recipient == Id)
                .Include(x => x.Relation)
                .OrderByDescending(x => x.Date)
                .GroupBy(x => x.Relation.Id != Id)
                .ToListAsync();

            IEnumerable<Message> smths = listConversation.SelectMany(group => group);
            List<Message> newList = smths.ToList();

            return await Task.FromResult(newList);
        }
    }
}
