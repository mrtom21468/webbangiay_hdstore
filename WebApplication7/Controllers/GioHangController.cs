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
                List<Cart> cart = _context.Carts.Where(c => c.AccountId == user.Result).ToList();
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
        //cập nhật giỏ hàng
        [HttpPost]
        public IActionResult UpdateQuantity(int productDetailId, int newQuantity)
        {
            try
            {
                // lấy account id cần cập nhật
                var userid = _userManager.GetUserId(User);
                var user = _context.Accounts.Where(u => u.UserId == userid).Select(u => u.AccountId).FirstOrDefaultAsync();
                var cartupdate = _context.Carts.Where(c => c.ProductdetailId == productDetailId && c.AccountId == user.Result).First();
                var Productdetail = _context.ProductDetails.Where(p => p.ProductdetailId == productDetailId && p.Quantity >= newQuantity).FirstOrDefault();
                if (Productdetail != null && cartupdate != null)
                {
                    cartupdate.Amount = newQuantity;
                    _context.Update(cartupdate);
                    _context.SaveChangesAsync();
                    return Json(new { success = true, message = "Cập nhật số lượng sản phẩm thành công" }); // Trả về kết quả thành công
                }
                else
                {
                    return Json(new { success = false, message = "Sản phẩm trong kho không đủ" }); ; // Trả về kết quả thành công
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Cập nhật số lượng sản phẩm thất bại lỗi hệ thống" }); ; // Trả về kết quả thành công
            }
        }
    }
}
