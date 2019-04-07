using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using WebApplication.Infrastructure.Commands;
using WebApplication.Infrastructure.DTO;
using WebApplication.Infrastructure.Services.User;

namespace WebApplication.Api.Controllers
{
    public class UsersController : ApiControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: api/users/{id} - Pobranie danych konta
        [HttpGet("{Id}")]
        public async Task<ActionResult> GetAccount(Guid Id)
            => Json(await _userService.GetAsync(Id));

        // GET: api/users/advertisements/{id} - Pobranie danych konta
        [HttpGet("advertisements/{Id}")]
        public async Task<ActionResult> GetUserAdvs(Guid Id)
            => Json(await _userService.GetAdvertisementsUserAsync(Id));

        // POST: api/users/registration - Rejestracja
        [HttpPost("registration")]
        public async Task<ActionResult> Register([FromBody] Register command)
        {
            await _userService.RegisterAsync(command.FirstName, command.LastName, command.PhoneNumber, command.Email, command.Password);

            return Created("/users", null);
        }

        // POST: api/users/login - Logowanie
        [HttpPost("login")]
        public async Task<ActionResult<LoginDTO>> Login([FromBody] Login command)
            => Json(await _userService.LoginAsync(command.Email, command.Password));

        // PUT: api/users/id - Aktualizacja użytkownika
        [HttpPut("{Id}")]
        [Authorize]
        public async Task<ActionResult> PutEvent(Guid Id, [FromBody] UpdateUser commend)
        {
            if (UserId != Id)
                return Forbid();

            await _userService.UpdateAsync(Id, commend);

            return NoContent();
        }

        // DELETE: api/users/{id} - Usuwanie użytkownika
        [HttpDelete("{Id}")]
        [Authorize]
        public async Task<ActionResult> DeleteAsync(Guid Id)
        {
            if(UserId != Id)
                return Forbid();

            await _userService.DeleteAsync(Id);

            return NoContent();
        }

        // GET: api/users/messages/{id} - Pobieranie konwersacji z danym użytkownikiem
        [HttpGet("messages/{Id}")]
        [Authorize]
        public async Task<ActionResult> GetMesseges(Guid Id)
        {
            throw new NotImplementedException();
        }

        // POST: api/users/messages/{id} - Wysyłanie wiadomości
        [HttpPost("messages/{Id}")]
        [Authorize]
        public async Task<ActionResult> PostMessege(Guid Id, [FromBody] Msg message)
        {
            var recipient = await _userService.GetAsync(Id);
            if (recipient == null)
                return NotFound();

            await _userService.UpdateMessageAsync(UserId, Id, message.Text);

            return NoContent();
        }
    }
}
