using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication7.Areas.Admin.Controllers
{
    [Area ("Admin")]
    [Authorize(Roles ="Admin")]
    public class RolesController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        public RolesController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }
        public IActionResult Index()
        {
            var roles= _roleManager.Roles.ToList();
            return View(roles);
        }
        // Action để tạo mới vai trò
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(string roleName)
        {
            if (!string.IsNullOrEmpty(roleName))
            {
                var role = new IdentityRole(roleName);
                await _roleManager.CreateAsync(role);
                ViewBag.Mess = "Tạo thành công";
            }
            else
            {
                ViewBag.Mess = "Tạo thất bại";

            }
            return View();
        }
        public async Task<IActionResult> Edit(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound();
            }
            return View(role);
        }

        // Action để xử lý yêu cầu chỉnh sửa vai trò
        [HttpPost]
        public async Task<IActionResult> Edit(string id, string roleName)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound();
            }
            role.Name = roleName;
            var result = await _roleManager.UpdateAsync(role);
            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }
            return View(role);
        }

        // Action để hiển thị form xác nhận xóa vai trò
        public async Task<IActionResult> Delete(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound();
            }
            return View(role);
        }

        // Action để xử lý yêu cầu xóa vai trò
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound();
            }
            var result = await _roleManager.DeleteAsync(role);
            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }
            return View(role);
        }
    }
}

