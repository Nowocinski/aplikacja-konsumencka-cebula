using Microsoft.AspNetCore.Authorization;
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
    public class AdvertisementsController : ApiControllerBase
    {
        private readonly IUserService _userService;

        public AdvertisementsController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: api/advertisements/{id} - Pobranie danych ogłoszenia po id
        [HttpGet("{Id}")]
        public async Task<ActionResult<AdvertisementDetailsDTO>> GetAdvertisement(Guid Id)
        {
            AdvertisementDetailsDTO advertisement;

            try { advertisement = await _userService.GetAdvertisementAsync(Id); }
            catch (Exception e) { return StatusCode(419, new { e.Message }); }

            return Json(advertisement);
        }

        // GET: api/advertisements/{parameter}/{type}:{page} - Sortowanie ogłoszeń
        [HttpGet("{parameter}/{type}:{page}")]
        public async Task<ActionResult<AdvertisementsWithPageToEndDTO>>
            GetAdvertisement(string parameter, string type, int page)
        {
            AdvertisementsWithPageToEndDTO advertisements;

            try { advertisements = await _userService.GetFilterAdvertismentsAsync(parameter, type, page); }
            catch (Exception e) { return StatusCode(419, new { e.Message }); }

            return Json(advertisements);
        }

        // GET: api/advertisements/{parameter}/{type}:{page}/{text} - Sortowanie ogłoszeń z filtorowaniem po tytule
        [HttpGet("{parameter}/{type}:{page}/{text}")]
        public async Task<ActionResult<AdvertisementsWithPageToEndDTO>>
            GetFiltrationAdvertisement(string parameter, string type, int page, string text)
        {
            AdvertisementsWithPageToEndDTO advertisements;

            try { advertisements = await _userService.GetFilterAdvertismentsAsync(parameter, type, page, text); }
            catch (Exception e) { return StatusCode(419, new { e.Message }); }

            return Json(advertisements);
        }

        // POST: api/advertisements - Dodawanie ogłoszenia
        [HttpPost]
        [Authorize]
        public async Task<ActionResult> PostAdvertisement([FromBody] CreateAdvertisment Command)
        {
            await _userService.AddAdvertisementAsync(Command, UserId);

            //try { await _userService.AddAdvertisementAsync(Command, UserId); }
            //catch (Exception e) { return StatusCode(419, new { e.Message }); }

            return Created("/advertisments", null);
        }

        // PUT: api/advertisements - Aktualizowanie ogłoszenia
        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult> PutAdvertisement(Guid Id, [FromBody] CreateAdvertisment Command)
        {
            try
            {
                AdvertisementDetailsDTO advertisement= await _userService.GetAdvertisementAsync(Id);

                if (advertisement == null)
                    return NotFound();

                if (advertisement.UserId != UserId)
                    return Forbid();

                await _userService.UpdateAdvertisementAsync(Command, Id);
            }
            catch (Exception e) { return StatusCode(419, new { e.Message }); }

            return NoContent();
        }

        // DELETE: api/advertisements - Usuwanie ogłoszenia
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult> DeleteAdvertisement(Guid Id)
        {
            try
            {
                AdvertisementDetailsDTO advertisement = await _userService.GetAdvertisementAsync(Id);

                if (advertisement == null)
                    return NotFound();

                if (advertisement.UserId != UserId)
                    return Forbid();

                await _userService.DeleteAdvertisementAsync(Id);
            }
            catch (Exception e) { return StatusCode(419, new { e.Message }); }

            return NoContent();
        }
    }
}
