using IdentityServer.Models;
using Microsoft.AspNetCore.Mvc;

namespace IdentityServer.Controllers
{
    public class AuthenticationController : Controller
    {
        [Route("login")]
        [HttpGet]
        public IActionResult Login(string returnUrl)
        {
            var model = new LoginViewModel();
            model.ReturnUrl= returnUrl;
            return View(model);
        }

        [Route("login")]
        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if(ModelState.IsValid)
            {
                //authentification

            }
            return View(model);
        }

        [Route("logout")]
        public IActionResult Logout()
        {
            return Ok();
        }
    }
}
