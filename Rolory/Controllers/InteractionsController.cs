using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Rolory.Models;

namespace Rolory.Controllers
{
    public class InteractionsController : Controller
    {
        private ApplicationDbContext db;
        private MessageManagement msg;
       public InteractionsController()
        {
            db = new ApplicationDbContext();
            msg = new MessageManagement();
            CycleGoalStatus();
        }
        // GET: Interactions
        public ActionResult Index(int? id, bool? inTouch = null)
        {
          
            string userId = User.Identity.GetUserId();
            var networker = db.Networkers.Where(n => n.UserId == userId).Select(n => n).SingleOrDefault();
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
            int contactId = db.Contacts.Where(c => c.Id == id).Where(c=>c.NetworkerId == networker.Id).Select(c=>c.Id).SingleOrDefault();
            var contact = db.Contacts.Where(c => c.Id == contactId).Select(c => c).SingleOrDefault();
            if (inTouch != null)
            {
                contact.InContact = Convert.ToBoolean(inTouch);
                db.Entry(contact).State = EntityState.Modified;
                db.SaveChanges();

            }
            var interactions = db.Interactions.Where(i => i.ContactId == contactId).Include(i => i.Message);
            ViewBag.Id = contactId;
            return View(interactions.ToList());
        }
        // GET: Interactions/Details/5
        public ActionResult Details(int? id, int? messageId)
        {
            ViewBag.Message = db.Messages.Where(m => m.Id == messageId).SingleOrDefault();
            if (messageId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Interaction interaction = db.Interactions.Find(id);
            if (interaction == null)
            {
                return HttpNotFound();
            }
            return View(interaction);
        }

        // GET: Interactions/Create
        [HttpGet]
        public ActionResult Create(int id)
        {
            Interaction interaction = new Interaction();
            Contact contact = db.Contacts.Where(c => c.Id == id).Select(c => c).SingleOrDefault();
            ContactInteractionViewModel contactInteraction = new ContactInteractionViewModel();
            contactInteraction.Contact = contact;
            ViewBag.Id = id;
            contactInteraction.Interaction = interaction;
            return View(contactInteraction);
        }

        // POST: Interactions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ContactInteractionViewModel contactInteraction)
        {

            if (ModelState.IsValid)
            {
                var recent = DateTime.Today.AddMonths(-3);
                var contact = contactInteraction.Contact;
                var description = db.Descriptions.Where(d => d.Id == contact.DescriptionId).Select(d=>d).SingleOrDefault();
                if (description != null && description.Relationship != null)
                {
                    var relationship = description.Relationship;
                    switch (relationship)
                    {
                        case "Distant":
                            recent = DateTime.Today.AddMonths(-6);
                            break;
                        case "Familiar":
                            recent = DateTime.Today.AddMonths(-3);
                            break;
                        case "Friend":
                            recent = DateTime.Today.AddMonths(-1);
                            break;
                        case "Close":
                            recent = DateTime.Today.AddDays(-6);
                            break;
                        case "Business Superior":
                            recent = DateTime.Today.AddMonths(-8);
                            break;
                        case "Business Equal":
                            recent = DateTime.Today.AddMonths(-8);
                            break;
                        case "Teacher":
                            recent = DateTime.Today.AddMonths(-8);
                            break;
                        case "Classmate":
                            recent = DateTime.Today.AddMonths(-8);
                            break;
                        default:
                            recent = DateTime.Today.AddMonths(-3);
                            break;
                    }
                }

                string goalCode = "5C0R3";
                var contactId = contactInteraction.Contact.Id;
                contactInteraction.Contact = db.Contacts.Where(c => c.Id == contactId).SingleOrDefault();
                contactInteraction.Interaction.Contact = contactInteraction.Contact;
                contactInteraction.Interaction.Contact.Id = contactId;
                contactInteraction.Interaction.ContactId = contactId;
                var networkerId = contactInteraction.Interaction.Contact.NetworkerId;
                var networker = db.Networkers.Where(n => n.Id == networkerId).Select(n => n).SingleOrDefault();
                contactInteraction.Interaction.Message.NetworkerId = networkerId;
                contactInteraction.Interaction.Message.Networker = networker;
                contactInteraction.Interaction.Contact = db.Contacts.Where(c => c.Id == contactInteraction.Interaction.ContactId).Select(c => c).SingleOrDefault();
                contactInteraction.Interaction.Message.NetworkerId = db.Contacts.Where(c => c.Id == contactInteraction.Interaction.ContactId).Select(c => c.NetworkerId).SingleOrDefault();
                contactInteraction.Interaction.Message.Networker = networker;
                contactInteraction.Interaction.Message.Networker.Id = networkerId;
                contactInteraction.Interaction.Message.Postmark = DateTime.Now;
                contactInteraction.Interaction.Message.IsInteraction = true;
                db.Interactions.Add(contactInteraction.Interaction);
                db.SaveChanges();
                var moment = contactInteraction.Interaction.Moment;
                if(moment >= recent)
                {
                    contactInteraction.Contact.InContact = true;
                    contactInteraction.Contact.InContactCountDown = moment;
                    networker.RunningTally++;
                    db.Entry(networker).State = EntityState.Modified;
                    db.SaveChanges();
                    if (networker.Goal != null && networker.RunningTally >= networker.Goal && networker.GoalStatus == true)
                    {
                        networker.GoalCoolDown = DateTime.Now;
                        networker.GoalStatus = false;
                        networker.RunningTally = 0;
                        db.Entry(networker).State = EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("Goal", "Interactions", new { code = goalCode });
                    }
                    return RedirectToAction("Index", new { id = contactInteraction.Contact.Id });
                }
            }
            return View(contactInteraction);
        }
        [HttpGet]
        public ActionResult CreatePlan(int id)
        {
           
            Contact contact = db.Contacts.Where(c => c.Id == id).Select(c => c).SingleOrDefault();
            var reminder = contact.Reminder;
            if(reminder != null)
            {
                //might need another view for modifying reminder event. Maybe an EditPlan.
                RedirectToAction("Details", "Interactions");
            }
            DateTime nonNullReminder = new DateTime(0001, 01, 01);
            ViewBag.Date = nonNullReminder; 
            return View(contact);
        }
        [HttpPost]
        public ActionResult CreatePlan(Contact contact)
        {
            var contactId = db.Contacts.Where(c => c.Id == contact.Id).Select(c => c.Id).SingleOrDefault();
            var reminder = db.Contacts.Where(c => c.Id == contact.Id).Select(c => c.Reminder).SingleOrDefault();
            var year = contact.Reminder.Value.Year.ToString();
            var month = contact.Reminder.Value.Month.ToString();
            var day = contact.Reminder.Value.Day.ToString();
            var time = Request.Form["Time"].ToString();
            var concatReminder = $"{year}, {month}, {day}, {time}";
            DateTime? fullReminder = Convert.ToDateTime(concatReminder);
            contact = db.Contacts.Where(c => c.Id == contactId).Select(c => c).SingleOrDefault();
            contact.Reminder = fullReminder;
            var subject = $"Reach Out To {contact.GivenName} {contact.FamilyName} Today".ToString();
            var body = $"You set a reminder to contact {contact.GivenName} {contact.FamilyName} on {contact.Reminder}. {contact.PhoneNumber} {contact.Email}.".ToString();
            var postMark = contact.Reminder;
            db.Entry(contact).State = EntityState.Modified;
            db.SaveChanges();
            msg.BuildMessage(contact.NetworkerId, subject, body, postMark);
            return RedirectToAction("Index", "Home");
        }
        // GET: Interactions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Interaction interaction = db.Interactions.Find(id);
            if (interaction == null)
            {
                return HttpNotFound();
            }
            interaction.MessageId = db.Messages.Where(m => m.Id == interaction.MessageId).Select(m=>m.Id).SingleOrDefault();
            interaction.Message = db.Messages.Where(m => m.Id == interaction.MessageId).SingleOrDefault();
            return View(interaction);
        }

