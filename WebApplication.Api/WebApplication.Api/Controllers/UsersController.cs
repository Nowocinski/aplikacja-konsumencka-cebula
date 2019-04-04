using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebApplication.Infrastructure.Commands;
using WebApplication.Infrastructure.Services.User;

namespace WebApplication.Api.Controllers
{
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        // POST: api/users/registration - Rejestracja
        [HttpPost("registration")]
        public async Task<ActionResult> Register([FromBody] Register command)
        {
            await _userService.RegisterAsync(command.FirstName, command.LastName, command.PhoneNumber, command.Email, command.Password);

            return Created("/users", null);
        }

        // POST: api/users/login - Logowanie
        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] Login command)
            => Json(await _userService.LoginAsync(command.Email, command.Password));
    }
}
