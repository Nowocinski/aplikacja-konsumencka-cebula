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

        // GET: api/voivodeships/id
        [HttpGet("{Id}")]
        public async Task<ActionResult<VoivodeshipDTO>> GetVoivodeship(int Id)
            => Json(await _voivodeshipService.GetAsync(Id));

        // GET: api/voivodeships
        [HttpGet]
        public async Task<ActionResult<IEnumerable<VoivodeshipDTO>>> GetVoivodeships()
            => Json(await _voivodeshipService.GetAllAsync());

        // GET: api/voivodeships/id/cities
        [HttpGet("{Id}/cities")]
        public async Task<ActionResult<IEnumerable<CityDTO>>> GetAllVoivodeships(int Id)
            => Json(await _voivodeshipService.GetCitiesInVoivodeship(Id));
    }
}
