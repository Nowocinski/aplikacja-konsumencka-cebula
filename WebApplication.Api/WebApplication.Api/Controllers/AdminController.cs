using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication.Infrastructure.DTO;
using WebApplication.Infrastructure.Services.User;
using Microsoft.AspNetCore.Authorization;

namespace WebApplication.Api.Controllers
{
    [EnableCors("Origins")]
    [Authorize(Policy = "HasAdminRole")]
    public class AdminController : ApiControllerBase
    {
        private readonly IAdminService _adminService;
        private readonly IUserService _userService;

        public AdminController(IAdminService adminService, IUserService userService)
        {
            _adminService = adminService;
            _userService = userService;
        }

        // GET: api/admin/users
        [HttpGet("users")]
        public async Task<ActionResult<IEnumerable<AccountDTO>>> GetUsers()
            => Json(await _adminService.GetUsersAsync());

        // PUT: api/admin/users/id
        [HttpPut("users/{id}")]
        public async Task<ActionResult<IEnumerable<AccountDTO>>> GetUsers(Guid id)
        {
            await _adminService.ChangeOfUserStatus(id);
            return NoContent();
        }

        // GET: api/admin/advertisements
        [HttpGet("advertisements")]
        public async Task<ActionResult<IEnumerable<AccountDTO>>> GetAdvertisements()
            => Json(await _adminService.GetAdvertisementsAsync());

        // PUT: api/admin/advertisements/id
        [HttpPut("advertisements/{id}")]
        public async Task<ActionResult<IEnumerable<AccountDTO>>> ChangeStatusAdvertisements(Guid id)
        {
            await _adminService.ChangeOfAdvertismentStatus(id);
            return NoContent();
        }

        // DELETE: api/admin/advertisements/id
        [HttpDelete("advertisements/{id}")]
        public async Task<ActionResult<IEnumerable<AccountDTO>>> DeleteAdvertisements(Guid id)
        {
            AdvertisementDetailsDTO advertisement = await _userService.GetAdvertisementAsync(id);
            if (advertisement == null)
            {
                return NotFound();
            }
            await _userService.DeleteAdvertisementAsync(id);
            return NoContent();
        }
    }
}
