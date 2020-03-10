using Microsoft.EntityFrameworkCore;
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
        public static async Task<List<Clothing>> GetClothingByPage(StoreContext context, int pageNum, int pageSize)
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
        public static async Task<int> GetNumClothing(StoreContext context)
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
        
        /// <summary>
        /// Returns a single clothing item or null if there is no match
        /// </summary>
        /// <param name="id">Id of the clothing item</param>
        /// <param name="context">Database context</param>  
        public static async Task<Clothing> GetClothingById(int id, StoreContext context)
        {
            Clothing c = await (from clothing in context.Clothing
                            where clothing.ItemId == id
                            select clothing).SingleOrDefaultAsync();
            return c;
        }

        public static async Task<Clothing> Edit(Clothing c, StoreContext context)
        {
            await context.AddAsync(c);
            context.Entry(c).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return c;
        }
                
        public static async Task Delete(Clothing c, StoreContext context)        
        {                    
                await context.AddAsync(c);
                context.Entry(c).State = EntityState.Deleted;
                await context.SaveChangesAsync();
            
        }

        public static async Task<SearchCriteria> BuildSearchQuery(SearchCriteria search, StoreContext context)
        {
            //IQueryable prepares the query(SELECT * FROM Clothes), but does not send to database
            IQueryable<Clothing> allClothes = from c in context.Clothing select c;

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

            if (!string.IsNullOrWhiteSpace(search.Size))
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


            search.Results = await allClothes.ToListAsync();
            return search;
        }

    }

}

