using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Rolory.Models
{
    public class FamilyMember
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Display(Name = "Relation")]
        public string Relation { get; set; }
        [ForeignKey("Description")]
        [Display(Name = "Description")]
        public int DescriptionId { get; set; }
        public Description Description { get; set; }
    }
}