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
        public int Id { get; set; }
        [Display(Name = "When")]
        public DateTime Moment { get; set; }
        [Display(Name = "Conversation Highlights")]
        [ForeignKey("Message")]
        public int? MessageId { get; set; }
        public Message Message { get; set; }
        [ForeignKey("Contact")]
        public int ContactId { get; set; }
        public Contact Contact { get; set; }
    }
}