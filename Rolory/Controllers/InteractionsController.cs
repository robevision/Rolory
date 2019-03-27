using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Rolory.Models;

namespace Rolory.Controllers
{
    public class InteractionsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Interactions
        public ActionResult Index(int? id)
        {
            int contactId = db.Contacts.Where(c => c.Id == id).Select(c=>c.Id).SingleOrDefault();
            var interactions = db.Interactions.Include(i => i.Contact).Include(i => i.Message);
            ViewBag.Id = contactId;
            return View(interactions.ToList());
        }
        // GET: Interactions/Details/5
        public ActionResult Details(int? id)
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

        // GET: Interactions/Create
        [HttpGet]
        public ActionResult Create(int id)
        {
            Interaction interaction = new Interaction();
            Contact contact = db.Contacts.Where(c => c.Id == id).Select(c => c).SingleOrDefault();
            ContactInteractionViewModel contactInteraction = new ContactInteractionViewModel();
            contactInteraction.Contact = contact;
            //interaction.ContactId = id;
            //interaction.Contact = contact;
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
                contactInteraction.Interaction.ContactId = contactInteraction.Contact.Id;
                contactInteraction.Interaction.Contact = db.Contacts.Where(c => c.Id == contactInteraction.Interaction.ContactId).Select(c => c).SingleOrDefault();
                contactInteraction.Interaction.Message.NetworkerId = db.Contacts.Where(c => c.Id == contactInteraction.Interaction.ContactId).Select(c => c.NetworkerId).SingleOrDefault();
                contactInteraction.Interaction.Message.Networker = db.Contacts.Where(c => c.Id == contactInteraction.Interaction.ContactId).Select(c => c.Networker).SingleOrDefault();
                contactInteraction.Interaction.Message.Postmark = DateTime.Now;
                db.Interactions.Add(contactInteraction.Interaction);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(contactInteraction);
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
            ViewBag.ContactId = new SelectList(db.Contacts, "Id", "Image", interaction.ContactId);
            ViewBag.MessageId = new SelectList(db.Messages, "Id", "Subject", interaction.MessageId);
            return View(interaction);
        }

        // POST: Interactions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Moment,MessageId,ContactId")] Interaction interaction)
        {
            if (ModelState.IsValid)
            {
                db.Entry(interaction).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ContactId = new SelectList(db.Contacts, "Id", "Image", interaction.ContactId);
            ViewBag.MessageId = new SelectList(db.Messages, "Id", "Subject", interaction.MessageId);
            return View(interaction);
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

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}
    }
}
