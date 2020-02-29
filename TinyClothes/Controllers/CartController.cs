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
    public class CartController : Controller
    {

        private readonly StoreContext _context; //c# convention to use underscore to set field instead of using this keyword down below, "readonly" means only the constructor can modify this variable
        private readonly IHttpContextAccessor _http; //to read cookie data

        public CartController(StoreContext context, IHttpContextAccessor http) //constructor gives controller access to database
        {
            _context = context;
            _http = http;
        }

        //Display all products in cart
        public IActionResult Index()
        {
            return View();
        }


        //Add a single product to the shopping cart
        public async Task<IActionResult> Add(int id, string prevUrl)
        {
            Clothing c = await ClothingDb.GetClothingById(id, _context);

            if(c!= null) 
            {
                CartHelper.Add(c, _http);
            }

            return Redirect(prevUrl);
        }

        //Summary/checkout page
        public IActionResult CheckOut()
        {
            return View();
        }

    }
}