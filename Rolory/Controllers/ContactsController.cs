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
    public class ContactsController : Controller
    {
        private ApplicationDbContext db;
        List<SelectListItem> stateList = new List<SelectListItem>();
        public void GetStateSelection()
        {
            stateList.Add(new SelectListItem() { Text = "Alabama", Value = "AL" });
            stateList.Add(new SelectListItem() { Text = "Alaska", Value = "AK" });
            stateList.Add(new SelectListItem() { Text = "Arizona", Value = "AZ" });
            stateList.Add(new SelectListItem() { Text = "Arkansas", Value = "AR" });
            stateList.Add(new SelectListItem() { Text = "California", Value = "CA" });
            stateList.Add(new SelectListItem() { Text = "Colorado", Value = "CO" });
            stateList.Add(new SelectListItem() { Text = "Connecticut", Value = "CT" });
            stateList.Add(new SelectListItem() { Text = "Delaware", Value = "DE" });
            stateList.Add(new SelectListItem() { Text = "Florida", Value = "FL" });
            stateList.Add(new SelectListItem() { Text = "Georgia", Value = "GA" });
            stateList.Add(new SelectListItem() { Text = "Hawaii", Value = "HI" });
            stateList.Add(new SelectListItem() { Text = "Idaho", Value = "ID" });
            stateList.Add(new SelectListItem() { Text = "Illinois", Value = "IL" });
            stateList.Add(new SelectListItem() { Text = "Indiana", Value = "IN" });
            stateList.Add(new SelectListItem() { Text = "Iowa", Value = "IA" });
            stateList.Add(new SelectListItem() { Text = "Kansas", Value = "KS" });
            stateList.Add(new SelectListItem() { Text = "Kentucky", Value = "KY" });
            stateList.Add(new SelectListItem() { Text = "Louisiana", Value = "LA" });
            stateList.Add(new SelectListItem() { Text = "Maine", Value = "ME" });
            stateList.Add(new SelectListItem() { Text = "Maryland", Value = "MD" });
            stateList.Add(new SelectListItem() { Text = "Massachusetts", Value = "MA" });
            stateList.Add(new SelectListItem() { Text = "Michigan", Value = "MI" });
            stateList.Add(new SelectListItem() { Text = "Minnesota", Value = "MN" });
            stateList.Add(new SelectListItem() { Text = "Connecticut", Value = "MS" });
            stateList.Add(new SelectListItem() { Text = "Missouri", Value = "MO" });
            stateList.Add(new SelectListItem() { Text = "Montana", Value = "MT" });
            stateList.Add(new SelectListItem() { Text = "Nebraska", Value = "NE" });
            stateList.Add(new SelectListItem() { Text = "Nevada", Value = "NV" });
            stateList.Add(new SelectListItem() { Text = "New Hampshire", Value = "NH" });
            stateList.Add(new SelectListItem() { Text = "New Jersey", Value = "NJ" });
            stateList.Add(new SelectListItem() { Text = "New York", Value = "NY" });
            stateList.Add(new SelectListItem() { Text = "New Mexico", Value = "NM" });
            stateList.Add(new SelectListItem() { Text = "North Carolina", Value = "NC" });
            stateList.Add(new SelectListItem() { Text = "North Dakota", Value = "ND" });
            stateList.Add(new SelectListItem() { Text = "Ohio", Value = "OH" });
            stateList.Add(new SelectListItem() { Text = "Oklahoma", Value = "OK" });
            stateList.Add(new SelectListItem() { Text = "Oregon", Value = "OR" });
            stateList.Add(new SelectListItem() { Text = "Pennsylvania", Value = "PA" });
            stateList.Add(new SelectListItem() { Text = "Rhode Island", Value = "RI" });
            stateList.Add(new SelectListItem() { Text = "South Carolina", Value = "SC" });
            stateList.Add(new SelectListItem() { Text = "South Dakota", Value = "SD" });
            stateList.Add(new SelectListItem() { Text = "Tennessee", Value = "TN" });
            stateList.Add(new SelectListItem() { Text = "Texas", Value = "TX" });
            stateList.Add(new SelectListItem() { Text = "Utah", Value = "UT" });
            stateList.Add(new SelectListItem() { Text = "Vermont", Value = "VT" });
            stateList.Add(new SelectListItem() { Text = "Virginia", Value = "VA" });
            stateList.Add(new SelectListItem() { Text = "Washington", Value = "WA" });
            stateList.Add(new SelectListItem() { Text = "West Virginia", Value = "WV" });
            stateList.Add(new SelectListItem() { Text = "Wisconsin", Value = "WI" });
            stateList.Add(new SelectListItem() { Text = "Wyoming", Value = "WY" });
        }
       
        public ContactsController()
        {
            db = new ApplicationDbContext();
            GetStateSelection();
        }
        // GET: Contacts
        public ActionResult Index(string sortOrder, string searchString)
        {
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
            string userId = User.Identity.GetUserId();
            var user = db.Networkers.Where(n => n.UserId == userId).Select(n => n).SingleOrDefault();
            var contacts = db.Contacts.Where(c=>c.NetworkerId == user.Id).Include(c => c.Address).Include(c => c.AlternateAddress).Include(c => c.Description).Include(c => c.Networker);
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
            ViewBag.States = stateList;
            ViewBag.AddressId = new SelectList(db.Addresses, "Id", "AddressType");
            ViewBag.AltAddressId = new SelectList(db.Addresses, "Id", "AddressType");
            ViewBag.DescriptionId = new SelectList(db.Descriptions, "Id", "Gender");
            ViewBag.NetworkerId = new SelectList(db.Networkers, "Id", "FirstName");
            return View();
        }

        // POST: Contacts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Image,Email,Prefix,GivenName,FamilyName,PhoneType,PhoneNumber,Organization,WorkTitle,AltPhoneNumberType,AltPhoneNumber,LastUpdated,InContact,AddressId,AltAddressId,DescriptionId,NetworkerId")] Contact contact)
        {
            if (ModelState.IsValid)
            {
                db.Contacts.Add(contact);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AddressId = new SelectList(db.Addresses, "Id", "AddressType", contact.AddressId);
            ViewBag.AltAddressId = new SelectList(db.Addresses, "Id", "AddressType", contact.AltAddressId);
            ViewBag.DescriptionId = new SelectList(db.Descriptions, "Id", "Gender", contact.DescriptionId);
            ViewBag.NetworkerId = new SelectList(db.Networkers, "Id", "FirstName", contact.NetworkerId);
            return View(contact);
        }

        // GET: Contacts/Edit/5
        public ActionResult Edit(int? id)
        {
            ViewBag.States = stateList;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contact contact = db.Contacts.Find(id);
            if (contact == null)
            {
                return HttpNotFound();
            }
            ViewBag.AddressId = new SelectList(db.Addresses, "Id", "AddressType", contact.AddressId);
            ViewBag.AltAddressId = new SelectList(db.Addresses, "Id", "AddressType", contact.AltAddressId);
            ViewBag.DescriptionId = new SelectList(db.Descriptions, "Id", "Gender", contact.DescriptionId);
            ViewBag.NetworkerId = new SelectList(db.Networkers, "Id", "FirstName", contact.NetworkerId);
            return View(contact);
        }

        // POST: Contacts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Image,Email,Prefix,GivenName,FamilyName,PhoneType,PhoneNumber,Organization,WorkTitle,AltPhoneNumberType,AltPhoneNumber,LastUpdated,InContact,AddressId,AltAddressId,DescriptionId,NetworkerId")] Contact contact)
        {
            if (ModelState.IsValid)
            {
                contact.LastUpdated = DateTime.Now;
                db.Entry(contact).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AddressId = new SelectList(db.Addresses, "id", "AddressType", contact.AddressId);
            ViewBag.AltAddressId = new SelectList(db.Addresses, "id", "AddressType", contact.AltAddressId);
            ViewBag.DescriptionId = new SelectList(db.Descriptions, "Id", "Gender", contact.DescriptionId);
            ViewBag.NetworkerId = new SelectList(db.Networkers, "Id", "FirstName", contact.NetworkerId);
            return View(contact);
        }
        [HttpGet]
        public ActionResult About(int? id)
        {
            if (ModelState.IsValid)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                ContactDescriptionViewModel contactDescription = new ContactDescriptionViewModel();
                contactDescription.Contact = db.Contacts.Where(c => c.Id == id).Select(c => c).SingleOrDefault();
                contactDescription.Description = db.Descriptions.Where(d => d.Id == contactDescription.Contact.DescriptionId).Select(d => d).SingleOrDefault();
                var currentId = id;
                if (contactDescription.Description == null)
                {
                    return RedirectToAction("Details", "Contacts", new { id = currentId });
                }
                if (contactDescription == null)
                {
                    return RedirectToAction("Create", "Contacts");
                }
                return View(contactDescription);
            }
            return RedirectToAction("Index", "Home");
        }
        public ActionResult Build([Bind(Include = "Id,Image,Email,Prefix,GivenName,FamilyName,PhoneType,PhoneNumber,Organization,WorkTitle,AltPhoneNumberType,AltPhoneNumber,LastUpdated,InContact,AddressId,AltAddressId,DescriptionId,NetworkerId")] Contact contact)
        {
            ContactDescriptionViewModel contactDescription = new ContactDescriptionViewModel();
            contactDescription.Contact = contact;
            return View(contactDescription);
        }
        [HttpPost]
        public ActionResult Build(Contact contact, Description description)
        {
            if (ModelState.IsValid)
            {
                contact.DescriptionId = description.Id;
                db.Contacts.Add(contact);
                db.SaveChanges();
                db.Descriptions.Add(description);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View();
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
