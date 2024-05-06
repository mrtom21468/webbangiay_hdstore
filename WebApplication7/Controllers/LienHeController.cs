using Microsoft.AspNetCore.Mvc;
using WebApplication7.Areas.Admin.Helpper;
using WebApplication7.ViewModel;
using AspNetCoreHero.ToastNotification.Notyf;

namespace WebApplication7.Controllers
{
    public class LienHeController : Controller
    {
        private readonly NotyfService _notyfService;
        public LienHeController(NotyfService notyfService)
        {
            _notyfService=notyfService; 
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult SubmitSupport(HoTro model)
        {
            if (ModelState.IsValid)
            {
                // Xử lý dữ liệu ở đây
                // Sau đó chuyển hướng hoặc hiển thị thông báo thành công
                bool resul=NotyfGmail.SenGmailHoTro(model.Email, model.HoTen, model.NoiDung);
                if (resul)
                {
                    _notyfService.Success("Gửi thông tin hỗ trợ thành công", 3);
                    return RedirectToAction("Index");
                }
                else
                {
                    _notyfService.Warning("Gửi thông tin hỗ trợ thất bại", 3);
                    return View("Index", model);
                }
            }
            // Nếu dữ liệu không hợp lệ, hiển thị lại form với thông báo lỗi
            return View("Index", model);
        }
    }
}
