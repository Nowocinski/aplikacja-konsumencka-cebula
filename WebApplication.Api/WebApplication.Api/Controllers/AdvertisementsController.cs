using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication.Infrastructure.Commands;
using WebApplication.Infrastructure.DTO;
using WebApplication.Infrastructure.Services.User;

namespace WebApplication.Api.Controllers
{
    public class AdvertisementsController : ApiControllerBase
    {
        private readonly IUserService _userService;

        public AdvertisementsController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: api/advertisments/{id} - Pobranie danych ogłoszenia po id
        [HttpGet("{Id}")]
        public async Task<ActionResult<AdvertisementDetailsDTO>> GetAdvertisement(Guid Id)
        {
            AdvertisementDetailsDTO adv;

            try { adv = await _userService.GetAdvertisementAsync(Id); }
            catch (Exception e) { return StatusCode(419, new { e.Message }); }

            return Json(adv);
        }

        // GET: api/advertisments - Pobranie danych wszystkich ogłoszeń
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AdvertismentDTO>>> GetAdvertisement()
        {
            IEnumerable<AdvertismentDTO> advs;

            try { advs = await _userService.GetAllAdvertismentsAsync(); }
            catch (Exception e) { return StatusCode(419, new  { e.Message}); }

            return Json(advs);
        }

        // GET: api/advertisments/{parameter}/{type}:{page} - Sortowanie ogłoszeń
        [HttpGet("{parameter}/{type}:{page}")]
        public async Task<ActionResult<AdvertisementsWithPageToEndDTO>>
            GetAdvertisement(string parameter, string type, int page)
        {
            AdvertisementsWithPageToEndDTO advs;

            try { advs = await _userService.GetSortAdvertismentsAsync(parameter, type, page); }
            catch (Exception e) { return StatusCode(419, new { e.Message }); }

            return Json(advs);
        }

        // POST: api/advertisments - Dodawanie ogłoszenia
        [HttpPost]
        [Authorize]
        public async Task<ActionResult> PostAdvertisement([FromBody] CreateAdv Command)
        {
            try { await _userService.AddAdvertisementAsync(Command, UserId); }
            catch (Exception e) { return StatusCode(419, new { e.Message }); }

            return Created("/advertisments", null);
        }

        // PUT: api/advertisments - Aktualizowanie ogłoszenia
        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult> PutAdvertisement(Guid Id, [FromBody] CreateAdv Command)
        {
            try
            {
                var adv = await _userService.GetAdvertisementAsync(Id);

                if (adv == null)
                    return NotFound();

                if (adv.UserId != UserId)
                    return Forbid();

                await _userService.UpdateAdvertisementAsync(Command, Id);
            }
            catch (Exception e) { return StatusCode(419, new { e.Message }); }

            return NoContent();
        }

        // DELETE: api/advertisments - Usuwanie ogłoszenia
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult> DeleteAdvertisement(Guid Id)
        {
            try
            {
                var adv = await _userService.GetAdvertisementAsync(Id);

                if (adv == null)
                    return NotFound();

                if (adv.UserId != UserId)
                    return Forbid();

                await _userService.DeleteAdvertisementAsync(Id);
            }
            catch (Exception e) { return StatusCode(419, new { e.Message }); }

            return NoContent();
        }
    }
}
