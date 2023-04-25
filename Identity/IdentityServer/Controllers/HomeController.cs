using Duende.IdentityServer.Services;
using IdentityServer.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace IdentityServer.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IIdentityServerInteractionService _identity;

        public HomeController(ILogger<HomeController> logger, IIdentityServerInteractionService identity)
        {
            _identity = identity;
            _logger = logger;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> Error(string? errorId)
        {
            var message = (await _identity.GetErrorContextAsync(errorId)).Error;
            ViewBag.Message = message;

            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}