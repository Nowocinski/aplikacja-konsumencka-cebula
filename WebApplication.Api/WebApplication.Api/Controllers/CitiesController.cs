using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication.Infrastructure.DTO;
using WebApplication.Infrastructure.Services.Voivodeship;

namespace WebApplication.Api.Controllers
{
    [EnableCors("Origins")]
    [Route("api/[controller]")]
    public class CitiesController : Controller
    {
        private readonly IVoivodeshipService _voivodeshipService;

        public CitiesController(IVoivodeshipService voivodeshipService)
        {
            _voivodeshipService = voivodeshipService;
        }

        // GET: api/cities/id
        [HttpGet("{Id}")]
        public async Task<ActionResult<CityDTO>> GetCity(int Id)
            => Json(await _voivodeshipService.GetCityAsync(Id));

        // GET: api/cities
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CityDTO>>> GetCities()
            => Json(await _voivodeshipService.GetAllCitiesAsync());
    }
}
