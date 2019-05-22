using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Collections.Generic;

namespace WebApplication.Api.Controllers
{
    [Route("api/[controller]")]
    public class ApiControllerBase : Controller
    {
        protected Guid UserId => User?.Identity?.IsAuthenticated == true ? Guid.Parse(User.Identity.Name) : Guid.Empty;
        protected IEnumerable<Claim> ROle => User.Claims;
    }
}
