using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication.Core.Domain;
using WebApplication.Core.Domain.Context;
using WebApplication.Core.Repositories;
using WebApplication.Core.Models;
using System.Reflection;

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
            => await _context.Users.SingleOrDefaultAsync(x => x.Id == Id);

        public async Task<User> GetAsync(string Email)
            => await _context.Users.SingleOrDefaultAsync(x => x.Email == Email);

        public async Task<User> GetByPhoneAsync(string Phone)
            => await _context.Users.SingleOrDefaultAsync(x => x.PhoneNumber == Phone);

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
                .Where(x => x.User_Id == Id && x.Verification == true).ToListAsync();

            return await Task.FromResult(advertisements);
        }

        public async Task<Advertisement> GetAdvertisementAsync(Guid Id)
        {
            Advertisement advertisement = await _context.Advertisements
                .Include(x => x.Images)
                .Include(x => x.User)
                .Include(x => x.City)
                .SingleOrDefaultAsync(x => x.Id == Id && x.Verification == true);

            return await Task.FromResult(advertisement);
        }

        public async Task<IEnumerable<Advertisement>> GetFilterAdvertismentsAsync
            (string parameter, string type, int number_page, string text = "")
        {
            List<Advertisement> advertisements = await _context.Advertisements
                .Where(x => x.Title.ToLower().Contains(text.ToLower()) && x.Verification == true)
                .Include(x => x.Images)
                .Include(x => x.City)
                .ToListAsync();

            PropertyInfo property_to_sort = typeof(Advertisement).GetProperty(parameter);

            if (type == "desc")
            {
                advertisements = advertisements
                    .OrderByDescending(x => property_to_sort.GetValue(x))
                    .ToList();
            }
            else
            {
                advertisements = advertisements
                    .OrderBy(x => property_to_sort.GetValue(x))
                    .ToList();
            }

            return await Task.FromResult(advertisements.Skip(number_page * 10 - 10).Take(10));
        }

        public async Task<int> GetAmountOfAdvertismentsAsync()
        {
            int amount = await _context.Advertisements.CountAsync();
            return await Task.FromResult(amount);
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

        public async Task UpdateMessageAsync(IEnumerable<Message> messages)
        {
            foreach(Message message in messages)
                _context.Messages.Update(message);
            await _context.SaveChangesAsync();
            await Task.CompletedTask;
        }

        public async Task<IEnumerable<Message>> GetMessagesAsync(Guid Sender, Guid Recipient)
        {
            List<Message> messages = await _context.Messages.Where(x =>
                (x.Sender_Id == Sender && x.Recipient_Id == Recipient) ||
                (x.Sender_Id == Recipient && x.Recipient_Id == Sender))
                .Include(x => x.Sender)
                .OrderByDescending(x => x.Date)
                .ToListAsync();

            return await Task.FromResult(messages);
        }

        public async Task<IEnumerable<ListConversations>> GetConversationListAsync(Guid Id)
        {
            List<ListConversations> senders = await _context.Messages
                .Where(x => x.Recipient_Id == Id)
                .Include(x => x.Sender)
                .Select(x => new ListConversations
                {
                    UserId = x.Sender_Id,
                    FirstName = x.Sender.FirstName,
                    LastName = x.Sender.LastName,
                    Date = x.Date
                })
                .ToListAsync();

            List<ListConversations> recipients = await _context.Messages
                .Where(x => x.Sender_Id == Id)
                .Include(x => x.Recipient)
                .Select(x => new ListConversations
                {
                    UserId = x.Recipient_Id,
                    FirstName = x.Recipient.FirstName,
                    LastName = x.Recipient.LastName,
                    Date = x.Date
                })
                .ToListAsync();

            IEnumerable<ListConversations> conversation_list = senders.Concat(recipients);
            return await Task.FromResult(conversation_list.GroupBy(x => x.UserId, (key, group) => group.LastOrDefault()));
        }
    }
}
