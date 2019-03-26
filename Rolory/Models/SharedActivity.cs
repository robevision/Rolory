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
        public int Id { get; set; }
        [Display(Name = "Type")]
        public string Type { get; set; }
        [Display(Name = "Activity")]
        public string Activity { get; set; }
        [Display(Name = "Level")]
        public string Bond { get; set; }
        [ForeignKey("Description")]
        [Display(Name = "Description")]
        public int DescriptionId { get; set; }
        public Description Description { get; set; }
    }
}