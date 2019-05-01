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
    public class VoivodeshipsController : Controller
    {
        private readonly IVoivodeshipService _voivodeshipService;

        public VoivodeshipsController(IVoivodeshipService voivodeshipService)
        {
            _voivodeshipService = voivodeshipService;
        }

        // GET: api/voivodeships/{id} - Pobranie danych danego województwa
        [HttpGet("{Id}")]
        public async Task<ActionResult<VoivodeshipDTO>> GetVoivodeship(int Id)
        {
            VoivodeshipDTO voivodeship;
            try { voivodeship = await _voivodeshipService.GetAsync(Id); }
            catch (Exception e) { return StatusCode(419, new { e.Message }); }

            return Json(voivodeship);
        }

        // GET: api/voivodeships - Pobranie wszystkich województw
        [HttpGet]
        public async Task<ActionResult<IEnumerable<VoivodeshipDTO>>> GetVoivodeships()
        {
            IEnumerable<VoivodeshipDTO> voivodeships;

            try { voivodeships = await _voivodeshipService.GetAllAsync(); }
            catch (Exception e) { return StatusCode(419, new { e.Message }); }

            return Json(voivodeships);
        }

        // GET: api/voivodeships/{id}/cities - Pobranie wysztkich miast w danym województwie
        [HttpGet("{Id}/cities")]
        public async Task<ActionResult<IEnumerable<CityDTO>>> GetAllVoivodeships(int Id)
        {
            IEnumerable<CityDTO> cities;

            try { cities = await _voivodeshipService.GetCitiesInVoivodeship(Id); }
            catch (Exception e) { return StatusCode(419, new { e.Message }); }

            return Json(cities);
        }
    }
}
