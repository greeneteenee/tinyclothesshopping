using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TinyClothes.Models;

namespace TinyClothes.Data
{
    public class StoreContext : DbContext //Our StoreContext (which talks to database) inherits from Microsoft.EntityFrameworkCore.DbContext
    {

        public StoreContext(DbContextOptions<StoreContext> options) : base(options) //A way to configure database
        {

        }

        /// <summary>
        /// Add a DbSet for each Entity that needs to be tracked by the DB
        /// https://docs.microsoft.com/en-us/aspnet/core/data/ef-mvc/intro?view=aspnetcore-3.1#create-the-database-context
        /// </summary>
        public DbSet<Clothing> Clothing { get; set; } //Property names can be the same as class names, however, developers usually make the property name plural

        public DbSet<Account> Accounts { get; set; }

    }
}
