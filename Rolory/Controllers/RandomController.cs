using Microsoft.AspNet.Identity;
using Rolory.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Rolory.Controllers
{
    public class RandomController : Controller
    {
        ApplicationDbContext db;
        Random random;
        public RandomController()
        {
            db = new ApplicationDbContext();
            random = new Random();
        }
        // GET: Random
        public ActionResult Index()
        {
            var nextWeek = DateTime.Today.AddDays(7);
            List<Contact> pushedContacts = new List<Contact>();
            List<Contact> pushedContactsByBirthDate = new List<Contact>();
            List<Contact> pushedContactsByAnniversaryDate = new List<Contact>();
            List<Contact> pushedContactsByProfession = new List<Contact>();
            List<Contact> pushedContactsByRelationship = new List<Contact>();
            string userId = User.Identity.GetUserId();
            var networker = db.Networkers.Where(n => n.UserId == userId).SingleOrDefault();
            var contactList = db.Contacts.Where(c => c.NetworkerId == networker.Id).Where(c=>c.InContact == false).Where(c=>c.CoolDown == false).Where(c=>c.Description.DeathDate == null).ToList();
            var filteredContactList = contactList.Where(c => c.Perpetual == false).ToList();
            foreach(Contact contact in filteredContactList)
            {
                DateTime contactBirthDate = contact.Description.BirthDate.Value;
                int contactBirthMonth = contactBirthDate.Month;
                var contactWorkTitle = contact.WorkTitle;
                DateTime contactAnniversary = contact.Description.Anniversary.Value;
                int contactAnniversaryMonth = contactAnniversary.Month;
                var contactRelation = contact.Description.Relationship;
                if(contactBirthDate != null)
                {
                    if (contactBirthMonth == DateTime.Today.Month || contactBirthDate <= nextWeek)
                    {
                        pushedContactsByBirthDate.Add(contact);
                    }
                }
                if(contactWorkTitle != null && networker.WorkTitle != null)
                {
                    if (contactWorkTitle.Contains(networker.WorkTitle))
                    {
                        pushedContactsByProfession.Add(contact);
                    }
                }
                if (contactAnniversary != null)
                {
                    if (contactAnniversaryMonth == DateTime.Today.Month || contactAnniversary <= nextWeek)
                    {
                        pushedContactsByAnniversaryDate.Add(contact);
                    }
                }
                if (contactRelation != null)
                {
                    if (contactRelation == "Friend" || contactRelation == "Family")
                    {
                        pushedContactsByRelationship.Add(contact);
                    }
                }
            }
            pushedContacts.Add(pushedContactsByBirthDate.SingleOrDefault());
            pushedContacts.Add(pushedContactsByAnniversaryDate.SingleOrDefault());
            pushedContacts.Add(pushedContactsByProfession.SingleOrDefault());
            pushedContacts.Add(pushedContactsByRelationship.SingleOrDefault());
            if (pushedContacts == null)
            {
                Contact contact = null;
                do
                {
                    int r = random.Next(filteredContactList.Count);
                    contact = filteredContactList[r];
                }
                while (contact == null);
                return View(contact);
            }
            Contact filteredContact = null;
            do
            {
                int r = random.Next(pushedContacts.Count);
                filteredContact = pushedContacts[r];
            }
            while (filteredContact == null);
            if(filteredContact.Id == pushedContactsByBirthDate.Select(p => p.Id).SingleOrDefault())
            {
                ViewBag.Message = $"It is {filteredContact.GivenName}'s birthday soon. Why not reach out?";
            }
            return View();
        }
        public ActionResult Index(Contact contact)
        {
            return RedirectToAction("Index", "Random");
        }
        // GET: Random/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Random/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Random/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Random/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Random/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Random/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Random/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
