﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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

        // GET: api/users/{id} - Pobranie danych konta
        [HttpGet("{Id}")]
        public async Task<ActionResult<AccountDTO>> GetAccount(Guid Id)
        {
            AccountDTO account;

            try { account = await _userService.GetAsync(Id); }
            catch (Exception e) { return StatusCode(419, new { e.Message }); }

            return Json(account);
        }

        // GET: api/users/advertisements/{id} - Pobranie danych konta
        [HttpGet("advertisements/{Id}")]
        public async Task<ActionResult<IEnumerable<AdvertismentDTO>>> GetUserAdvs(Guid Id)
        {
            IEnumerable<AdvertismentDTO> adv;

            try { adv = await _userService.GetAdvertisementsUserAsync(Id); }
            catch (Exception e) { return StatusCode(419, new { e.Message }); }

            return Json(adv);
        }

        // POST: api/users/registration - Rejestracja
        [HttpPost("registration")]
        public async Task<ActionResult> Register([FromBody] Register data)
        {
            try { await _userService.RegisterAsync(data); }
            catch (Exception e) { return StatusCode(419, new { e.Message }); }

            return Created("/users", null);
        }

        // POST: api/users/login - Logowanie
        [HttpPost("login")]
        public async Task<ActionResult<LoginDTO>> Login([FromBody] Login command)
        //=> Json(await _userService.LoginAsync(command.Email, command.Password));
        {
            LoginDTO login_data;
            try { login_data = await _userService.LoginAsync(command.Email, command.Password); }
            catch (Exception e) { return StatusCode(419, new { e.Message }); }

            return Json(login_data);
        }

        // PUT: api/users/id - Aktualizacja użytkownika
        [HttpPut("{Id}")]
        [Authorize]
        public async Task<ActionResult> PutEvent(Guid Id, [FromBody] UpdateUser commend)
        {
            if (UserId != Id)
                return Forbid();

            try { await _userService.UpdateAsync(Id, commend); }
            catch (Exception e) { return StatusCode(419, new { e.Message }); }

            return NoContent();
        }

        // DELETE: api/users/{id} - Usuwanie użytkownika
        [HttpDelete("{Id}")]
        [Authorize]
        public async Task<ActionResult> DeleteAsync(Guid Id)
        {
            if(UserId != Id)
                return Forbid();

            try { await _userService.DeleteAsync(Id); }
            catch (Exception e) { return StatusCode(419, new { e.Message }); }

            return NoContent();
        }

        // GET: api/users/messages/{id} - Pobieranie konwersacji z danym użytkownikiem
        [HttpGet("messages/{Id}")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<MessagesDTO>>> GetMesseges(Guid Id)
        {
            return Json(await _userService.GetMessagesAsync(UserId, Id));
        }

        // GET: api/users/messages/conversations - Pobieranie konwersacji z danym użytkownikiem
        [HttpGet("messages/conversations")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<ListConversationDTO>>> GetMesseges()
        {
            return Json(await _userService.GetConversationListAsync(UserId));
        }

        // POST: api/users/messages/{id} - Wysyłanie wiadomości
        [HttpPost("messages/{Id}")]
        [Authorize]
        public async Task<ActionResult> PostMessege(Guid Id, [FromBody] SendMessage message)
        {
            try
            {
                AccountDTO recipient = await _userService.GetAsync(Id);
                if (recipient == null)
                    return NotFound();

                await _userService.UpdateMessageAsync(UserId, Id, message.Text);
            }
            catch (Exception e) { return StatusCode(419, new { e.Message }); }

            return NoContent();
        }
    }
}
