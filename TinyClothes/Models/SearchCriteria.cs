using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TinyClothes.Models
{
    public class SearchCriteria
    {
        public SearchCriteria()
        {
            Results = new List<Clothing>();
        }

        /// <summary>
        /// The type of clothing (i.e. shirt, pants, etc.)
        /// </summary>
        public string Type { get; set; }

        [StringLength(150)]
        public string Title { get; set; }

        public string Size { get; set; }

        [Display(Name = "Min Price")]
        [Range(0.0, double.MaxValue, ErrorMessage ="Min Price can't be negative")]
        public double? MinPrice { get; set; }

        [Display(Name = "Max Price")]
        [Range(0, double.MaxValue, ErrorMessage ="Max Price can't be negative")]
        public double? MaxPrice { get; set; }

        public List<Clothing> Results { get; set; }

    }
}
