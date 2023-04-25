using Microsoft.AspNetCore.Mvc;

namespace IdentityServer.Controllers
{
    public class AuthenticationController : Controller
    {
        [Route("login")]
        [HttpGet]
        public IActionResult Login(string returnUrl)
        {
            return View();
        }

        [Route("login")]
        [HttpPost]
        public IActionResult Login()
        {
            return View();
        }

        [Route("logout")]
        public IActionResult Logout()
        {
            return Ok();
        }
    }
}
