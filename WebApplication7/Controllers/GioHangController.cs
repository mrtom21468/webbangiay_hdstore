using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication7.Models;
using AspNetCoreHero.ToastNotification.Notyf;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using DocumentFormat.OpenXml.Office2010.Excel;


namespace WebApplication7.Controllers
{

    [Authorize]
    public class GioHangController : Controller
    {
        private readonly QLDBcontext _context;
        private readonly NotyfService _notyfService;
        private readonly UserManager<UserIdentitycs> _userManager;

        public GioHangController(QLDBcontext context, NotyfService notyfService, UserManager<UserIdentitycs> userManager)
        {
            _context = context;
            _notyfService= notyfService;
            _userManager= userManager;
        }
        // GET: GioHang
        public async Task<IActionResult> Index()
        {
            try
            {
                var userid = _userManager.GetUserId(User);
                var user = _context.Accounts.Where(u => u.UserId == userid).Select(u => u.AccountId).FirstOrDefaultAsync();
                if (user != null)
                {
                    var QLDBcontext = _context.Carts
                    .Include(c => c.Account)
                    .Include(c => c.Productdetail)
                        .ThenInclude(c => c.Product)
                     .Include(c => c.Productdetail)
                        .ThenInclude(c => c.Color)
                     .Include(c => c.Productdetail)
                        .ThenInclude(c => c.Size)
                    .Where(c => c.Account.AccountId == user.Result);
                    return View((await QLDBcontext.ToListAsync()));
                }
                return View();
            }
            catch (Exception ex)
            {
                return View();
            }
        }
        //xóa sản phẩm khỏi giỏ hàng
        public ActionResult RemoveFromCart(int ProductdetailId)
        {
            try
            {
                var userid = _userManager.GetUserId(User);
                var user = _context.Accounts.Where(u => u.UserId == userid).Select(u => u.AccountId).FirstOrDefaultAsync();
                List<Cart> cart = _context.Carts.Where(c => c.AccountId == 2).ToList();
                var cartremove = _context.Carts.Where(c => c.ProductdetailId == ProductdetailId && c.AccountId == user.Result).First();
                if (cart != null && cartremove != null)
                {
                    _context.Remove(cartremove);
                    _context.SaveChanges();
                }
                _notyfService.Success("Xóa sản phẩm khỏi giỏ hàng thành công", 3);
                // Sau đó, bạn có thể redirect hoặc trả về View tùy thuộc vào yêu cầu
                return RedirectToAction("Index", "GioHang"); // Chuyển hướng đến trang giỏ hàng
            }
            catch
            {
                _notyfService.Warning("Xóa sản phẩm khỏi giỏ hàng thất bại", 3);
                return RedirectToAction("Index", "GioHang"); // Chuyển hướng đến trang giỏ hàng

            }
        }
    }
}
