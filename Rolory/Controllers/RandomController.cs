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
        private ApplicationDbContext db;
        private MessageManagement msg;
        Random random;
        public RandomController()
        {
            db = new ApplicationDbContext();
            random = new Random();
            msg = new MessageManagement();
        }
        // GET: Random
        [HttpGet]
        public ActionResult Index()
        {
            msg.BuildEmail(3, "Test", "Testy test test. This is a test to see if this email works. Testing testing, test, test.");
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
                contact.Description = db.Contacts.Where(c=>c.DescriptionId == contact.DescriptionId).Select(c=>c.Description).SingleOrDefault();
                DateTime? birthDateNullTest = contact.Description.BirthDate;
                DateTime? anniversaryDateNullTest = contact.Description.Anniversary;
                if (birthDateNullTest == null)
                {
                    contact.Description.BirthDate = new DateTime(0001, 12, 25);
                }
                DateTime contactBirthDate = contact.Description.BirthDate.Value;
                int contactBirthMonth = contactBirthDate.Month;
                var contactWorkTitle = contact.WorkTitle;
                if (anniversaryDateNullTest == null)
                {
                    contact.Description.Anniversary = new DateTime(0001, 12, 25);
                }
                DateTime contactAnniversary = contact.Description.Anniversary.Value;
                int contactAnniversaryMonth = contactAnniversary.Month;
                var contactRelation = contact.Description.Relationship;
                if(!contactBirthDate.ToString().Contains("12/25/0001"))
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
                if (!contactAnniversary.ToString().Contains("12/25/0001"))
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
            if (pushedContacts.Count == 4 && pushedContacts[0] == null && pushedContacts[1] == null && pushedContacts[2] == null && pushedContacts[3] == null)
            {
                if(filteredContactList.Count != 0)
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
                return RedirectToAction("Complete", "Random");
              
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
            else if (filteredContact.Id == pushedContactsByAnniversaryDate.Select(p => p.Id).SingleOrDefault())
            {
                ViewBag.Message = $"It is {filteredContact.GivenName}'s anniversary soon. Why not reach out?";
            }
            else if (filteredContact.Id == pushedContactsByProfession.Select(p => p.Id).SingleOrDefault())
            {
                ViewBag.Message = $"{filteredContact.GivenName} and you share a career. Why not reach out?";
            }
            else if (filteredContact.Id == pushedContactsByRelationship.Select(p => p.Id).SingleOrDefault())
            {
                ViewBag.Message = $"You should get back in touch with {filteredContact.GivenName}.It's been a while.";
            }
           
            return View(filteredContact);
        }
        [HttpPost]
        public ActionResult Index(Contact contact)
        {
            return RedirectToAction("Index", "Random");
        }
        public ActionResult Complete()
        {
            return View();
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
