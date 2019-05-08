using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
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

        // GET: api/advertisements/id
        [HttpGet("{Id}")]
        public async Task<ActionResult<AdvertisementDetailsDTO>> GetAdvertisement(Guid Id)
            => Json(await _userService.GetAdvertisementAsync(Id));
          
        // GET: api/advertisements/parameter/type:page
        [HttpGet("{parameter}/{type}:{page}")]
        public async Task<ActionResult<AdvertisementsWithPageToEndDTO>>
            GetAdvertisement(string parameter, string type, int page)
            => Json(await _userService.GetFilterAdvertismentsAsync(parameter, type, page));

        // GET: api/advertisements/parameter/type:page/text
        [HttpGet("{parameter}/{type}:{page}/{text}")]
        public async Task<ActionResult<AdvertisementsWithPageToEndDTO>>
            GetFiltrationAdvertisement(string parameter, string type, int page, string text)
            => Json(await _userService.GetFilterAdvertismentsAsync(parameter, type, page, text));

        // POST: api/advertisements
        [HttpPost]
        [Authorize]
        public async Task<ActionResult> PostAdvertisement([FromBody] CreateAdvertisment Command)
        {
            await _userService.AddAdvertisementAsync(Command, UserId);
            return Created("/advertisments", null);
        }

        // PUT: api/advertisements
        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult> PutAdvertisement(Guid Id, [FromBody] CreateAdvertisment Command)
        {
            AdvertisementDetailsDTO advertisement = await _userService.GetAdvertisementAsync(Id);

            if (advertisement == null)
                return NotFound();
            if (advertisement.UserId != UserId)
                return Forbid();
            await _userService.UpdateAdvertisementAsync(Command, Id);
            return NoContent();
        }

        // DELETE: api/advertisements
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult> DeleteAdvertisement(Guid Id)
        {
            AdvertisementDetailsDTO advertisement = await _userService.GetAdvertisementAsync(Id);

            if (advertisement == null)
                return NotFound();
            if (advertisement.UserId != UserId)
                return Forbid();
            await _userService.DeleteAdvertisementAsync(Id);
            return NoContent();
        }
    }
}
