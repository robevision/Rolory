﻿using System;
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
        public int id { get; set; }
        [Display(Name = "First Name")]
        public string firstName { get; set; }
        [Display(Name = "Last Name")]
        public string lastName { get; set; }
        [Display(Name = "Relation")]
        public string relation { get; set; }
        [ForeignKey("Description")]
        [Display(Name = "Description")]
        public int descriptionId { get; set; }
        public Description Description { get; set; }
    }
}