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
        [Required]
        [StringLength(35)]        
        public string Title { get; set; }

        /// <summary>
        /// Description of the clothing
        /// </summary>
        [StringLength(800)]
        public string Description { get; set; }

        /// <summary>
        /// The size of clothing
        /// </summary>
        [Required(ErrorMessage ="Size is required")]       
        public string Size { get; set; }

        /// <summary>
        /// The type of clothing (i.e. shirt, pants, etc.)
        /// </summary>
        public string Type { get; set; }

        [Required]
        public string Color { get; set; }

        /// <summary>
        /// Retail price of item
        /// </summary>
        [Range(0.0, 9999.99)]
        [DataType(DataType.Currency)]
        public double Price { get; set; }

    }
}
