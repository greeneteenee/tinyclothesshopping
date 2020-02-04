﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TinyClothes.Models;

namespace TinyClothes.Data
{
    /// <summary>
    /// Contains DB helper methods for <see cref="Models.Clothing"/>
    /// </summary>
    public static class ClothingDb
    {
       

        /// <summary>
        /// Returns a specific page of clothing items with specific number of items sorted by ItemId in ascending order
        /// </summary>        /// 
        const int PageOffset = 1; //must offset by one to get page 1
        public async static Task<List<Clothing>> GetClothingByPage(StoreContext context, int pageNum, int pageSize)
        {
                       

            List<Clothing> clothes = await context.Clothing
                .OrderBy(c => c.ItemId)
                .Skip((pageNum - PageOffset)*pageSize)
                .Take(pageSize)                
                .ToListAsync(); //LINQ method syntax



            //List<Clothing> clothes2 = await (from c in context.Clothing orderby c.ItemId ascending select c) //LINQ method syntax (keeping for notes)
            //.Skip((pageNum - PageOffset) * pageSize)
            //.Take(pageSize)
            //.ToListAsync(); 

            return clothes;

        }

        /// <summary>
        /// Returns the total number of clothing items
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async static Task<int> GetNumClothing(StoreContext context)
        {
            return await context.Clothing.CountAsync();
        }

        /// <summary>
        /// Adds a clothing object to the database
        /// </summary>
        /// <param name="c"></param>
        /// <returns>Returns the object with the Id populated</returns>
        public static async Task<Clothing> Add(Clothing c, StoreContext context) //best to pass context as a parameter instead of creating a new context inside method
        {
            await context.AddAsync(c); //prepares INSERT query
            await context.SaveChangesAsync(); //execute INSERT query,must call save changes so changes goes into database

            return c;
        }

    }
}
