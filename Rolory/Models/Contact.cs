using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Rolory.Models
{
    public class Contact
    {
        [Key]
        public int id { get; set; }
        [Display(Name = "Upload File")]
        public string image { get; set; }
        [Display(Name = "Email")]
        public string email { get; set; }
        [Display(Name = "Prefix")]
        public string prefix { get; set; }
        [Display(Name = "First Name")]
        public string givenName { get; set; }
        [Display(Name = "Last Name")]
        public string familyName { get; set; }
        [Display(Name = "Phone Type")]
        public string phoneType { get; set; }
        [Display(Name = "Phone Number")]
        public int? phoneNumber { get; set; }
        [Display(Name = "Organization")]
        public string organization { get; set; }
        [Display(Name = "Work Title")]
        public string workTitle { get; set; }
        [Display(Name = "Alternate Phone Type")]
        public string altPhoneNumberType { get; set; }
        [Display(Name = "Phone Number")]
        public int? altPhoneNumber { get; set; }
        [Display(Name = "Last Modified")]
        public DateTime? lastupdated { get; set; }
        [Display(Name = "In Touch")]
        public bool inContact { get; set; }
        [ForeignKey("Address")]
        public int? addressId { get; set; }
        public Address Address { get; set; }
        [ForeignKey("AlternateAddress")]
        [Display(Name = "Alternate Address")]
        public int? altAddressId { get; set; }
        public Address AlternateAddress { get; set; }
        [ForeignKey("Description")]
        public int? descriptionId { get; set; }
        public Description Description { get; set; }
        [ForeignKey("Networker")]
        public int networkerId { get; set; }
        public Networker Networker { get; set; }
    }
}