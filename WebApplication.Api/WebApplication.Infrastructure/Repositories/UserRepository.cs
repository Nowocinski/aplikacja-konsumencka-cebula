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
            User user = await _context.Users.SingleOrDefaultAsync(x => x.Id == Id);
            return await Task.FromResult(user);
        }

        public async Task<User> GetAsync(string Email)
        {
            User user = await _context.Users.SingleOrDefaultAsync(x => x.Email == Email);
            return await Task.FromResult(user);
        }

        public async Task<User> GetByPhoneAsync(string Phone)
        {
            User user = await _context.Users.SingleOrDefaultAsync(x => x.PhoneNumber == Phone);
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
            List<Advertisement> advertisements = await _context.Advertisements
                .Include(x => x.Images)
                .Include(x => x.User)
                .Include(x => x.City)
                    .ThenInclude(x => x.Voivodeship)
                .Where(x => x.User_Id == Id).ToListAsync();

            return await Task.FromResult(advertisements);
        }

        public async Task<Advertisement> GetAdvertisementAsync(Guid Id)
        {
            Advertisement advertisements = await _context.Advertisements
                .Include(x => x.Images)
                .Include(x => x.User)
                .Include(x => x.City)
                .SingleOrDefaultAsync(x => x.Id == Id);

            return await Task.FromResult(advertisements);
        }

        public async Task<IEnumerable<Advertisement>> GetAllAdvertismentsAsync(string text="")
        {
            List<Advertisement> advertisements = await _context.Advertisements.Where(x => x.Title.ToLower()
                .RemoveDiacritics()
                .Contains(text.ToLower()))
                .Include(x => x.Images)
                .Include(x => x.User)
                .ToListAsync();

            return await Task.FromResult(advertisements);
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
            List<Message> messages = await _context.Messages.Where(x =>
                (x.Sender_Id == Sender && x.Recipient_Id == Recipient) || (x.Sender_Id == Recipient && x.Recipient_Id == Sender))
                .Include(x => x.User)
                .OrderByDescending(x => x.Date)
                .ToListAsync();

            return await Task.FromResult(messages);
        }

        public async Task<IEnumerable<Message>> GetConversationListAsync(Guid Id)
        {
            List<Message> conversations = await _context.Messages.Where(x => x.Recipient_Id == Id)
                .Include(x => x.User)
                .GroupBy(x => x.Sender_Id, (key, group) => group.LastOrDefault())
                .ToListAsync();

            return await Task.FromResult(conversations);
        }
    }
}
