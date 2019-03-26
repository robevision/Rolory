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
        public int Id { get; set; }
        [Display(Name = "Gender")]
        public string Gender { get; set; }
        [Display(Name = "Relationship")]
        public string Relationship { get; set; }
        [Display(Name = "Category")]
        public string Category { get; set; }
        [Display(Name = "Birth Date")]
        public DateTime? BirthDate { get; set; }
        [Display(Name = "Death Date")]
        public DateTime? DeathDate { get; set; }
        [Display(Name = "Anniversary")]
        public DateTime? Anniversary { get; set; }
        [Display(Name = "General Notes")]
        public string Notes { get; set; }

    }
}