using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Rolory.Models
{
    public class Description
    {
        [Key]
        public int id { get; set; }
        [Display(Name = "Gender")]
        public string gender { get; set; }
        [Display(Name = "Relationship")]
        public string relationship { get; set; }
        [Display(Name = "Category")]
        public string category { get; set; }
        [Display(Name = "Birth Date")]
        public DateTime? birthDate { get; set; }
        [Display(Name = "Death Date")]
        public DateTime? deathDate { get; set; }
        [Display(Name = "Anniversary")]
        public DateTime? anniversary { get; set; }
        [Display(Name = "General Notes")]
        public List <string> notes { get; set; }

    }
}