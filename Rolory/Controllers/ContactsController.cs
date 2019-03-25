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
    public class ContactsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Contacts
        public ActionResult Index()
        {
            var contacts = db.Contacts.Include(c => c.Address).Include(c => c.AlternateAddress).Include(c => c.Description).Include(c => c.Networker);
            return View(contacts.ToList());
        }

        // GET: Contacts/Details/5
        public ActionResult Details(int? id)
        {
            if (ModelState.IsValid)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Contact contact = db.Contacts.Find(id);
                if (contact == null)
                {
                    return RedirectToAction("Create", "Contacts");
                }
                return View(contact);
            }
            return RedirectToAction("Index", "Home");
        }

        // GET: Contacts/Create
        public ActionResult Create()
        {
            ViewBag.addressId = new SelectList(db.Addresses, "id", "addressType");
            ViewBag.altAddressId = new SelectList(db.Addresses, "id", "addressType");
            ViewBag.descriptionId = new SelectList(db.Descriptions, "id", "gender");
            ViewBag.networkerId = new SelectList(db.Networkers, "id", "firstName");
            return View();
        }

        // POST: Contacts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,image,email,prefix,givenName,familyName,phoneType,phoneNumber,organization,workTitle,altPhoneNumberType,altPhoneNumber,lastupdated,inContact,addressId,altAddressId,descriptionId,networkerId")] Contact contact)
        {
            if (ModelState.IsValid)
            {
                db.Contacts.Add(contact);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.addressId = new SelectList(db.Addresses, "id", "addressType", contact.addressId);
            ViewBag.altAddressId = new SelectList(db.Addresses, "id", "addressType", contact.altAddressId);
            ViewBag.descriptionId = new SelectList(db.Descriptions, "id", "gender", contact.descriptionId);
            ViewBag.networkerId = new SelectList(db.Networkers, "id", "firstName", contact.networkerId);
            return View(contact);
        }

        // GET: Contacts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contact contact = db.Contacts.Find(id);
            if (contact == null)
            {
                return HttpNotFound();
            }
            ViewBag.addressId = new SelectList(db.Addresses, "id", "addressType", contact.addressId);
            ViewBag.altAddressId = new SelectList(db.Addresses, "id", "addressType", contact.altAddressId);
            ViewBag.descriptionId = new SelectList(db.Descriptions, "id", "gender", contact.descriptionId);
            ViewBag.networkerId = new SelectList(db.Networkers, "id", "firstName", contact.networkerId);
            return View(contact);
        }

        // POST: Contacts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,image,email,prefix,givenName,familyName,phoneType,phoneNumber,organization,workTitle,altPhoneNumberType,altPhoneNumber,lastupdated,inContact,addressId,altAddressId,descriptionId,networkerId")] Contact contact)
        {
            if (ModelState.IsValid)
            {
                db.Entry(contact).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.addressId = new SelectList(db.Addresses, "id", "addressType", contact.addressId);
            ViewBag.altAddressId = new SelectList(db.Addresses, "id", "addressType", contact.altAddressId);
            ViewBag.descriptionId = new SelectList(db.Descriptions, "id", "gender", contact.descriptionId);
            ViewBag.networkerId = new SelectList(db.Networkers, "id", "firstName", contact.networkerId);
            return View(contact);
        }

        // GET: Contacts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contact contact = db.Contacts.Find(id);
            if (contact == null)
            {
                return HttpNotFound();
            }
            return View(contact);
        }

        // POST: Contacts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Contact contact = db.Contacts.Find(id);
            db.Contacts.Remove(contact);
            db.SaveChanges();
            return RedirectToAction("Index");
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
