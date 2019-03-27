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
        public int Id { get; set; }
        [Display(Name = "Subject")]
        public string Subject { get; set; }
        [DataType(DataType.MultilineText)]
        [Display(Name = "Body")]
        public string Body { get; set; }
        [Display(Name = "Date Received")]
        public DateTime Postmark { get; set; }
        [DataType(DataType.EmailAddress), Display(Name = "To")]
        public string ToEmail { get; set; }
        [DataType(DataType.EmailAddress)]
        [Display(Name = "CC")]
        public string EmailCC { get; set; }
        [DataType(DataType.EmailAddress)]
        [Display(Name = "BCC")]
        public string EmailBCC { get; set; }
        [Display(Name = "New")]
        public bool? IsActive { get; set; }
        [ForeignKey("Networker")]
        public int NetworkerId { get; set; }
        public Networker Networker { get; set; }
    }
}