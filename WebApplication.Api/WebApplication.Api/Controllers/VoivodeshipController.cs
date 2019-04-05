using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebApplication.Infrastructure.Services.Voivodeship;

namespace WebApplication.Api.Controllers
{
    [Route("api/[controller]")]
    public class VoivodeshipsController : Controller
    {
        private readonly IVoivodeshipService _voivodeshipService;

        public VoivodeshipsController(IVoivodeshipService voivodeshipService)
        {
            _voivodeshipService = voivodeshipService;
        }

        // GET: api/voivodeships/{id} - Pobranie danych danego województwa
        [HttpGet("{Id}")]
        public async Task<ActionResult> GetVoivodeship(int Id)
            => Json(await _voivodeshipService.GetAsync(Id));

        // GET: api/voivodeships/{id} - Pobranie wszystkich województw
        [HttpGet]
        public async Task<ActionResult> GetVoivodeships()
            => Json(await _voivodeshipService.GetAllAsync());

        // GET: api/voivodeships/{id}/cities - Pobranie wysztkich miast w danym województwie
        [HttpGet("{Id}/cities")]
        public async Task<ActionResult> GetAllVoivodeships(int Id)
            => Json(await _voivodeshipService.GetCitiesInVoivodeship(Id));
    }
}
