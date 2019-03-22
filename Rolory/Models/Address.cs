using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Rolory.Models
{
    public class Address
    {
        [Key]
        public int id { get; set; }
        [Display(Name = "Address Type")]
        public string addressType { get; set; }
        [Display(Name = "Street Address")]
        public string streetAddress { get; set; }
        [Display(Name = "Unit")]
        public string unit { get; set; }
        [Display(Name = "City")]
        public string locality { get; set; }
        [Display(Name = "State")]
        public string region { get; set; }
        [Display(Name = "Zipcode")]
        public int? zipcode { get; set; }
        [Display(Name = "Country")]
        public string countryName { get; set; }
    }
}