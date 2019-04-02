using Rolory.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rolory.Controllers
{
    public class RandomManagement
    {
        ApplicationDbContext db;
        public RandomManagement()
        {
            db = new ApplicationDbContext();
        }
        public List<Contact> GetContactsByBirthDate()
        {
            List<Contact> pushedContactsByBirthDate = new List<Contact>();

            return pushedContactsByBirthDate;
        }
    }
}