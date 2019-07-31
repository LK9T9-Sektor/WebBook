using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebBook.Models;
using WebBook.ViewModels;

namespace WebBook.Controllers
{
    [Authorize(Roles = "admin")]
    public class ReportController : Controller
    {
        private readonly WebBookContext _context;

        public ReportController(WebBookContext context)
        {
            _context = context;
        }

        // GET: Report        
        public async Task<IActionResult> Index()
        {
            var webBookContext = _context.History
                .Include(h => h.Book)
                .Include(h => h.User)
                .Where(h => h.Book.Issued == true && h.ReturnDate == null);
            ViewData["Message"] = webBookContext.Count();
            return View(await webBookContext.ToListAsync());
        }

        #region Формирование страниц отчетов

        // Кол-во и список книг на руках
        public async Task<IActionResult> CurrentlyTaken()
        {
            var webBookContext = _context.History
                .Include(h => h.Book)
                .Include(h => h.User)
                .Where(h => h.Book.Issued == true && h.ReturnDate == null);

            ViewData["Message"] = webBookContext.Count();
            return View(await webBookContext.ToListAsync());
        }

        // Оборот выбранной книги по дате
        public async Task<IActionResult> Turnover()
        {
            var webBookContext = _context.History
                .Include(h => h.Book)
                .Include(h => h.User)
                .GroupBy(h => h.Book.Name).Select(g => g.First());

            return View(await webBookContext.ToListAsync());
        }

        [HttpPost]
        public async Task<IActionResult> Turnover(DateTime FirstDate, DateTime SecondDate, int id)
        {
            var history = _context.History
                .Include(h => h.Book)
                .Include(h => h.User)
                .Where(m => m.Issue >= FirstDate && m.ReturnDate <= SecondDate && m.ReturnDate != null &&
                m.Book.Id == id);
            return View(await history.ToListAsync());
        }

        // Оборот всех книг за период
        public async Task<IActionResult> PeriodHistory()
        {
            var bookLibraryContext = _context.History.Include(h => h.Book).Include(h => h.User);
            return View(await bookLibraryContext.ToListAsync());
        }

        [HttpPost]
        public async Task<IActionResult> PeriodHistory(DateTime FirstDate, DateTime SecondDate)
        {
            var bookLibraryContext = _context.History.Include(h => h.Book).Include(h => h.User)
                .Where(d => d.Issue >= FirstDate && d.ReturnDate <= SecondDate);
            return View(await bookLibraryContext.ToListAsync());
        }

        #endregion

        #region Вывод страниц отчетов для печати

        [HttpPost]
        public async Task<IActionResult> PrintTaken()
        {
            var webBookContext = _context.History
                .Include(h => h.Book)
                .Include(h => h.User)
                .Where(h => h.Book.Issued == true && h.ReturnDate == null);

            ViewData["Message"] = webBookContext.Count();
            return View(await webBookContext.ToListAsync());
        }

        [HttpPost]
        public async Task<IActionResult> PrintTurnover(DateTime FirstDate, DateTime SecondDate, int id)
        {
            ViewData["DateStart"] = FirstDate.Date.ToShortDateString();
            ViewData["DateEnd"] = SecondDate.Date.ToShortDateString();

            var webBookContext = _context.History
                .Include(h => h.Book)
                .Include(h => h.User)
                .Where(m => m.Issue >= FirstDate && m.ReturnDate <= SecondDate && m.ReturnDate != null &&
                m.Book.Id == id);
            return View(await webBookContext.ToListAsync());
        }

        [HttpPost]
        public async Task<IActionResult> PrintPeriod(DateTime FirstDate, DateTime SecondDate)
        {
            ViewData["DateStart"] = FirstDate.Date.ToShortDateString();
            ViewData["DateEnd"] = SecondDate.Date.ToShortDateString();

            var webBookContext = _context.History
                .Include(h => h.Book)
                .Include(h => h.User)
                .Where(d => d.Issue >= FirstDate && d.ReturnDate <= SecondDate);
            return View(await webBookContext.ToListAsync());
        }

        #endregion

        private bool HistoryExists(int id)
        {
            return _context.History.Any(e => e.Id == id);
        }
    }
}
