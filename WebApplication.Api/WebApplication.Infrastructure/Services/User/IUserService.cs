using System;
using System.Threading.Tasks;
using WebApplication.Infrastructure.Commands;
using WebApplication.Infrastructure.DTO;

namespace WebApplication.Infrastructure.Services.User
{
    public interface IUserService
    {
        Task<AccountDTO> GetAsync(Guid Id);
        Task RegisterAsync(string FirstName, string LastName, string PhoneNumber, string Email, string Password);
        Task<LoginDTO> LoginAsync(string Email, string Password);
        Task DeleteAsync(Guid Id);
        Task UpdateAsync(Guid Id, UpdateUser User);
    }
}
