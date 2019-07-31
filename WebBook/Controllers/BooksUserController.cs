using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebBook.Models;

namespace WebBook.Controllers
{
    public class BooksUserController : Controller
    {
        private readonly WebBookContext _context;

        public BooksUserController(WebBookContext context)
        {
            _context = context;
        }

        // GET: BooksUser
        [Authorize(Roles = "admin, user")]
        public async Task<IActionResult> Index()
        {
            // Проверка даты списания книги, вывод не списанных
            var query = _context.Books.Where(b => b.OutDate == null);
            return View(await query.ToListAsync());
        }

        [HttpPost]
        [Authorize(Roles = "admin, user")]
        public async Task<IActionResult> Take(int id)
        {
            var book = await _context.Books.FirstOrDefaultAsync(m => m.Id == id);
            if (book.Issued == false)
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email.Equals(User.Identity.Name));
                _context.History.Add(new History(id, user.Id));
                book.Issued = true;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("TakenBooks", "Histories");
        }

        // GET: BooksUser/Details/5
        [Authorize(Roles = "admin, user")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var books = await _context.Books
                .FirstOrDefaultAsync(m => m.Id == id);
            if (books == null)
            {
                return NotFound();
            }

            return View(books);
        }
    }
}
