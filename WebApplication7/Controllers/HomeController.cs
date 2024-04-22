using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebApplication7.Models;

namespace WebApplication7.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly QLDBcontext _QLDBcontext;
        private readonly SignInManager<UserIdentitycs> _SignInManager;
        private readonly UserManager<UserIdentitycs> _UserManager;
        public HomeController(ILogger<HomeController> logger, QLDBcontext QLDBcontext, SignInManager<UserIdentitycs> SignInManager
            , UserManager<UserIdentitycs> userManager)
        {
            _logger = logger;
            _QLDBcontext = QLDBcontext;
            _SignInManager = SignInManager;
            _UserManager = userManager;
        }

        public IActionResult Index()
        {
            if (_SignInManager.IsSignedIn(User))
            {
                var accId = _QLDBcontext.Accounts
                    .Where(a => a.UserId == _UserManager.GetUserId(User))
                    .Select(a => a.AccountId).FirstOrDefault();
                if (accId != null)
                {
                    int carCount = _QLDBcontext.Carts.Where(c => c.AccountId == accId).Count();
                    HttpContext.Session.SetInt32("cartCount", carCount);
                }
                else
                {
                    HttpContext.Session.SetInt32("cartCount", 0);
                }
            }
            else
            {
                HttpContext.Session.SetInt32("cartCount", 0);
            }

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
