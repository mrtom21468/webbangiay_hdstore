using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using AspNetCoreHero.ToastNotification.Notyf;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using WebApplication7.Models;

namespace WebApplication7.Controllers
{
    public class SanPhamChiTietController : Controller
    {
        private readonly QLDBcontext _context;
        private readonly UserManager<UserIdentitycs> _userManager;
        private readonly NotyfService _notyfService;
        private readonly SignInManager<UserIdentitycs> _SignInManager;

        public SanPhamChiTietController(QLDBcontext context, UserManager<UserIdentitycs> userManager, NotyfService notyfService, SignInManager<UserIdentitycs> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _notyfService = notyfService;
            _SignInManager = signInManager;
        }
        //lấy thông tin số lượng sản phẩm 
        [HttpGet]
        public async Task<JsonResult> GetProductStock(int productId, int sizeid, int colorid)
        {
            // Thực hiện các logic để lấy số lượng hàng dựa trên productId, size, và color
            // Ví dụ:
            try
            {
/*                int? stockQuantity = _context.ProductDetails
                .Where(p => p.Product.ProductId == productId && p.Color.ColorId == colorid && p.Size.SizeId == sizeid)
                .First().Quantity;*/
                decimal? x = _context.ProductDetails
                .Where(p => p.Product.ProductId == productId && p.Color.ColorId == colorid && p.Size.SizeId == sizeid)
                .First().SellingPrice;
                var stockQuantity = _context.ProductDetails
                .Where(p => p.Product.ProductId == productId && p.Color.ColorId == colorid && p.Size.SizeId == sizeid)
                .First().Quantity;
                if (stockQuantity != null && x!=null)
                {
                        return Json(new { x, stockQuantity });
                }
                return Json(new { x="Hết hàng",stockQuantity = "0" });
            }
            catch
            {
                return Json(new { x="Hết hàng",stockQuantity = "0" });

            }
        }
        // GET: SanPhamChiTiet
        public async Task<IActionResult> Index(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Home");
            }
            var list = await _context.ProductDetails
                    .Where(p => p.ProductId == id)
                    .Include(p => p.Color)
                    .Include(c => c.Product)
                    .ThenInclude(b=>b.Brand)
                    .Include(c => c.Product)
                    .ThenInclude(c=>c.Category)
                    .Include(c => c.Size)
                    .Where(p => p.Product.State == true)
                    .ToListAsync();

            if (list.Count > 0)
            {
                var categoryId = list.FirstOrDefault()?.Product?.CategoryId;
                // Kiểm tra xem categoryId có null không, tránh lỗi NullReferenceException
                if (categoryId != null)
                {
                    // Lấy ra tất cả sản phẩm có cùng categoryId với sản phẩm đầu tiên trong danh sách
                    var relatedProducts = _context.Products
                                                   .Include(p => p.ProductDetails)
                                                   .Where(p => p.CategoryId == categoryId &&p.State==true &&p.ProductId != id && p.ProductDetails.Any(pd => pd.Quantity > 0))
                                                   .OrderBy(x => Guid.NewGuid()) // Sắp xếp ngẫu nhiên
                                                   .Take(4)
                                                   .ToList();

                    // Gán danh sách các sản phẩm liên quan vào ViewBag
                    ViewBag.SPlienquan = relatedProducts;
                }
                return View(list);
            }
            else
            {
                _notyfService.Warning("Sản phẩm đã bị xóa hoặc bị ẩn", 3);
                return RedirectToAction("Index", "Home");
            }
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddToCart(int productId, int sizeid, int colorid, int Soluongthem)
        {
            try
            {
                var userid = _userManager.GetUserId(User);
                var user = _context.Accounts.Where(u => u.UserId == userid).Select(u => u.AccountId).FirstOrDefaultAsync();
                //lấy tất cả sản phẩm trong giỏ hàng
                var yourcart = await _context.Carts.Where(c => c.AccountId == user.Result).ToListAsync();
                //lấy sản phẩm cần thêm
                var addProductDetails = await _context.ProductDetails
                .Where(p => p.Product.ProductId == productId && p.Color.ColorId == colorid && p.Size.SizeId == sizeid)
                .FirstOrDefaultAsync();
                if(addProductDetails == null)
                {
                    return Json(new { success = false, message = "Thêm thất bại do không đủ hàng" });
                }
                //kiểm tra xem giỏ hàng có tồn tại sản phẩm cần thêm chưa
                var existingCartItem = yourcart.FirstOrDefault(c => c.ProductdetailId == addProductDetails.ProductdetailId);

                if (existingCartItem != null)
                {
                    //check tiếp là số lượng thêm vào có đủ hàng không 
                    var tongthem = existingCartItem.Amount + Soluongthem;
                    if (tongthem < addProductDetails.Quantity)
                    {
                        //thực hiện thêm
                        existingCartItem.Amount = tongthem;
                        _context.SaveChanges();
                        //cập nhật hiển thị giỏ hàng
                        if (_SignInManager.IsSignedIn(User))
                        {
                            var accId = _context.Accounts
                                .Where(a => a.UserId == _userManager.GetUserId(User))
                                .Select(a => a.AccountId).FirstOrDefault();
                            if (accId != null)
                            {
                                int carCount = _context.Carts.Where(c => c.AccountId == accId).Count();
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
                        return Json(new { success = true, message = "Thêm sản phẩm vào giỏ hàng thành công", carCount = _context.Carts.Where(c => c.AccountId == user.Result).Count() });

                    }
                    else
                    {
                        return Json(new { success = false, message = "Thêm thất bại do không đủ hàng" });
                    }
                }
                else
                {
                    //check xem số lượng thêm vào có đủ nguồn hàng không

                    if (Soluongthem <= addProductDetails.Quantity)
                    {
                        //thực hiện thêm
                        var newcart = new Cart()
                        {
                            AccountId = user.Result,
                            Amount = Soluongthem,
                            ProductdetailId = addProductDetails.ProductdetailId,
                        };
                        await _context.AddAsync(newcart);
                        await _context.SaveChangesAsync();
                      

                        return Json(new { success = true, message = "Thêm sản phẩm vào giỏ hàng thành công", carCount = _context.Carts.Where(c => c.AccountId == user.Result).Count() });
                    }
                    else
                    {
                        //trả về thông báo thêm thất bại
                        return Json(new { success = false, message = "Thêm thất bại do không đủ hàng" });

                    }
                }
            }
            catch
            {
                // Trả về kết quả thất bại
                return Json(new { success = false, message = "Thêm sản phẩm vào giỏ hàng thất bại do hệ thống lỗi" });
            }
        }

    }
}
