using Microsoft.AspNetCore.Mvc;
using System;

namespace WebApplication.Api.Controllers
{
    [Route("api/[controller]")]
    public class ApiControllerBase : Controller
    {
        protected Guid UserId => User?.Identity?.IsAuthenticated == true ? Guid.Parse(User.Identity.Name) : Guid.Empty;
    }
}
