using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Notyf;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication7.Areas.Admin.Helpper;
using WebApplication7.Models;

namespace WebApplication7.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ColorsController : Controller
    {
        private readonly QLDBcontext _context;
        private readonly NotyfService _notyfService;
        public ColorsController(QLDBcontext context, NotyfService notyfService)
        {
            _context = context;
            _notyfService= notyfService;
        }

        // GET: Admin/Colors
        public async Task<IActionResult> Index()
        {
            return View(await _context.Colors.ToListAsync());
        }

        // GET: Admin/Colors/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Colors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ColorId,ColorName")] Color color)
        {
            if (ModelState.IsValid)
            {
                color.ColorSlug = Slug.GenerateSlug(color.ColorName);
                var checkcolor = _context.Colors.FirstOrDefault(m => m.ColorSlug == color.ColorSlug);
                if (checkcolor == null)
                {
                    _notyfService.Success("Thêm thành công", 3);
                    _context.Add(color);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                _notyfService.Error("Màu sắc trùng", 3);
                return View(color);
            }
            _notyfService.Error("Nhập dữ liệu hợp lệ", 3);
            return View(color);
        }

        // GET: Admin/Colors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var color = await _context.Colors.FindAsync(id);
            if (color == null)
            {
                return NotFound();
            }
            return View(color);
        }

        // POST: Admin/Colors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ColorId,ColorName")] Color color)
        {
            if (id != color.ColorId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    color.ColorSlug = Slug.GenerateSlug(color.ColorName);
                    var checkSize = _context.Colors.FirstOrDefault(m => m.ColorSlug == color.ColorSlug && m.ColorId != id);
                    if (checkSize != null)
                    {
                        _notyfService.Error("Trùng màu sắc", 3);
                        return View(color);
                    }
                    _context.Update(color);
                    await _context.SaveChangesAsync();
                    _notyfService.Success("Cập nhật thành công", 3);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ColorExists(color.ColorId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(color);
        }

        // GET: Admin/Colors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var color = await _context.Colors
                .FirstOrDefaultAsync(m => m.ColorId == id);
            if (color == null)
            {
                return NotFound();
            }

            return View(color);
        }

        // POST: Admin/Colors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var color = await _context.Colors.FindAsync(id);
            if (color != null)
            {
                _context.Colors.Remove(color);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ColorExists(int id)
        {
            return _context.Colors.Any(e => e.ColorId == id);
        }
    }
}
