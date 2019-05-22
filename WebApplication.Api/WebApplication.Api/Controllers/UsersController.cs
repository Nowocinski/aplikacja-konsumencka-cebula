using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication.Core.Models;
using WebApplication.Infrastructure.Commands;
using WebApplication.Infrastructure.DTO;
using WebApplication.Infrastructure.Services.User;

namespace WebApplication.Api.Controllers
{
    [EnableCors("Origins")]
    public class UsersController : ApiControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: api/users/id
        [HttpGet("{Id}")]
        public async Task<ActionResult<AccountDTO>> GetAccount(Guid Id)
            => Json(await _userService.GetAsync(Id));

        // GET: api/users/advertisements/id
        [HttpGet("advertisements/{Id}")]
        public async Task<ActionResult<IEnumerable<AdvertismentDTO>>> GetUserAdvs(Guid Id)
            => Json(await _userService.GetAdvertisementsUserAsync(Id));

        // POST: api/users/registration
        [HttpPost("registration")]
        public async Task<ActionResult> Register([FromBody] Register data)
        {
            await _userService.RegisterAsync(data);
            return Created("/users", null);
        }

        // POST: api/users/login
        [HttpPost("login")]
        public async Task<ActionResult<LoginDTO>> Login([FromBody] Login command)
            => Json(await _userService.LoginAsync(command.Email, command.Password));

        // PUT: api/users/id
        [HttpPut("{Id}")]
        [Authorize]
        public async Task<ActionResult> PutEvent(Guid Id, [FromBody] UpdateUser commend)
        {
            if (UserId != Id)
                return Forbid();
            await _userService.UpdateAsync(Id, commend);
            return NoContent();
        }

        // DELETE: api/users/id
        [HttpDelete("{Id}")]
        [Authorize]
        public async Task<ActionResult> DeleteAsync(Guid Id)
        {
            if(UserId != Id)
                return Forbid();
            await _userService.DeleteAsync(Id);
            return NoContent();
        }

        // GET: api/users/messages/id
        [HttpGet("messages/{Id}")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<MessagesDTO>>> GetMesseges(Guid Id)
            => Json(await _userService.GetMessagesAsync(UserId, Id));

        // GET: api/users/messages/conversations
        [HttpGet("messages/conversations")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<ListConversations>>> GetMesseges()
            => Json(await _userService.GetConversationListAsync(UserId));

        // POST: api/users/messages/id
        [HttpPost("messages/{Id}")]
        [Authorize]
        public async Task<ActionResult> PostMessege(Guid Id, [FromBody] SendMessage message)
        {
            AccountDTO recipient = await _userService.GetAsync(Id);
            if (recipient == null)
                return NotFound();
            await _userService.UpdateMessageAsync(UserId, Id, message.Text);
            return NoContent();
        }
    }
}
