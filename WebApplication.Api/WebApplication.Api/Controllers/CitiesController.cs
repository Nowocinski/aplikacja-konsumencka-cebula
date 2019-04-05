using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebApplication.Infrastructure.Services.Voivodeship;

namespace WebApplication.Api.Controllers
{
    [Route("api/[controller]")]
    public class CitiesController : Controller
    {
        private readonly IVoivodeshipService _voivodeshipService;

        public CitiesController(IVoivodeshipService voivodeshipService)
        {
            _voivodeshipService = voivodeshipService;
        }

        // GET: api/cities/{id} - Pobranie danych miasta
        [HttpGet("{Id}")]
        public async Task<ActionResult> GetCity(int Id)
            => Json(await _voivodeshipService.GetCityAsync(Id));

        // GET: api/cities - Pobranie danych wszystkich miast
        [HttpGet]
        public async Task<ActionResult> GetCities()
            => Json(await _voivodeshipService.GetAllCitiesAsync());
    }
}
