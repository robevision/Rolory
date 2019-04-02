﻿using Microsoft.AspNet.Identity;
using Rolory.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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
        [Authorize]
        [HttpGet]
        public ActionResult Index()
        {
            //msg.BuildEmail(3, "Test", "Testy test test. This is a test to see if this email works. Testing testing, test, test.");
            var nextWeek = DateTime.Today.AddDays(7);
            List<Contact> pushedContacts = new List<Contact>();
            List<Contact> pushedContactsByBirthDate = new List<Contact>();
            List<Contact> pushedContactsByAnniversaryDate = new List<Contact>();
            List<Contact> pushedContactsByProfession = new List<Contact>();
            List<Contact> pushedContactsByRelationship = new List<Contact>();
            string userId = User.Identity.GetUserId();
            var networker = db.Networkers.Where(n => n.UserId == userId).SingleOrDefault();
            var networkerNullCheck = db.Networkers.Where(n => n.UserId == userId).Any();
            if (networkerNullCheck == false)
            {
               return RedirectToAction("CreateAccount", "User");
            }
            var contactsNullCheck = db.Contacts.Where(c => c.NetworkerId == networker.Id).ToList();
            if (contactsNullCheck[0] == null || contactsNullCheck.Count() == 0)
            {
                //Add a page to send the logged in user to a message that says they have no contacts logged
               return RedirectToAction("Create", "Contacts");
            }
            DateTime endCoolDownTimer = DateTime.Now;
            List<Contact> contactsBackInPool = new List<Contact>();
            var contactsWithCoolDown = db.Contacts.Where(c => c.CoolDown == true).ToList();
            foreach (Contact contact in contactsWithCoolDown)
            {
                if(contact.CoolDownTime.Value.AddHours(1) <= endCoolDownTimer)
                {
                    contactsBackInPool.Add(contact);
                }
            }
            List<bool> coolDownProperty = contactsBackInPool.Where(c => c.CoolDown == true).Select(c=>c.CoolDown).ToList();
            for (int i = 0; i < coolDownProperty.Count(); i++)
            {
                coolDownProperty[i] = false;
            }
            foreach (Contact contact in contactsBackInPool)
            {
                foreach(bool coolDownBool in coolDownProperty)
                {
                    contact.CoolDown = coolDownBool;
                    db.Entry(contact).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            var contactList = db.Contacts.Where(c => c.NetworkerId == networker.Id).Where(c=>c.InContact == false).Where(c=>c.CoolDown == false).Where(c=>c.Description.DeathDate == null).ToList();
            var filteredContactList = contactList.Where(c => c.Perpetual == false).ToList();
            foreach(Contact contact in filteredContactList)
            {
                contact.Description = db.Contacts.Where(c=>c.DescriptionId == contact.DescriptionId).Select(c=>c.Description).SingleOrDefault();
                DateTime? birthDateNullTest = contact.Description.BirthDate;
                DateTime? anniversaryDateNullTest = contact.Description.Anniversary;
                if (birthDateNullTest == null)
                {
                    contact.Description.BirthDate = new DateTime(1801, 12, 25);
                }
                DateTime contactBirthDate = contact.Description.BirthDate.Value;
                int contactBirthMonth = contactBirthDate.Month;
                var contactWorkTitle = contact.WorkTitle;
                if (anniversaryDateNullTest == null)
                {
                    contact.Description.Anniversary = new DateTime(1801, 12, 25);
                }
                DateTime contactAnniversary = contact.Description.Anniversary.Value;
                int contactAnniversaryMonth = contactAnniversary.Month;
                string contactRelation = contact.Description.Relationship;
                //need to query for networker id by getting list of contacts with description. Basically all contacts with description. Their ids then foreach to query whether those match with the ones thathave sharedactivities attached.
                List<int> contactActivities = db.SharedActivities.Select(s => s.Description.Id).ToList();
                //
                if(!contactBirthDate.ToString().Contains("12/25/1801"))
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
                if (!contactAnniversary.ToString().Contains("12/25/1801"))
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
                if (contactActivities != null)
                {

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
                    contact.CoolDown = true;
                    string timeNow = DateTime.Now.ToString();
                    DateTime? nullableTimeNow = Convert.ToDateTime(timeNow);
                    contact.CoolDownTime = nullableTimeNow;
                    db.Entry(contact).State = EntityState.Modified;
                    db.SaveChanges();
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
            
            filteredContact.CoolDown = true;
            filteredContact.CoolDownTime = DateTime.Now;
            db.Entry(filteredContact).State = EntityState.Modified;
            db.SaveChanges();
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
        public ActionResult GetInTouch(int? id)
        {
            string userId = User.Identity.GetUserId();
            var networker = db.Networkers.Where(n => n.UserId == userId).SingleOrDefault();
            var today = DateTime.Now.Day;
            var week = DateTime.Now.AddDays(6);
            DateTime weekAgo = DateTime.Now.AddDays(-6);
            var contact = db.Contacts.Where(c => c.Id == id).Select(c => c).SingleOrDefault();
           

            if(contact.WorkTitle != null)
            {
                if (contact.WorkTitle == networker.WorkTitle || contact.WorkTitle.Contains(networker.WorkTitle))
                {
                    ViewBag.Message = $"You and {contact.GivenName} may have a similar occupation. Why not set up a coffee to share your experiences?";
                }
            }
            if(contact.Description.Relationship != null)
            {
                if (contact.Description.Relationship == "Friend")
                {
                    
                }
                if(contact.Description.Relationship == "Family")
                {

                }
                if(contact.Description.Relationship == "Classmate")
                {

                }
            }
            if (contact.Description.Anniversary.Value != null)
            {
                if (contact.Description.Anniversary.Value.Month == DateTime.Now.Month)
                {
                    if (contact.Description.Anniversary.Value.Day == today)
                    {
                        ViewBag.Message = $"It is {contact.GivenName}'s Wedding Anniversary today! Just a simple 'Happy Anniversary' can get a conversation going about how both of you have been!";
                    }
                    else if (contact.Description.Anniversary.Value.Day > today && contact.Description.Anniversary.Value <= weekAgo)
                    {
                        ViewBag.Message = $"It was {contact.GivenName}'s Wedding Anniversary on {contact.Description.Anniversary.Value.DayOfWeek}! Just a simple 'Happy Anniversary' shows you're thinking of them.";
                    }
                    else if (contact.Description.Anniversary.Value.Day < today && contact.Description.Anniversary.Value >= week)
                    {
                        ViewBag.Message = $"{contact.GivenName}'s Anniversary is coming up! It's on {contact.Description.BirthDate.Value.DayOfWeek}! Reaching out with a 'Happy Anniversary' shows you're thinking of them.";
                    }
                }
            }
            if (contact.Description.BirthDate.Value != null)
            {
                if (contact.Description.BirthDate.Value.Month == DateTime.Now.Month)
                {
                    if (contact.Description.BirthDate.Value.Day == today)
                    {
                        ViewBag.Message = $"It is {contact.GivenName}'s Birthday today! Just a simple 'Happy Birthday' can get a conversation going about how both of you have been!";
                    }
                    else if (contact.Description.BirthDate.Value.Day > today && contact.Description.BirthDate.Value <= weekAgo)
                    {
                        ViewBag.Message = $"It was {contact.GivenName}'s Birthday on {contact.Description.BirthDate.Value.DayOfWeek}! Just a simple 'Happy Belated Birthday' shows you're thinking of them.";
                    }
                    else if (contact.Description.BirthDate.Value.Day < today && contact.Description.BirthDate.Value >= week)
                    {
                        ViewBag.Message = $"{contact.GivenName}'s Birthday is coming up! It's on {contact.Description.BirthDate.Value.DayOfWeek}! Reaching out with a 'Happy Birthday' shows you're thinking of them.";
                    }
                }
            }
            return View(contact);
        }
        // GET: Random/Details/5

        public ActionResult ProvideContext(int? id)
        {
            Contact contact = db.Contacts.Where(c => c.Id == id).Select(c => c).SingleOrDefault();
            //Update Info About Person or Log A Moment
            return View(contact);

        }
        public ActionResult UpdateInfo(int? id, string question=null, string answer=null)
        {
            Contact contact = db.Contacts.Where(c => c.Id == id).Select(c => c).SingleOrDefault();
            //Update Info. Will be recursive in adding info about the person to log. Have a button for when they want to go to the next person.
            if (question != null && answer != null)
            {
                ViewBag.Message = "hi";
            }
            
            return View(contact);
        }
        public ActionResult ConnectQuery(int? id)
        {
            Contact contact = db.Contacts.Where(c => c.Id == id).Select(c => c).SingleOrDefault(); 
            //Would you like to connect?
            return View(contact);
        }
        public ActionResult Interact(int? id)
        {
            Contact contact = db.Contacts.Where(c => c.Id == id).Select(c => c).SingleOrDefault();
            return View(contact);
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
