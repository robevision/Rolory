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
        [DataType(DataType.MultilineText)]
        [Display(Name = "Activities")]
        public string UserActivities { get; set; }
        public string Occupation { get; set; }
        [Display(Name = "Title")]
        public string WorkTitle { get; set; }
        [ForeignKey("ApplicationUser")]
        public string UserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}