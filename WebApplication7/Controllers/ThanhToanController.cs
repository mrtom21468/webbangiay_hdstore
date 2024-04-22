using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    [Authorize]
    public class ThanhToanController : Controller
    {
        private readonly QLDBcontext _context;
        private readonly NotyfService _notyfService;
        private readonly UserManager<UserIdentitycs> _userManager;

        public ThanhToanController(QLDBcontext context, NotyfService notyfService, UserManager<UserIdentitycs> userManager)
        {
            _context = context;
            _notyfService = notyfService;
            _userManager = userManager;
        }

        // GET: ThanhToan
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
                    if(QLDBcontext!=null)
                    {
                        return View((await QLDBcontext.ToListAsync()));
                    }
                    else
                    {
                        return RedirectToAction("Index","Home");
                    }
                }
                return RedirectToAction("Index", "Home");

            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Home");

            }

        }
        //xử lý thanh toán
        [HttpPost]
        public async Task<IActionResult> XuLyThanhToan(IFormCollection requestions)
        {
            if (requestions == null)
            {
                return NotFound();
            }

            try
            {
                var userid = _userManager.GetUserId(User);
                var user = _context.Accounts.Where(u => u.UserId == userid).Select(u => u.AccountId).FirstOrDefaultAsync();
                // Thêm địa chỉ cho acc
                var newAddress = new Address()
                {
                    AccountId = user.Result,
                    Country = "Việt Nam",
                    City = requestions["tinh"],
                    PhoneNumber = requestions["phone"],
                    FullAddress = requestions["xom"] + requestions["huyen"]
                };
                _context.Add(newAddress);
                await _context.SaveChangesAsync();

                // Tính tổng tiền
                decimal? tongtien = await _context.Carts
                    .Where(c => c.AccountId == user.Result)
                    .SumAsync(p => p.Productdetail.SellingPrice * p.Amount);

                // Thêm hóa đơn mới
                var newOrders = new Order()
                {
                    AccountId = user.Result,
                    Status = "Chờ xử lý",
                    TotalAmount = tongtien,
                    PhoneNumber = requestions["phone"],
                    PaymentStatus = requestions["payment_method"],
                    Address = newAddress.FullAddress + newAddress.City
                };
                await _context.AddAsync(newOrders);
                await _context.SaveChangesAsync();
                ViewBag.Madonhang = newOrders.OrderId;

                // Thêm chi tiết các hóa đơn
                foreach (var cartItem in await _context.Carts.Where(c => c.AccountId == user.Result).ToListAsync())
                {
                    OrderDetail detail = new OrderDetail()
                    {
                        OrderId = newOrders.OrderId,
                        ProductdetailId = cartItem.ProductdetailId,
                        Quantity = cartItem.Amount
                    };
                    await _context.AddAsync(detail);
                }
                await _context.SaveChangesAsync();

                // Xóa các sản phẩm trong giỏ hàng của id
                var itemsToRemove =await  _context.Carts.Where(c => c.AccountId == user.Result).ToListAsync();
                _context.RemoveRange(itemsToRemove);
                await _context.SaveChangesAsync();
                //Xóa sản phẩm chi tiết theo số lượng đã đặt

                return View();
            }
            catch (Exception ex)
            {
                // Xử lý ngoại lệ
                return StatusCode(500, "An error occurred while processing the payment. Please try again later." + ex.Message);
            }
        }
    }
}
