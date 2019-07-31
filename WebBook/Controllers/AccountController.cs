using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebBook.ViewModels;
using WebBook.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace AuthApp.Controllers
{
    public class AccountController : Controller
    {
        private WebBookContext db;

        public AccountController(WebBookContext context)
        {
            db = context;
        }

        #region Логин
        [HttpGet]
        public IActionResult Login()
        {
            if (User.IsInRole("admin"))
            {
                return RedirectToAction("Index", "Books");
            }
            if (User.IsInRole("user"))
            {
                return RedirectToAction("Index", "BooksUser");

            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                Users user = await db.Users.FirstOrDefaultAsync(u => u.Email == model.Email && u.Password == model.Password);
                if (user != null)
                {
                    // Аутентификация
                    await Authenticate(user);

                    return RedirectToAction("Index", "Books");
                }
                ModelState.AddModelError("", "Некорректные логин и(или) пароль");
            }
            return View(model);
        }
        #endregion

        #region Регистрация
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                Users user = await db.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
                if (user == null)
                {
                    // добавляем пользователя в бд
                    user = new Users { Email = model.Email, Password = model.Password, Role = "user" };

                    if (user.Role != null)
                    {
                        db.Users.Add(user);
                        await db.SaveChangesAsync();
                        await Authenticate(user); // аутентификация
                    }
                    return RedirectToAction("Index", "BooksUser");
                }
                else
                    ModelState.AddModelError("", "Некорректные логин и(или) пароль");
            }
            return View(model);
        }
        #endregion

        #region Аутентификация (Claims)
        private async Task Authenticate(Users user)
        {
            // Создание Claim
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Email),
                // Проверка роли
                new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role)
            };
            // Создание объекта ClaimsIdentity
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            // Установка аутентификационных куки
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }
        #endregion

        #region Логаут
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }
        #endregion
    }
}