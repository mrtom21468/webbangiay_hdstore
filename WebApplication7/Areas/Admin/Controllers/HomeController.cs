using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using WebApplication7.Models;
namespace WebApplication7.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles ="Admin")]
    public class HomeController : Controller
    {
        private readonly QLDBcontext _qLDBcontext;
        public HomeController(QLDBcontext qQLDBcontext)
        {
            _qLDBcontext = qQLDBcontext;
        }
        public IActionResult Index()
        {
            ViewBag.Tongdonhang = _qLDBcontext.Orders.Count();
            ViewBag.Tongdoanhthu = _qLDBcontext.Orders.Sum(order => order.TotalAmount);
            ViewBag.Tongtaikhoan = _qLDBcontext.Accounts.Count();
            ViewBag.Tongsosanphamban = _qLDBcontext.OrderDetails.Sum(or=>or.Quantity);
            return View();
        }
    }
}
