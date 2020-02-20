using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TinyClothes.Data;
using TinyClothes.Models;

namespace TinyClothes.Controllers
{
    public class AccountController : Controller
    {
        private readonly StoreContext _context; //c# convention to use underscore to set field instead of using this keyword down below, "readonly" means only the constructor can modify this variable
        private readonly IHttpContextAccessor _http;

        public AccountController(StoreContext context, IHttpContextAccessor http) //constructor gives controller access to database
        {
            _context = context;
            _http = http;
        }


        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel reg)
        {
            if (ModelState.IsValid)
            {
                // Check username is not taken
                if (!await AccountDb.IsUsernameTaken(reg.Username, _context))
                {
                    // Maps register object to account object
                    Account acc = new Account()
                    {
                        Email = reg.Email,
                        FullName = reg.FullName,
                        Password = reg.Password,
                        Username = reg.Username
                    };

                    // Add account to database
                    await AccountDb.Register(_context, acc);

                    // Create user session
                    SessionHelper.CreateUserSession(acc.AccountId, acc.Username, _http);
                 

                    return RedirectToAction("Index", "Home");
                }
                else //display error
                {
                    ModelState.AddModelError(nameof(Account.Username), "Username is already taken");
                }


            }

            return View(reg);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel login)
        {
            if (ModelState.IsValid)
            {
                Account acc = await AccountDb.DoesUserAMatch(login, _context);

                // Create user session
                SessionHelper.CreateUserSession(acc.AccountId, acc.Username, _http);

                return RedirectToAction("Index", "Home");
            }
            return View(login);
        }
        
        public IActionResult Logout()
        {
            SessionHelper.DestroyUserSession(_http);
            return RedirectToAction("Index", "Home");
        }

    }
}