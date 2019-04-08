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

        // GET: api/cities/{id} - Pobranie danych miasta
        [HttpGet("{Id}")]
        public async Task<ActionResult<CityDTO>> GetCity(int Id)
        {
            CityDTO city;

            try { city = await _voivodeshipService.GetCityAsync(Id); }
            catch (Exception e) { return StatusCode(419, new { e.Message }); }

            return Json(city);
        }

        // GET: api/cities - Pobranie danych wszystkich miast
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CityDTO>>> GetCities()
        {
            IEnumerable<CityDTO> cities; 

            try { cities = await _voivodeshipService.GetAllCitiesAsync(); }
            catch (Exception e) { return StatusCode(419, new { e.Message }); }

            return Json(cities);
        }
    }
}
