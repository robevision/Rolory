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
        public int Id { get; set; }
        [DataType(DataType.Upload)]
        [Display(Name = "Upload File")]
        public string Image { get; set; }
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Display(Name = "Prefix")]
        public string Prefix { get; set; }
        [Display(Name = "First Name")]
        public string GivenName { get; set; }
        [Display(Name = "Last Name")]
        public string FamilyName { get; set; }
        [Display(Name = "Phone Type")]
        public string PhoneType { get; set; }
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
        [Display(Name = "Organization")]
        public string Organization { get; set; }
        [Display(Name = "Work Title")]
        public string WorkTitle { get; set; }
        [Display(Name = "Alternate Phone Type")]
        public string AltPhoneNumberType { get; set; }
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Phone Number")]
        public string AltPhoneNumber { get; set; }
        [Display(Name = "Last Modified")]
        public DateTime? LastUpdated { get; set; }
        [Display(Name = "In Touch")]
        public bool InContact { get; set; }
        public DateTime? InContactCountDown { get; set; }
        public bool Perpetual { get; set; }
        public bool CoolDown { get; set; }
        public DateTime? CoolDownTime { get; set; }
        [DataType(DataType.Date)]
        public DateTime? Reminder { get; set; }
        [ForeignKey("Address")]
        public int? AddressId { get; set; }
        public Address Address { get; set; }
        [ForeignKey("AlternateAddress")]
        [Display(Name = "Alternate Address")]
        public int? AltAddressId { get; set; }
        public Address AlternateAddress { get; set; }
        [ForeignKey("Description")]
        public int? DescriptionId { get; set; }
        public Description Description { get; set; }
        [ForeignKey("Networker")]
        public int NetworkerId { get; set; }
        public Networker Networker { get; set; }
    }
}