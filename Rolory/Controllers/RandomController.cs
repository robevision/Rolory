using Microsoft.AspNet.Identity;
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
        private readonly MessageManagement msg;
        private RandomManagement rndmngmnt;
        Random random;
        public RandomController()
        {
            db = new ApplicationDbContext();
            random = new Random();
            msg = new MessageManagement();
            rndmngmnt = new RandomManagement();
        }
        // GET: Random
        [Authorize]
        [HttpGet]
        public ActionResult Index()
        {
            var nextWeek = DateTime.Today.AddDays(6);
            //instantiated lists
            List<Contact> pushedContacts = new List<Contact>();
            List<Contact> newPushedContacts = new List<Contact>();
            List<Contact> pushedContactsByBirthDate = new List<Contact>();
            List<Contact> pushedContactsByAnniversaryDate = new List<Contact>();
            List<Contact> pushedContactsByProfession = new List<Contact>();
            List<Contact> pushedContactsByRelationship = new List<Contact>();
            List<Contact> pushedContactsBySharedActivities = new List<Contact>();

            List<Contact> filteredContactsBySharedActivities = new List<Contact>();

            //get who is logged in and only their contacts
            string userId = User.Identity.GetUserId();
            var networker = db.Networkers.Where(n => n.UserId == userId).SingleOrDefault();

            //null check for networker profile
            var networkerNullCheck = db.Networkers.Where(n => n.UserId == userId).Any();
            if (networkerNullCheck == false)
            {
               return RedirectToAction("CreateAccount", "User");
            }
            var contactsNullCheck = db.Contacts.Where(c => c.NetworkerId == networker.Id).ToList();
            try
            {
                if (contactsNullCheck[0] == null || contactsNullCheck.Count() == 0)
                {

                    return RedirectToAction("Null", "Contacts");
                }
            }
            catch
            {
                return RedirectToAction("Null", "Contacts");
            }

            rndmngmnt.CheckContactCoolDown();
            var contactList = db.Contacts.Where(c => c.NetworkerId == networker.Id).Where(c=>c.InContact == false).Where(c=>c.CoolDown == false).Where(c=>c.Description.DeathDate == null).ToList();
            var filteredContactList = contactList.Where(c => c.Perpetual == false).ToList();

            pushedContactsByBirthDate = rndmngmnt.GetContactsByBirthDate(filteredContactList);
            pushedContactsByAnniversaryDate = rndmngmnt.GetContactsByAnniversary(filteredContactList);
            pushedContactsByProfession = rndmngmnt.GetContactsByWorkTitle(filteredContactList);
            pushedContactsByRelationship = rndmngmnt.GetContactsByRelation(filteredContactList);

            //filteredContactsBySharedActivities = rndmngmnt.GetContactsBySharedActivities(filteredContactList);
            //rndmngmnt.CheckSharedActivitiesWithSeason(filteredContactsBySharedActivities);
            if(pushedContactsByBirthDate != null)
            {
                foreach(Contact contact in pushedContactsByBirthDate)
                {
                    pushedContacts.Add(contact);
                }
            }
            if(pushedContactsByAnniversaryDate != null)
            {
                foreach(Contact contact in pushedContactsByAnniversaryDate)
                {
                    pushedContacts.Add(contact);
                }
            }
            if(pushedContactsByProfession != null)
            {
                foreach(Contact contact in pushedContactsByProfession)
                {
                    pushedContacts.Add(contact);
                }
            }
            if(pushedContactsByRelationship != null)
            {
                foreach(Contact contact in pushedContactsByRelationship)
                {
                    pushedContacts.Add(contact);
                }
            }
            pushedContacts.Add(pushedContactsBySharedActivities.SingleOrDefault());
            foreach(Contact contact in pushedContacts)
            {
                if(contact != null)
                {
                    newPushedContacts.Add(contact);
                }
            }
            if (newPushedContacts.Count == 0)
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
                    contact = db.Contacts.Where(c => c.Id == contact.Id).Select(c => c).SingleOrDefault();
                    contact.CoolDown = true;
                    contact.CoolDownTime = DateTime.Now;
                    string timeNow = DateTime.Now.ToString();
                    DateTime? nullableTimeNow = Convert.ToDateTime(timeNow);
                    contact.Description = db.Descriptions.Where(d => d.Id == contact.DescriptionId).Select(d => d).SingleOrDefault();
                    var thisCoolDownTime = db.Contacts.Where(c => c.Id == contact.Id).Select(c => c.CoolDownTime).SingleOrDefault();
                    thisCoolDownTime = contact.CoolDownTime;
                    var thisCoolDown = db.Contacts.Where(c => c.Id == contact.Id).Select(c => c.CoolDown).SingleOrDefault();
                    thisCoolDown = contact.CoolDown;
                    var thisDescriptionId = db.Contacts.Where(c => c.Description.Id == contact.Description.Id).Select(c => c.Description.Id).SingleOrDefault();
                    thisDescriptionId = contact.Description.Id;
                    var thisContactDescriptionId = db.Contacts.Where(c => c.DescriptionId == contact.DescriptionId).Select(c => c.DescriptionId).SingleOrDefault();
                    thisContactDescriptionId = contact.DescriptionId;
                    db.Entry(contact).State = EntityState.Modified;
                    db.SaveChanges();
                    return View(contact);
                }
                return RedirectToAction("Complete", "Random");
              
            }
            Contact filteredContact = null;
            if(pushedContacts.Count != 0)
            {
                do
                {
                    int r = random.Next(pushedContacts.Count);
                    if (pushedContacts.Count == 1)
                    {
                        filteredContact = pushedContacts.SingleOrDefault();
                        if(filteredContact == null)
                        {
                            break;
                        }
                    }
                    filteredContact = pushedContacts[r];
                }
                while (filteredContact == null);
            }
           
            if(pushedContactsByBirthDate.Select(p => p.Id).Contains(filteredContact.Id))
            {
                ViewBag.Message = $"It is {filteredContact.GivenName}'s birthday soon. Why not reach out?";
            }
            else if (pushedContactsByAnniversaryDate.Select(p => p.Id).Contains(filteredContact.Id))
            {
                ViewBag.Message = $"It is {filteredContact.GivenName}'s anniversary soon. Why not reach out?";
            }
            else if (pushedContactsByProfession.Select(p => p.Id).Contains(filteredContact.Id))
            {
                ViewBag.Message = $"{filteredContact.GivenName} and you share a career. Why not reach out?";
            }
            else if (pushedContactsByRelationship.Select(p => p.Id).Contains(filteredContact.Id))
            {
                ViewBag.Message = $"You should get back in touch with {filteredContact.GivenName}.It's been a while.";
            }
            if(filteredContact.AddressId != null)
            {
                filteredContact.AddressId = db.Contacts.Where(c => c.Id == filteredContact.Id).Select(c => c.AddressId).SingleOrDefault();
            }
            filteredContact = db.Contacts.Where(c => c.Id == filteredContact.Id).Select(c => c).SingleOrDefault();
            filteredContact.CoolDown = true;
            filteredContact.CoolDownTime = DateTime.Now;
            filteredContact.Description = db.Descriptions.Where(d => d.Id == filteredContact.DescriptionId).Select(d => d).SingleOrDefault();
            var descriptionId = db.Contacts.Where(c => c.Description.Id == filteredContact.Description.Id).Select(c => c.Description.Id).SingleOrDefault();
            descriptionId = filteredContact.Description.Id;
            var contactDescriptionId = db.Contacts.Where(c => c.DescriptionId == filteredContact.DescriptionId).Select(c => c.DescriptionId).SingleOrDefault();
            contactDescriptionId = filteredContact.DescriptionId;
            var coolDownTime = db.Contacts.Where(c => c.Id == filteredContact.Id).Select(c => c.CoolDownTime).SingleOrDefault();
            coolDownTime = filteredContact.CoolDownTime;
            var coolDown = db.Contacts.Where(c => c.Id == filteredContact.Id).Select(c => c.CoolDown).SingleOrDefault();
            coolDown = filteredContact.CoolDown;
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
           

            if(contact.WorkTitle != null && networker.WorkTitle != null)
            {
                if (contact.WorkTitle == networker.WorkTitle || contact.WorkTitle.Contains(networker.WorkTitle))
                {
                    ViewBag.Message = $"You and {contact.GivenName} may have a similar occupation. Why not set up a coffee to share your experiences?";
                }
            }
            if(contact.Description != null && contact.Description.Relationship != null)
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
            if (contact.Description != null && contact.Description.Anniversary.Value != null)
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
            if (contact.Description != null && contact.Description.BirthDate.Value != null)
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
            else if(contact.Description == null)
            {
                WeatherManagement weath = new WeatherManagement();
                var season = weath.GetCurrentSeason();
                string[] dateRelevantMessages = new string[]
                {

                };
                string[] genericMessages = new string[]
                {
                    $"How about, 'Hey {contact.GivenName}, I saw something the other day that made me think of you...'",
                    "'What have you been up to lately?'", $"'Hi {contact.GivenName}, how has it been since...'",
                    "'You came up on my feed because of...we should catch up!'", "'I hope you are having a great day!'",
                    "'How about, 'Hey, It's been a while since we last spoke, what have you been up to?'",
                    $"'Hey there {contact.GivenName}. Had a memory recently of when... How have you been?'",
                    "'What are you up to these days? We should grab coffee to catch up!'"
                };
                random = new Random();
                var newRandom = random.Next(8);
                ViewBag.Message = genericMessages[newRandom];

              

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
