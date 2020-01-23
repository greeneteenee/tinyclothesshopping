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
        public static List<Clothing> GetAllClothing()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Adds a clothing object to the database
        /// </summary>
        /// <param name="c"></param>
        /// <returns>Returns the object with the Id populated</returns>
        public static Clothing Add(Clothing c, StoreContext context) //best to pass context as a parameter instead of creating a new context inside method
        {
            context.Add(c); //prepares INSERT query
            context.SaveChanges(); //execute INSERT query,must call save changes so changes goes into database

            return c;
        }

    }
}
