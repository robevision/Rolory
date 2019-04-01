using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Rolory.Models
{
    public class Address
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "Address Type")]
        public string AddressType { get; set; }
        [Display(Name = "Street Address")]
        public string StreetAddress { get; set; }
        [Display(Name = "Unit")]
        public string Unit { get; set; }
        [Display(Name = "City")]
        public string Locality { get; set; }
        [Display(Name = "State")]
        public string Region { get; set; }
        [Display(Name = "Zipcode")]
        public int? ZipCode { get; set; }
        [Display(Name = "Country")]
        public string CountryName { get; set; }
        public float? Latitude { get; set; }
        public float? Longitude { get; set; }
    }
}