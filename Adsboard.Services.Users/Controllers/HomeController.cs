using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Adsboard.Services.Users.Controllers
{
    [Route("")]
    public class HomeController : ControllerBase
    {
        public HomeController()
        {
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Get()
            => Ok("Adsboard Users Serivce API");

        [HttpGet("ping")]
        [AllowAnonymous]
        public IActionResult Ping()
            => Ok();
    }
}