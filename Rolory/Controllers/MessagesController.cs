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
    [Authorize]
    public class MessagesController : Controller
    {
        private ApplicationDbContext db;
        private MessageManagement msg;

        public MessagesController()
        {
            db = new ApplicationDbContext();
            msg = new MessageManagement();
        }
        // GET: Messages
        public ActionResult Index()
        {
        
            string userId = User.Identity.GetUserId();
            //null check for networker profile
            var networkerNullCheck = db.Networkers.Where(n => n.UserId == userId).Any();
            if (networkerNullCheck == false)
            {
                return RedirectToAction("CreateAccount", "User");
            }
            var networker = db.Networkers.Where(n => n.UserId == userId).Select(n => n).SingleOrDefault();
            Message noMessageCheck = db.Messages.Select(m => m).FirstOrDefault();
            if(noMessageCheck == null) 
            {
                return View("Empty");
            }
            var messagesNullCheck = db.Messages.Where(m => m.NetworkerId == networker.Id).Where(m => m.IsEmail == false).Where(m => m.IsInteraction == false).Where(m=>m.IsActive == false).Select(m => m).Any();
            if (messagesNullCheck == false)
            {
                return View("Empty");
            }
            var messages = db.Messages.Where(m => m.NetworkerId == networker.Id).Where(m => m.IsEmail == false).Where(m => m.IsInteraction == false).Where(m => m.IsActive != null).Select(m => m).ToList();
            ViewBag.Id = networker.Id;
            return View(messages.ToList());
        }

        // GET: Messages/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Message message = db.Messages.Find(id);
            if (message == null)
            {
                return HttpNotFound();
            }
            message.IsActive = false;
            db.Entry(message).State = EntityState.Modified;
            db.SaveChanges();
            return View(message);
        }
        [HttpGet]
        public ActionResult Get(int id)
        {
           
            msg.GenerateEmail(id);
            return RedirectToAction("Index", "Messages");
        }
        // GET: Messages/Create
        public ActionResult Create()
        {
           
            ViewBag.NetworkerId = new SelectList(db.Networkers, "Id", "FirstName");
            return View();
        }

        // POST: Messages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Subject,Body,Postmark,ToEmail,EmailCC,EmailBCC,IsEmail,IsActive,NetworkerId")] Message message)
        {
            if (ModelState.IsValid)
            {
                db.Messages.Add(message);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.NetworkerId = new SelectList(db.Networkers, "Id", "FirstName", message.NetworkerId);
            return View(message);
        }

        // GET: Messages/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Message message = db.Messages.Find(id);
            if (message == null)
            {
                return HttpNotFound();
            }
            ViewBag.NetworkerId = new SelectList(db.Networkers, "Id", "FirstName", message.NetworkerId);
            return View(message);
        }

        // POST: Messages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Subject,Body,Postmark,ToEmail,EmailCC,EmailBCC,IsEmail,IsActive,NetworkerId")] Message message)
        {
            if (ModelState.IsValid)
            {
                db.Entry(message).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.NetworkerId = new SelectList(db.Networkers, "Id", "FirstName", message.NetworkerId);
            return View(message);
        }

        // GET: Messages/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Message message = db.Messages.Find(id);
            if (message == null)
            {
                return HttpNotFound();
            }
           
            db.Messages.Remove(message);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        // POST: Messages/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
       
        //}

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
