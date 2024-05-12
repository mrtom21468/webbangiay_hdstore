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
using Newtonsoft.Json.Linq;
using Store_HD.Models;
using Azure.Core;
using DocumentFormat.OpenXml.Drawing.Charts;

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
                    if(QLDBcontext.Count()>0)
                    {
                        return View((await QLDBcontext.ToListAsync()));
                    }
                    else
                    {
                        _notyfService.Warning("Vui lòng chọn sản phẩm để thanh toán");
                        return RedirectToAction("Index","GioHang");
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
                var carts = await _context.Carts.Where(c => c.AccountId == user.Result).Include(p => p.Productdetail).ThenInclude(p => p.Product).ToListAsync();
                if (carts.Count<=0)
                {
                _notyfService.Warning("Vui lòng chọn sản phẩm để thanh toán!",5);
                return RedirectToAction("Index", "SanPhamShop", new { area = "" });
                }
                //check số lượng sản phẩm trong kho có còn đủ không
                foreach (var cartItem in carts)
                {
                var sp = _context.ProductDetails.Where(p => p.ProductdetailId == cartItem.ProductdetailId)
                    .Include(p=>p.Product)
                    .Include(c=>c.Color)
                    .Include(s=>s.Size)
                    .FirstOrDefault();
                if (sp.Quantity < cartItem.Amount)
                {
                    var err = string.Format("Sản phẩm {0}-{1}-{2} không đủ hàng! SL còn: {3}", sp.Product.ProductName,sp.Color.ColorName, sp.Size.SizeName, sp.Quantity);
                    if (sp.Quantity <= 0)
                    {
                        //xóa sản phẩm khỏi giỏ hàng đó
                        _context.Remove(cartItem);
                        _context.SaveChanges();
                    }
                    cartItem.Amount = 1;
                    _notyfService.Error(err, 5);
                    return RedirectToAction("Index", "GioHang", new { area = "" });
                }
                }
                //check thành công tiến hành giảm số lượng sản phẩm shop
                foreach (var cartItem in carts)
                {
                    var sp = _context.ProductDetails.Where(p => p.ProductdetailId == cartItem.ProductdetailId).FirstOrDefault();
                    sp.Quantity-=cartItem.Amount;
                    _context.Update(sp);
                }
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
                var newOrders = new Models.Order()
                {
                    AccountId = user.Result,
                    Status = "1",
                    TotalAmount = tongtien,
                    PhoneNumber = requestions["phone"],
                    PaymentType = requestions["payment_method"],
                    OrderIdMoMo="",
                    ReqrIdMoMo="",
                    FullName = requestions["fname"],
                    PaymentStatus = requestions["payment_method"]== "Tiền mặt" ? "3":"0",
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
            //thanh toán theo hình thức khác chọn
            if (requestions["payment_method"] == "Ví Momo")
            {

                var link = thanhToanMomo(newOrders.OrderId,(float)tongtien * 26000);
                return Redirect(link);
            }
            if (requestions["payment_method"] == "Thẻ ngân hàng")
            {

                var link= thanhToanATM(newOrders.OrderId,(float)tongtien * 26000);
                return Redirect(link);
            }
            if (requestions["payment_method"] == "Thẻ thanh toán quốc tế")
            {

                var link = thanhToanCC(newOrders.OrderId, (float)tongtien * 26000);
                return Redirect(link);
            }
            ViewBag.Message = "Đặt hàng thành công";
            return View();
            
        }
        [HttpPost]
        public ActionResult UpdatePaymentStatus(int itemId)
        {
            try
            {
                var order= _context.Orders.FirstOrDefault(o=> o.OrderId == itemId);
                // Tại đây, bạn có thể thực hiện các thao tác cập nhật trạng thái thanh toán
                // Ví dụ: cập nhật trong cơ sở dữ liệu, gọi API bên ngoài, vv.
                string partnerCode = "MOMO5RGX20191128";
                string serectkey = "nqQiVSgDMy809JoPF6OzP5OdBUB550Y4";
                string accessKey = "M8brj9K6E22vXoDB";
                string endpoint = "https://test-payment.momo.vn//v2/gateway/api/query";
                //Before sign HMAC SHA256 signature
                string rawHash = "accessKey=" + accessKey +
                    "&orderId=" + order.OrderIdMoMo +
                    "&partnerCode=" + partnerCode +
                    "&requestId=" + order.ReqrIdMoMo
                    ;

                MoMoSecurity crypto = new MoMoSecurity();
                //sign signature SHA256
                string signature = crypto.signSHA256(rawHash, serectkey);
                JObject message = new JObject
                {
                { "partnerCode", partnerCode },
                { "requestId", order.ReqrIdMoMo },
                { "orderId", order.OrderIdMoMo },
                { "signature", signature },
                { "lang", "en" }

            };
                string responseFromMomo = PaymentRequest.sendPaymentRequest(endpoint, message.ToString());
                JObject jmessage = JObject.Parse(responseFromMomo);
                string result = jmessage.GetValue("resultCode").ToString();
                // Sau khi cập nhật thành công, bạn có thể trả về kết quả bất kỳ nếu cần
                if (result == "0")
                {
                    order.PaymentStatus = "1";
                    _context.SaveChanges();
                    return Json(new { success = true, message = result });

                }
                if (result == "1000" || result=="7002" || result=="7000")
                {
                    return Json(new { success = true, message = result });
                }
                order.PaymentStatus = "2";
                if(order.Status == "6")
                {
                    return Json(new { success = true, message = result });
                }
                //cập nhật trạng thái đơn hàng hủy
                order.Status = "6";
                //nếu hủy cập nhật lại sản phẩm trong kho
                var ord = _context.OrderDetails.Where(o => o.OrderId == order.OrderId).ToList();
                 if (ord.Count() > 0)
                 {
                    foreach (var item in ord)
                    {
                        var sp = _context.ProductDetails.FirstOrDefault(p => p.ProductdetailId == item.ProductdetailId);
                        sp.Quantity += item.Quantity;
                        _context.Update(sp);
                    }
                }            
                _context.SaveChanges();
                return Json(new { success = true, message = result });

            }
            catch (Exception ex)
            {
                // Xử lý ngoại lệ nếu có lỗi xảy ra trong quá trình cập nhật
                return Json(new { success = false, message = "Đã xảy ra lỗi khi cập nhật trạng thái thanh toán." });
            }
        }
        public async Task<IActionResult> XacNhan( string orderId, string requestId)
        {
            string partnerCode = "MOMO5RGX20191128";
            string serectkey = "nqQiVSgDMy809JoPF6OzP5OdBUB550Y4";
            string accessKey = "M8brj9K6E22vXoDB";
            string endpoint = "https://test-payment.momo.vn//v2/gateway/api/query";
            //Before sign HMAC SHA256 signature
            string rawHash = "accessKey=" + accessKey +
                "&orderId=" + orderId +
                "&partnerCode=" + partnerCode +
                "&requestId=" + requestId 
                ;

            MoMoSecurity crypto = new MoMoSecurity();
            //sign signature SHA256
            string signature = crypto.signSHA256(rawHash, serectkey);
            JObject message = new JObject
            {
                { "partnerCode", partnerCode },
                { "requestId", requestId },
                { "orderId", orderId },
                { "signature", signature },
                { "lang", "en" }

            };
            string responseFromMomo = PaymentRequest.sendPaymentRequest(endpoint, message.ToString());
            JObject jmessage = JObject.Parse(responseFromMomo);
            string result = jmessage.GetValue("resultCode").ToString();
            if (result == "0")
            {
                //xử lý thanh toán thành công
                ViewBag.Message = "Thanh toán đơn hàng thành công ";
                ViewBag.Madonhang = orderId;
                var od =_context.Orders.Where(o=>o.OrderIdMoMo == orderId).FirstOrDefault();
                if(od != null)
                {
                    od.PaymentStatus = "1";
                    _context.SaveChanges();
                }
            }
            else
            {
                var od = _context.Orders.Where(o => o.OrderIdMoMo == orderId).FirstOrDefault();
                od.PaymentStatus = "2";
                if (od.Status == "6")
                {
                    _context.SaveChanges();
                    ViewBag.Message = "Thanh toán đơn hàng thất bại ";
                    ViewBag.Madonhang = orderId;
                    return View("XuLyThanhToan");
                }
                od.Status = "6";
                //nếu hủy cập nhật lại sản phẩm trong kho
                var ord = _context.OrderDetails.Where(o => o.OrderId == od.OrderId).ToList();
                if (ord.Count() > 0)
                {
                    foreach (var item in ord)
                    {
                        var sp = _context.ProductDetails.FirstOrDefault(p => p.ProductdetailId == item.ProductdetailId);
                        sp.Quantity += item.Quantity;
                        _context.Update(sp);
                    }
                }
                _context.SaveChanges();
                ViewBag.Message ="Thanh toán đơn hàng thất bại ";
                ViewBag.Madonhang = orderId;
            }

            return View("XuLyThanhToan");
        }
        private string thanhToanMomo(int OrderId,float sotien)
        {
            string endpoint = "https://test-payment.momo.vn/v2/gateway/api/create";
            string partnerCode = "MOMO5RGX20191128";
            string accessKey = "M8brj9K6E22vXoDB";
            string serectkey = "nqQiVSgDMy809JoPF6OzP5OdBUB550Y4";
            string orderInfo = "Store HD";
            string redirectUrl = "https://localhost:7166/ThanhToan/XacNhan";
            string ipnUrl = "https://localhost:7166/Huy";
            string requestType = "captureWallet";

            string amount = sotien.ToString();
            string orderId = Guid.NewGuid().ToString();
            string requestId = Guid.NewGuid().ToString();
            string extraData = "";
            //lưu mã đơn hàng và mã yêu cầu vào đây
            var od = _context.Orders.Where(o => o.OrderId == OrderId).FirstOrDefault();
            if (od != null)
            {
                od.OrderIdMoMo = orderId;
                od.ReqrIdMoMo = requestId;
                _context.SaveChanges();
            }
            //Before sign HMAC SHA256 signature
            string rawHash = "accessKey=" + accessKey +
                "&amount=" + amount +
                "&extraData=" + extraData +
                "&ipnUrl=" + ipnUrl +
                "&orderId=" + orderId +
                "&orderInfo=" + orderInfo +
                "&partnerCode=" + partnerCode +
                "&redirectUrl=" + redirectUrl +
                "&requestId=" + requestId +
                "&requestType=" + requestType
                ;

            MoMoSecurity crypto = new MoMoSecurity();
            //sign signature SHA256
            string signature = crypto.signSHA256(rawHash, serectkey);

            //build body json request
            JObject message = new JObject
            {
                { "partnerCode", partnerCode },
                { "partnerName", "Test" },
                { "storeId", "MomoTestStore" },
                { "requestId", requestId },
                { "amount", amount },
                { "orderId", orderId },
                { "orderInfo", orderInfo },
                { "redirectUrl", redirectUrl },
                { "ipnUrl", ipnUrl },
                { "lang", "en" },
                { "extraData", extraData },
                { "requestType", requestType },
                { "signature", signature }

            };
            string responseFromMomo = PaymentRequest.sendPaymentRequest(endpoint, message.ToString());
            Console.WriteLine(responseFromMomo);
            JObject jmessage = JObject.Parse(responseFromMomo);
            return jmessage.GetValue("payUrl").ToString();
        }
        private string thanhToanATM(int OrderId,float sotien)
        {
            string endpoint = "https://test-payment.momo.vn//v2/gateway/api/create";
            string partnerCode = "MOMO5RGX20191128";
            string accessKey = "M8brj9K6E22vXoDB";
            string serectkey = "nqQiVSgDMy809JoPF6OzP5OdBUB550Y4";
            string requestId = Guid.NewGuid().ToString();
            string amount = sotien.ToString();
            string orderId = Guid.NewGuid().ToString();
            string orderInfo = "Store HD";
            string redirectUrl = "https://localhost:7166/ThanhToan/XacNhan";
            string ipnUrl = "https://facebook.com";
            string requestType = "payWithATM";
            string extraData = "";
            string lang = "vi";
            string signature = "";
            //lưu mã đơn hàng và mã yêu cầu vào đây
            var od = _context.Orders.Where(o => o.OrderId == OrderId).FirstOrDefault();
            if (od != null)
            {
                od.OrderIdMoMo = orderId;
                od.ReqrIdMoMo = requestId;
                _context.SaveChanges();
            }
            string rawhash = "accessKey=" + accessKey +
                                "&amount=" + amount +
                                "&extraData=" + extraData +
                                "&ipnUrl=" + ipnUrl +
                                "&orderId=" + orderId +
                                "&orderInfo=" + orderInfo +
                                "&partnerCode=" + partnerCode +
                                "&redirectUrl=" + redirectUrl +
                                "&requestId=" + requestId +
                                "&requestType=" + requestType;
            MoMoSecurity moMoSecurity = new MoMoSecurity();
            signature = moMoSecurity.signSHA256(rawhash, serectkey);

            JObject message = new JObject
            {
                { "partnerCode", partnerCode },
                { "requestId", requestId },
                { "amount", amount },
                { "orderId", orderId },
                { "orderInfo", orderInfo },
                { "redirectUrl", redirectUrl },
                { "ipnUrl", ipnUrl },
                { "requestType", requestType },
                { "extraData", extraData },
                {"lang", lang },
                { "signature", signature}
            };
            string responseMomo = PaymentRequest.sendPaymentRequest(endpoint, message.ToString());
            try
            {
                JObject jmessage = JObject.Parse(responseMomo);
                return jmessage.GetValue("payUrl").ToString();
            }
            catch (Exception ex)
            {
                return "";
            }
        }
        private string thanhToanCC(int OrderId, float sotien)
        {
            string endpoint = "https://test-payment.momo.vn//v2/gateway/api/create";
            string partnerCode = "MOMO5RGX20191128";
            string accessKey = "M8brj9K6E22vXoDB";
            string serectkey = "nqQiVSgDMy809JoPF6OzP5OdBUB550Y4";
            string requestId = Guid.NewGuid().ToString();
            string amount = sotien.ToString();
            string orderId = Guid.NewGuid().ToString();
            string orderInfo = "Store HD";
            string redirectUrl = "https://localhost:7166/ThanhToan/XacNhan";
            string ipnUrl = "https://facebook.com";
            string requestType = "payWithCC";
            string extraData = "";
            string lang = "vi";
            string signature = "";
            //lưu mã đơn hàng và mã yêu cầu vào đây
            var od = _context.Orders.Where(o => o.OrderId == OrderId).FirstOrDefault();
            if (od != null)
            {
                od.OrderIdMoMo = orderId;
                od.ReqrIdMoMo = requestId;
                _context.SaveChanges();
            }
            string rawhash = "accessKey=" + accessKey +
                                "&amount=" + amount +
                                "&extraData=" + extraData +
                                "&ipnUrl=" + ipnUrl +
                                "&orderId=" + orderId +
                                "&orderInfo=" + orderInfo +
                                "&partnerCode=" + partnerCode +
                                "&redirectUrl=" + redirectUrl +
                                "&requestId=" + requestId +
                                "&requestType=" + requestType;
            MoMoSecurity moMoSecurity = new MoMoSecurity();
            signature = moMoSecurity.signSHA256(rawhash, serectkey);

            JObject message = new JObject
            {
                { "partnerCode", partnerCode },
                { "requestId", requestId },
                { "amount", amount },
                { "orderId", orderId },
                { "orderInfo", orderInfo },
                { "redirectUrl", redirectUrl },
                { "ipnUrl", ipnUrl },
                { "requestType", requestType },
                { "extraData", extraData },
                {"lang", lang },
                { "signature", signature}
            };
            string responseMomo = PaymentRequest.sendPaymentRequest(endpoint, message.ToString());
            try
            {
                JObject jmessage = JObject.Parse(responseMomo);
                return jmessage.GetValue("payUrl").ToString();
            }
            catch (Exception ex)
            {
                return "";
            }
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
