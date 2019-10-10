using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Adsboard.Services.Adverts.Controllers
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
            => Ok("Adverts Serivce API");

        [HttpGet("ping")]
        [AllowAnonymous]
        public IActionResult Ping()
            => Ok();
    }
}