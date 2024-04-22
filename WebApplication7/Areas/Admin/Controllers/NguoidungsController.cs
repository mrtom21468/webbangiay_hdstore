using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using WebApplication7.Models;

namespace WebApplication7.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles ="Admin")]
    public class NguoidungsController : Controller
    {
        private readonly UserManager<UserIdentitycs> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly QLDBcontext _qLDBcontext ;

        public NguoidungsController(UserManager<UserIdentitycs> userManager, RoleManager<IdentityRole> roleManager, QLDBcontext qLDBcontext)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _qLDBcontext = qLDBcontext;
        }

        // Action để hiển thị danh sách người dùng
        public IActionResult Index()
        {
            var users = _userManager.Users.ToList();
            if (users != null)
            {
                ViewBag.UserManager = _userManager;
                return View(users);
            }
            else
            {
                return View(users);
            }
        }

        // Action để hiển thị form chỉnh sửa người dùng
        public async Task<IActionResult> Edit(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // Action để xử lý yêu cầu chỉnh sửa người dùng
        [HttpPost]
        public async Task<IActionResult> Edit(string id, string email, string name)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            user.Email = email;
            user.Name = name;
            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }
            return View(user);
        }

        // Action để hiển thị form xác nhận xóa người dùng
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // Action để xử lý yêu cầu xóa người dùng
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                Models.Account acc= _qLDBcontext.Accounts.Where(u=> u.UserId== id).First();
                if (acc != null)
                {
                    _qLDBcontext.Remove(acc);
                }
                return RedirectToAction("Index");
            }
            return View(user);
        }
    }
}
