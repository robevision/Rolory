using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rolory.Models
{
    public class ContactDescriptionViewModel
    {
        public Contact Contact { get; set; }
        public Description Description { get; set; }
    }
    public class ContactFamilyMemberViewModel
    {
        public Contact Contact { get; set; }
        public FamilyMember FamilyMember { get; set; }
    }
    public class ContactSharedActivityViewModel
    {
        public Contact Contact { get; set; }
        public SharedActivity SharedActivity { get; set; }
    }
    public class ContactInteractionViewModel
    {
        public Contact Contact { get; set; }
        public Interaction Interaction { get; set; }
    }
}