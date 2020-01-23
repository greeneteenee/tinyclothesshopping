using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TinyClothes.Models
{
    //Represents a single clothing item
    public class Clothing
    {
        /// <summary>
        /// The unique identifier for the clothing item
        /// </summary>
        [Key] //Attribute that sets property primary key
        public int ItemId { get; set; }


        /// <summary>
        /// Title of clothing item
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Description of the clothing
        /// </summary>
        public string Description { get; set; }

        public string Size { get; set; }

        /// <summary>
        /// The type of clothing (i.e. shirt, pants, etc.)
        /// </summary>
        public string Type { get; set; }

        public string Color { get; set; }

        /// <summary>
        /// Retail price of item
        /// </summary>
        public double Price { get; set; }

    }
}
