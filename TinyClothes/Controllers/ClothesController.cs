using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TinyClothes.Data;
using TinyClothes.Models;

namespace TinyClothes.Controllers
{
    public class ClothesController : Controller
    {

        private readonly StoreContext _context; //c# convention to use underscore to set field instead of using this keyword down below, "readonly" means only the constructor can modify this variable

        public ClothesController(StoreContext context) //constructor gives controller access to database
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult ShowAll()
        {
            List<Clothing> clothes = new List<Clothing>(); //just a placeholder

            return View(clothes);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(Clothing c) 
        { 
            if (ModelState.IsValid)//if data is valid
            {
                await ClothingDb.Add(c, _context); //add to database

                //TempData lasts for one redirect
                TempData["Message"] = $"{c.Title} added successfully";

                return RedirectToAction("ShowAll");
            }

            return View(c); //else return the same view with validation error messages

        }

    }
}