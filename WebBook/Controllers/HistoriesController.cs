using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebBook.Models;

namespace WebBook.Controllers
{
    public class HistoriesController : Controller
    {
        private readonly WebBookContext _context;

        public HistoriesController(WebBookContext context)
        {
            _context = context;
        }

        // Вывод истории взятых книг текущего пользователя
        // GET: Histories
        [Authorize(Roles = "admin, user")]
        public async Task<IActionResult> Index(int? id)
        {
            var query = _context.History.Include(h => h.Book).Include(h => h.User);
            if (id.HasValue)
            {
                return View(await query.Where(b => b.Book.Id.Equals(id)).ToListAsync());
            }
            return View(await query.ToListAsync());
        }

        // Вывод всей истории
        [Authorize(Roles = "admin, user")]
        public async Task<IActionResult> TakenBooks()
        {
            var webBookContext = _context.History.Include(h => h.Book).Include(h => h.User);
            return View(await webBookContext.ToListAsync());
        }

        // Возвращение книги читателем
        [HttpPost]
        [Authorize(Roles = "admin, user")]
        public async Task<IActionResult> Return(int id)
        {
            var history = await _context.History
                .Include(h => h.Book)
                .Include(h => h.User)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (history.Book.Issued == true)
            {
                history.ReturnDate = DateTime.Now;
                history.Book.Issued = false;
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("TakenBooks", "Histories");
        }

        private bool HistoryExists(int id)
        {
            return _context.History.Any(e => e.Id == id);
        }
    }
}
