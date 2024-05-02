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
using WebApplication7.Areas.Admin.Helpper;
using System.Security.Claims;
using System.Text;

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

                var userid = _userManager.GetUserId(User);
                var user = _context.Accounts.Where(u => u.UserId == userid).Select(u => u.AccountId).FirstOrDefaultAsync();
                // Thêm địa chỉ cho acc
                var newAddress = new Address()
                {
                    AccountId = user.Result,
                    Country = "Việt Nam",
                    City = requestions["tinh"],
                    PhoneNumber = requestions["phone"],
                    FullAddress = requestions["chitietdiachi"] +"-"+ requestions["huyen"]
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
                    PaymentType = requestions["payment_method"],
                    PaymentStatus="0",
                    Address = newAddress.FullAddress +"-"+ newAddress.City
                };
                await _context.AddAsync(newOrders);
                await _context.SaveChangesAsync();
                ViewBag.Madonhang = newOrders.OrderId;

                // Thêm chi tiết các hóa đơn
                foreach (var cartItem in await _context.Carts.Where(c => c.AccountId == user.Result).Include(p=>p.Productdetail).ToListAsync())
                {
                    OrderDetail detail = new OrderDetail()
                    {
                        OrderId = newOrders.OrderId,
                        ProductdetailId = cartItem.ProductdetailId,
                        PriceAtOrderTime= cartItem.Productdetail.SellingPrice,
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

                //Gửi thông báo về mail người dùng
                var userClaims = User.Claims;
                var emailClaim = userClaims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
                var thongtin = CreateOrderSummaryTable(_context.OrderDetails.Include(p=>p.Productdetail).ThenInclude(p=>p.Product)
                    .Include(p => p.Productdetail).ThenInclude(c=>c.Color).Include(p=>p.Productdetail).ThenInclude(s=>s.Size).Where(o => o.OrderId == newOrders.OrderId).ToList());
                NotyfGmail.SenGmail(emailClaim, newOrders.OrderId, thongtin);
                return View();
            
        }
        public string CreateOrderSummaryTable(List<OrderDetail> orderDetails)
        {
            // Tạo biến string chứa nội dung bảng
            StringBuilder table = new StringBuilder();

            // Thêm phần đầu của bảng HTML
            table.AppendLine("<table>");
            table.AppendLine("<thead>");
            table.AppendLine("<tr>");
            table.AppendLine("<th>STT</th>");
            table.AppendLine("<th>Tên sản phẩm</th>");
            table.AppendLine("<th>Đơn giá</th>");
            table.AppendLine("<th>Số lượng</th>");
            table.AppendLine("<th>Thành tiền</th>");
            table.AppendLine("</tr>");
            table.AppendLine("</thead>");
            table.AppendLine("<tbody>");

            // Duyệt qua mỗi chi tiết đơn hàng và thêm thông tin vào bảng
            int stt = 1;
            decimal? totalAmount = 0;
            foreach (var detail in orderDetails)
            {
                decimal? totalPrice = detail.PriceAtOrderTime * detail.Quantity;
                table.AppendLine("<tr>");
                table.AppendLine($"<td>{stt}</td>");
                table.AppendLine($"<td>{detail.Productdetail.Product.ProductName} - {detail.Productdetail.Color.ColorName} - {detail.Productdetail.Size.SizeName} </td>");
                table.AppendLine($"<td>${detail.PriceAtOrderTime}</td>");
                table.AppendLine($"<td>{detail.Quantity}</td>");
                table.AppendLine($"<td>${totalPrice}</td>");
                table.AppendLine("</tr>");
                totalAmount += totalPrice;
                stt++;
            }

            // Thêm phần tổng tiền vào bảng
            table.AppendLine("</tbody>");
            table.AppendLine("</table>");
            table.AppendLine("<p>Tổng tiền: $" + totalAmount + "</p>");

            // Trả về bảng dưới dạng string
            return table.ToString();
        }
    }
}
