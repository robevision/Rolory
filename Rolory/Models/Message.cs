using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Rolory.Models
{
    public class Message
    {
        [Key]
        public int id { get; set; }
        [Display(Name = "Subject")]
        public string subject { get; set; }
        [Display(Name = "Body")]
        public string body { get; set; }
        [Display(Name = "Date Received")]
        public DateTime postmark { get; set; }
        [Display(Name = "New")]
        public bool? isActive { get; set; }
        [ForeignKey("Networker")]
        public int networkerId { get; set; }
        public Networker Networker { get; set; }
    }
}