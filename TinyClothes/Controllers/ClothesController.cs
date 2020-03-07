﻿using System;
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
        public async Task<IActionResult> ShowAll(int? page) //variable name need to match asp-route name, use a nullable int because don't have a page to begin with
        {
            const int PageSize = 2;
            int pageNumber = page.HasValue ? page.Value : 1;
            ViewData["CurrentPage"] = pageNumber;

            int maxPage = await GetMaxPage(PageSize);
            ViewData["MaxPage"] = maxPage;

            List<Clothing> clothes = await ClothingDb.GetClothingByPage(_context, pageNumber, PageSize); //database context is injected by the framework using the constructor makes it so you do have to create new database objects everyone you need it

            return View(clothes);
        }

        private async Task<int> GetMaxPage(int PageSize)
        {
            int numProducts = await ClothingDb.GetNumClothing(_context);
            int maxPage = Convert.ToInt32(Math.Ceiling((double)numProducts / PageSize));
            return maxPage;
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
        //when user first goes to webpage
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            Clothing c = await ClothingDb.GetClothingById(id, _context);

            if(c == null) //if clothing is not in the database
            {
                //returns a HTTP 404 - Not found error
                return NotFound();
            }

            return View(c);
        }

        //when user is interacting with database
        [HttpPost]
        public async Task<IActionResult> Edit(Clothing c)
        {
            if (ModelState.IsValid)
            {
                await ClothingDb.Edit(c, _context);
                ViewData["Message"] = c.Title + " updated successfully";
                return View(c);
            }
            return View(c);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            Clothing c = await ClothingDb.GetClothingById(id, _context);
            if (c == null) //if clothing is not in the database
            {
                //returns a HTTP 404 - Not found error
                return NotFound();
            }
            return View(c);
        }

        [HttpPost]
        [ActionName("Delete")] //workaround because we can't have two methods with the same parameters with the same name
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Clothing c = await ClothingDb.GetClothingById(id, _context);
            await ClothingDb.Delete(c, _context);
            TempData["Message"] = $"{c.Title} deleted successfully";
            return RedirectToAction(nameof(ShowAll)); //turns "ShowAll" into a string
        }

        [HttpGet]
        public async Task<IActionResult> Search(SearchCriteria search)
        {
            //IQueryable prepares the query(SELECT * FROM Clothes), but does not send to database
            IQueryable<Clothing> allClothes = from c in _context.Clothing select c;

            //WHERE Price > MinPrice
            if (search.MinPrice.HasValue)
            {
                allClothes = from c in allClothes
                                where c.Price >= search.MinPrice
                                select c;
            }


            //WHERE Price < MaxPrice
            if (search.MinPrice.HasValue)
            {
                allClothes = from c in allClothes
                             where c.Price <= search.MaxPrice
                             select c;
            }

            if(!string.IsNullOrWhiteSpace(search.Size))
            {
                allClothes = from c in allClothes
                             where c.Size == search.Size
                             select c;
            }

            if (!string.IsNullOrWhiteSpace(search.Type))
            {
                allClothes = from c in allClothes
                             where c.Type == search.Type
                             select c;
            }

            if (!string.IsNullOrWhiteSpace(search.Title))
            {
                allClothes = from c in allClothes
                             where c.Title.Contains(search.Title)
                             select c;
            }
            

            search.Results = allClothes.ToList();
            return View(search);
        }

    }
}