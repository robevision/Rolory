using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Rolory.Models
{
    public class Interaction
    {
        [Key]
        public int id { get; set; }
        [Display(Name = "When")]
        public DateTime moment { get; set; }
        [Display(Name = "Conversation Highlights")]
        [ForeignKey("Message")]
        public int? messageId { get; set; }
        public Message Message { get; set; }
        [ForeignKey("Contact")]
        public int contactId { get; set; }
        public Contact Contact { get; set; }
        [ForeignKey("Networker")]
        [Display(Name = "You")]
        public int networkerId { get; set; }
        public Networker Networker { get; set; }
    }
}