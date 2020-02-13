using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TinyClothes.Data;
using TinyClothes.Models;

namespace TinyClothes.Controllers
{
    public class AccountController : Controller
    {
        private readonly StoreContext _context; //c# convention to use underscore to set field instead of using this keyword down below, "readonly" means only the constructor can modify this variable

        public AccountController(StoreContext context) //constructor gives controller access to database
        {
            _context = context;
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
                //check username is not taken
                if (!await AccountDb.IsUsernameTaken(reg.Username, _context))
                {
                    //maps register object to account object
                    Account acc = new Account()
                    {
                        Email = reg.Email,
                        FullName = reg.FullName,
                        Password = reg.Password,
                        Username = reg.Username
                    };

                    //add account to database
                    await AccountDb.Register(_context, acc);
                    return RedirectToAction("Index", "Home");
                }
                else //display error
                {
                    ModelState.AddModelError(nameof(Account.Username), "Username is already taken");
                }


            }

            return View(reg);
        }
    }
}