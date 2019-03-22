using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Rolory.Models
{
    public class SharedActivity
    {
        [Key]
        public int id { get; set; }
        [Display(Name = "Type")]
        public string type { get; set; }
        [Display(Name = "Activity")]
        public string activity { get; set; }
        [Display(Name = "Level")]
        public string bond { get; set; }
        [ForeignKey("Description")]
        [Display(Name = "Description")]
        public int descriptionId { get; set; }
        public Description Description { get; set; }
    }
}