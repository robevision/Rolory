using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Rolory.Models
{
    public class Networker
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Display(Name = "Email Notifications")]
        public bool ReceiveEmails { get; set; }
        public bool EmailQuota { get; set; }
        public int EmailFrequency { get; set; }
        [DataType(DataType.MultilineText)]
        [Display(Name = "Activities")]
        public string UserActivities { get; set; }
        public string Occupation { get; set; }
        [Display(Name = "Title")]
        public string WorkTitle { get; set; }
        public int RunningTally { get; set; }
        public int? Goal { get; set; }
        public bool GoalActive { get; set; }
        public bool GoalStatus { get; set; }
        public string Availability { get; set; }
        [ForeignKey("ApplicationUser")]
        public string UserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        [ForeignKey("Address")]
        public int? AddressId { get; set; }
        public Address Address { get; set; }
    }
}