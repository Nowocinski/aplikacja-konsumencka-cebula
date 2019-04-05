using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication.Infrastructure.DTO;
using WebApplication.Infrastructure.Services.User;

namespace WebApplication.Api.Controllers
{
    [Route("api/[controller]")]
    public class AdvertisementsController : Controller
    {
        private readonly IUserService _userService;

        public AdvertisementsController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: api/advertisments/{id} - Pobranie danych ogłoszenia po id
        [HttpGet("{Id}")]
        public async Task<ActionResult<AdvertisementDetailsDTO>> GetAdvertisement(Guid Id)
            => Json(await _userService.GetAdvertisementAsync(Id));

        // GET: api/advertisments/- Pobranie danych wszystkich ogłoszeń
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AdvertismentDTO>>> GetAdvertisement()
            => Json(await _userService.GetAllAdvertismentsAsync());

        // GET: api/advertisments/- Sortowanie ogłoszeń
        [HttpGet("{parameter}/{type}:{page}")]
        public async Task<ActionResult<IEnumerable<AdvertismentDTO>>> GetAdvertisement(string parameter, string type, int page)
            => Json(await _userService.GetSortAdvertismentsAsync(parameter, type, page));
    }
}
