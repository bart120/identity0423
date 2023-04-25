using Duende.IdentityServer;
using Duende.IdentityServer.Events;
using Duende.IdentityServer.Services;
using Duende.IdentityServer.Test;
using IdentityServer.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace IdentityServer.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly IIdentityServerInteractionService _identity;
        private readonly TestUserStore _testUserStore;
        private readonly IEventService _event;

        public AuthenticationController(IIdentityServerInteractionService identity, 
            TestUserStore testUserStore, IEventService events)
        {
            _identity = identity;
            _testUserStore = testUserStore;
            _event = events;
        }

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
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            var context = await _identity.GetAuthorizationContextAsync(model.ReturnUrl);
            if(ModelState.IsValid)
            {
                //authentification
                if (_testUserStore.ValidateCredentials(model.Email, model.Password))
                {
                    var user = _testUserStore.FindByUsername(model.Email);
                    //déclenchement event de réussite d'authentification
                    await _event.RaiseAsync(new UserLoginSuccessEvent(user.Username, user.SubjectId, user.Username, clientId: context.Client.ClientId));

                    //génération du cookie
                    var isuser = new IdentityServerUser(user.SubjectId);
                    //si remember me!
                    var props = new AuthenticationProperties
                    {
                        IsPersistent = true,
                        ExpiresUtc = DateTimeOffset.UtcNow.AddHours(24)
                    };
                    //pose du cookie d'authentification
                    await HttpContext.SignInAsync(isuser, null /*props*/);



                    return Redirect(model.ReturnUrl);
                }
                else
                {
                    ModelState.AddModelError("Email", "Login / mot de passe invalide");
                    await _event.RaiseAsync(new UserLoginFailureEvent(model.Email, "Login / mot de passe invalide", clientId: context.Client.ClientId));
                }

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