        // POST: Interactions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Moment,Message,MessageId,ContactId")] Interaction interaction)
        {
            if (ModelState.IsValid)
            {
                interaction.Contact = db.Contacts.Where(c => c.Id == interaction.ContactId).SingleOrDefault();
                interaction.Message.Id = interaction.MessageId.Value;
                interaction.Message.Postmark = DateTime.Now;
                Message message = interaction.Message;
                interaction.Message.NetworkerId = interaction.Contact.NetworkerId;
                db.Entry(message).State = EntityState.Modified;
                db.Entry(interaction).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { id = interaction.ContactId });
            }
            ViewBag.ContactId = new SelectList(db.Contacts, "Id", "Image", interaction.ContactId);
            ViewBag.MessageId = new SelectList(db.Messages, "Id", "Subject", interaction.MessageId);
            return View(interaction);
        }
        public ActionResult DisplayRecent(int id)
        {
            var twoWeeksAgo = DateTime.Now.AddDays(-13);
            string userId = User.Identity.GetUserId();
                    var networker = db.Networkers.Where(n => n.UserId == userId).Select(n => n).SingleOrDefault();
                    var interactionList = db.Interactions.Where(i => i.Moment >= twoWeeksAgo).Where(i => i.Contact.NetworkerId == networker.Id).Select(m => m).ToList();
           foreach(Interaction interaction in interactionList)
            {
                var contactId = db.Interactions.Where(i => i.ContactId == interaction.ContactId).Select(i=>i.ContactId).SingleOrDefault();
                var contact = db.Contacts.Where(c => c.Id == contactId).Select(c => c).SingleOrDefault();
                interaction.Contact = contact;
            }
            return View(interactionList);
        }
        // GET: Interactions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Interaction interaction = db.Interactions.Find(id);
            if (interaction == null)
            {
                return HttpNotFound();
            }
            return View(interaction);
        }

        // POST: Interactions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Interaction interaction = db.Interactions.Find(id);
            db.Interactions.Remove(interaction);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult Goal(string code)
        {

            if (code == "5C0R3")
            {
                return View();
            }
            return RedirectToAction("Index", "Interactions");
        }
        public void CycleGoalStatus()
        {
            var networkers = db.Networkers.Select(n => n).ToList();
            var networkersList = db.Networkers.Where(n => n.GoalActive == true).ToList();
            if (!networkersList.Contains(null))
            {
                foreach (Networker networker in networkersList)
                {
                    if (networker.GoalCoolDown.AddHours(9) >= DateTime.Now)
                    {
                        networker.GoalStatus = true;
                        db.Entry(networker).State = EntityState.Modified;
                    }
                }
                db.SaveChanges();
            }  
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
