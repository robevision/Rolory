using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
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
        private ContactsManagement cm;
        private WeatherManagement wm;
        public ContactsController()
        {
            db = new ApplicationDbContext();
            cm = new ContactsManagement();
            wm = new WeatherManagement();
        }
      
        // GET: Contacts
        public ActionResult Index(string sortOrder, string searchString)
        {
            ViewBag.FirstNameSortParm = sortOrder == "givenName" ? "givenName_desc" : "givenName";
            ViewBag.LastNameSortParm = String.IsNullOrEmpty(sortOrder) ? "familyName_desc" : "";
            ViewBag.DateSortParm = sortOrder == "date" ? "date_desc" : "date";
            ViewBag.InContactParm = sortOrder == "inTouch" ? "inTouch_desc" : "inTouch";
            string userId = User.Identity.GetUserId();
            var networker = db.Networkers.Where(n => n.UserId == userId).Select(n => n).SingleOrDefault();
            var networkerNullCheck = db.Networkers.Where(n => n.UserId == userId).Any();
            List <DateTime> interactionMoments = new List<DateTime>();
            if (networkerNullCheck == false)
            {
                return RedirectToAction("CreateAccount", "User");
            }
            List<Contact> contactsNullCheck = db.Contacts.Where(c => c.NetworkerId == networker.Id).ToList();
            try
            {
                if (contactsNullCheck[0] == null || contactsNullCheck.Count() == 0)
                {
                    //Add a page to send the logged in user to a message that says they have no contacts logged
                    return RedirectToAction("Null", "Contacts");
                }
            }
            catch
            {
                return RedirectToAction("Null", "Contacts");
            }
            ViewBag.ContactCount = GetAmountOfContacts(networker.Id);
            ViewBag.ContactAmount = ConcatContactAmount(ViewBag.ContactCount);
            var contacts = db.Contacts.Where(c=>c.NetworkerId == networker.Id).Include(c => c.Address).Include(c => c.AlternateAddress).Include(c => c.Description).Include(c => c.Networker);
            //foreach(Contact contact in contacts)
            //{
            //    DateTime interactionMoment = db.Interactions.Where(i => i.ContactId == contact.Id).OrderByDescending(i => i.Moment).Select(i => i.Moment).FirstOrDefault();
            //    interactionMoments.Add(interactionMoment);
            //}
            contacts = ChooseSortOrder(sortOrder, networker, contacts, interactionMoments);

            if (!String.IsNullOrEmpty(searchString))
            {
                contacts = db.Contacts.Where(c => c.NetworkerId == networker.Id).Where(c => c.FamilyName.Contains(searchString));
            }

            ViewBag.InteractionMoments = interactionMoments;
            ViewBag.InTouch = null;
            ViewBag.True = "True";
            ViewBag.False = "False";
            return View(contacts.ToList());
        }

        // GET: Contacts/Details/5
        public ActionResult Details(int? id)
        {
            var lastWeek = DateTime.Today.AddDays(-6);
            var nullDateTime = new DateTime();
            if (ModelState.IsValid)
            {
                var moment = cm.FindLastTimeInTouch(id, db);
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                var interactions = db.Interactions.Where(i => i.ContactId == id).Select(i => i);
                var interactionMomentsBool = interactions.Select(i => i.Moment >= lastWeek).Any();
                DateTime interactionMoment = interactionMoment = db.Interactions.Where(i => i.ContactId == id).OrderByDescending(i => i.Moment).Select(i => i.Moment).FirstOrDefault();
                if (interactionMomentsBool == true)
                {
                    if (interactionMoment.Date == nullDateTime)
                    {
                        interactionMoment = db.Interactions.Where(i => i.ContactId == id).OrderByDescending(i => i.Moment).Select(i => i.Moment).FirstOrDefault();
                    }
                    else
                    {
                        var dayAmount = DateTime.Today - interactionMoment.Date;
                        //ViewBag.Moment = dayAmount.Days.ToString();
                    }
                    ViewBag.Moment = moment;
            }
                Contact contact = db.Contacts.Find(id);
                if (contact == null)
                {
                    return RedirectToAction("Create", "Contacts");
                }
                contact.Description = db.Descriptions.Where(d => d.Id == contact.DescriptionId).Select(d => d).SingleOrDefault();
                contact.Address = db.Addresses.Where(a => a.Id == contact.AddressId).Select(a => a).SingleOrDefault();

                if(contact.AltAddressId != null)
                {
                    contact.AlternateAddress = db.Contacts.Where(c => c.AltAddressId == contact.AltAddressId).Select(c => c.AlternateAddress).SingleOrDefault();
                }
                string inTouch = Convert.ToString(contact.InContact).ToLower();
                switch (inTouch)
                {
                    case "true":
                        ViewBag.InTouch = "Yes";
                        return View(contact);
                    case "false":
                        ViewBag.InTouch = "No";
                        return View(contact);
                    default:
                        return View(contact);
                }
            }
            return RedirectToAction("Index", "Home");
        }

        // GET: Contacts/Create
        public ActionResult Create()
        {
            ViewBag.Prefix = cm.prefixList;
            ViewBag.States = cm.stateList;
            ViewBag.Types = cm.typeList;
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
        public ActionResult Create([Bind(Include = "Id,ImagePath,Email,Prefix,GivenName,FamilyName,PhoneType,PhoneNumber,Organization,WorkTitle,AltPhoneNumberType,AltPhoneNumber,LastUpdated,InContact,AddressId,AltAddressId,DescriptionId,NetworkerId")] Contact contact)
        {
            if (ModelState.IsValid)
            {
              
                var image = new Image();
                image.ImageFile = Request.Files["ImageFile"];
                if (image.ImageFile != null)
                {

                    string fileName = Path.GetFileNameWithoutExtension(image.ImageFile.FileName);
                    string extension = Path.GetExtension(image.ImageFile.FileName);
                    fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                    contact.ImagePath = "~/UserImages/" + fileName;
                    fileName = Path.Combine(Server.MapPath("~/UserImages/"), fileName);
                    image.ImageFile.SaveAs(fileName);
                }
                string userId = User.Identity.GetUserId();
                var networker = db.Networkers.Where(n => n.UserId == userId).SingleOrDefault();
                contact.NetworkerId = networker.Id;
                contact.LastUpdated = DateTime.Now;
                contact.PhoneType = Request.Form["Phone Type"].ToString();
                contact.AltPhoneNumberType = Request.Form["Alternate Phone Type"].ToString();
                var PhoneType = db.Contacts.Where(c => c.Id == contact.Id).Select(c => c.PhoneType).SingleOrDefault();
                PhoneType = contact.PhoneType;
                contact.PhoneNumber = cm.PopulatePhoneNumber(contact.PhoneNumber);
                contact.AltPhoneNumber = cm.PopulatePhoneNumber(contact.AltPhoneNumber);
                var altPhoneType = db.Contacts.Where(c => c.Id == contact.Id).Select(c => c.AltPhoneNumberType).SingleOrDefault();
                altPhoneType = contact.AltPhoneNumberType;
                Description description = new Description();
                contact.DescriptionId = description.Id;
                contact.Description = description;
                db.Descriptions.Add(description);
                Address address = new Address();
                contact.AddressId = address.Id;
                contact.Address = address;
                db.Addresses.Add(address);
                db.SaveChanges();
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
            ViewBag.States = cm.stateList;
            ViewBag.Types = cm.typeList;
            ViewBag.Prefix = cm.prefixList;
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
                var inContact = db.Contacts.Where(c => c.Id == contact.Id).Select(c => c.InContact).SingleOrDefault();
                inContact = contact.InContact;

                var phoneType = db.Contacts.Where(c => c.Id == contact.Id).Select(c => c.PhoneType).SingleOrDefault();
                phoneType = contact.PhoneType;
                
                if (cm.ReturnOnlyIntegers(contact.PhoneNumber) != cm.ReturnOnlyIntegers(db.Contacts.Where(c => c.Id == contact.Id).Select(c => c.PhoneNumber).SingleOrDefault().ToString()))
                {
                    contact.PhoneNumber = cm.PopulatePhoneNumber(contact.PhoneNumber);
                }
                if (cm.ReturnOnlyIntegers(contact.AltPhoneNumber) != cm.ReturnOnlyIntegers(db.Contacts.Where(c => c.Id == contact.Id).Select(c => c.AltPhoneNumber).SingleOrDefault().ToString()))
                {
                    contact.AltPhoneNumber = cm.PopulatePhoneNumber(contact.AltPhoneNumber);
                }
                var altPhoneType = db.Contacts.Where(c => c.Id == contact.Id).Select(c => c.AltPhoneNumberType).SingleOrDefault();
                altPhoneType = contact.AltPhoneNumberType;
                contact.Description = db.Descriptions.Where(d => d.Id == contact.DescriptionId).Select(d => d).SingleOrDefault();
                db.Entry(contact.Description).State = EntityState.Modified;
                //contact.Description.Id = db.Contacts.Where(c => c.Id == contact.Id).Select(c => c.DescriptionId.Value).SingleOrDefault();
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
                contactDescription.Contact.Address = db.Addresses.Where(a => a.Id == contactDescription.Contact.AddressId).Select(a => a).SingleOrDefault();
                var nullDateTime = new DateTime();
                var currentId = id;
                ViewBag.Temperature = wm.GetInitialWeatherStream(currentId);
                ViewBag.Moment = cm.FindLastTimeInTouch(id, db);
                var recent = DateTime.Today.AddMonths(-3);
                var description = contactDescription.Description;
                if (description.Relationship != null)
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
                var moment = db.Interactions.Where(i => i.ContactId == contactDescription.Contact.Id).OrderByDescending(i=>i.Moment).Select(i=>i.Moment).FirstOrDefault();
                if (moment <= recent)
                {
                    contactDescription.Contact.InContact = false;
                    if (moment != nullDateTime)
                    {
                        contactDescription.Contact.InContactCountDown = moment;
                    }
            
                    db.SaveChanges();
                }
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
        [HttpGet]
        public ActionResult Build(int? id)
        {
            ViewBag.Gender = cm.genderList;
            ViewBag.Category = cm.categoryList;
            ViewBag.Relationship = cm.relationshipList;
            ViewBag.Prefix = cm.prefixList;
            var contact = db.Contacts.Where(c => c.Id == id).Select(c => c).SingleOrDefault();
            contact.Description = db.Descriptions.Where(d => d.Id == contact.DescriptionId).Select(d => d).SingleOrDefault();
            contact.Description.Id = db.Descriptions.Where(d => d.Id == contact.DescriptionId).Select(d=>d.Id).SingleOrDefault();
            contact.Address = db.Addresses.Where(a => a.Id == contact.AddressId).Select(a => a).SingleOrDefault();
            contact.Address.Id = db.Addresses.Where(a => a.Id == contact.AddressId).Select(a => a.Id).SingleOrDefault();
            if (contact == null)
            {
                return RedirectToAction("Error", "Contacts");
            }
            if(contact.Description.Gender != null && contact.Description.Category != null && contact.Description.Relationship != null && contact.Description.BirthDate != null)
            {
                if (contact.Address != null && contact.Address.AddressType != null && contact.Address.StreetAddress != null && contact.Address.Locality != null && contact.Address.Region != null && contact.Address.CountryName != null)
                {
                    return RedirectToAction("Expand", "Contacts", new { passedId = id });
                }
               
                return RedirectToAction("BuildAddress", "Contacts", new {passedId = id });
              
            }
            return View(contact);
        }
        [HttpPost]
        public ActionResult Build(Contact contact)
        {
            contact.Description.Id = contact.DescriptionId;
            db.SaveChanges();
            if (ModelState.IsValid)
            {
                contact.DescriptionId = db.Contacts.Where(c => c.Id == contact.Id).Select(c => c.Description.Id).SingleOrDefault();
                Contact contactReference = db.Contacts.Where(c => c.Id == contact.Id).Select(c => c).SingleOrDefault();
                contactReference.Description = db.Descriptions.Where(d => d.Id == contact.Description.Id).Select(d => d).SingleOrDefault();
                contactReference.Description.Gender = contact.Description.Gender;
                contactReference.Description.Relationship = contact.Description.Relationship;
                contactReference.Description.Category = contact.Description.Category;
                contactReference.Description.BirthDate = contact.Description.BirthDate;
                //if this doesn't work, make description be declared as a variable and write an entityState.modified for that table as well.
                db.Entry(contactReference).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", "Contacts", new { id = contact.Id });
            }

            return RedirectToAction("Error", "Contacts");
        }
        [HttpGet]
        public ActionResult BuildAddress(int? passedId)
        {
            if (passedId != null)
            {
                ViewBag.Gender = cm.genderList;
                ViewBag.Category = cm.categoryList;
                ViewBag.Relationship = cm.relationshipList;
                ViewBag.States = cm.stateList;
                ViewBag.Types = cm.typeList;

                Contact contact = db.Contacts.Where(c => c.Id == passedId).Select(c => c).SingleOrDefault();
                int addressId = db.Addresses.Where(a => a.Id == contact.AddressId).Select(a => a.Id).SingleOrDefault();
                contact.Address = db.Addresses.Where(a => a.Id == contact.AddressId).Select(a => a).SingleOrDefault();

                return View(contact.Address);
            }
            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        public ActionResult BuildAddress(Address address)
        {
            Contact contact = db.Contacts.Where(c => c.AddressId == address.Id).Select(c => c).SingleOrDefault();
            //if(address.Id != 0)
            //{
            //    contact = db.Contacts.Where(c => c.Id == contact.Id).Select(c => c).SingleOrDefault();
            //    contact.Address.Id = contact.AddressId;
            //    contact.Address = db.Addresses.Where(a => a.Id == contact.AddressId).Select(a => a).SingleOrDefault();
            //    db.SaveChanges();
            //}
            //contact.Address.Id = contact.AddressId;
            //contact.Address = db.Addresses.Where(a => a.Id == contact.AddressId).Select(a => a).SingleOrDefault();
            //db.SaveChanges();
            if (ModelState.IsValid)
            {
                
                Address addressInDb = db.Addresses.Where(c => c.Id == address.Id).FirstOrDefault();
                contact.LastUpdated = DateTime.Now;

                if (contact.Address != null)
                {
                    wm.SetLatLong(address);
                    contact.Address = address;
                }
     
                db.Entry(contact).State = EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("Details", "Contacts", new {id = contact.Id});

        }
        [HttpGet]
        public ActionResult Expand(int? passedId)
        {
            if (passedId != null)
            {

                Contact contact = db.Contacts.Where(c => c.Id == passedId).Select(c => c).SingleOrDefault();
                ViewBag.Gender = cm.genderList;
                ViewBag.Category = cm.categoryList;
                ViewBag.Relationship = cm.relationshipList;
                ViewBag.States = cm.stateList;
                ViewBag.Types = cm.typeList;
                ViewBag.Prefix = cm.prefixList;
                contact.Description = db.Descriptions.Where(d => d.Id == contact.DescriptionId).Select(d => d).SingleOrDefault();
                contact.Address = db.Addresses.Where(a => a.Id == contact.AddressId).Select(a => a).SingleOrDefault();
                return View(contact);
               
            }

            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        public ActionResult Expand(Contact contact, string save = null)
        {
            if (ModelState.IsValid)
            {
                //    var description = contact.Description;
                //    db.Entry(description).State = EntityState.Modified;
                //db.SaveChanges();
                Contact contactInDB = db.Contacts.Where(c => c.Id == contact.Id).FirstOrDefault();
                if(contact.Address != null)
                {
                    wm.SetLatLong(contact.Address);
                }

                if (String.IsNullOrEmpty(contact.Prefix) != true)
                {
                    contactInDB.Prefix = contact.Prefix;
                }
                if(String.IsNullOrEmpty(contact.GivenName) != true)
                {
                    contactInDB.GivenName = contact.GivenName;
                }
                if(String.IsNullOrEmpty(contact.FamilyName) != true)
                {
                    contactInDB.FamilyName = contact.FamilyName;
                }
                if(String.IsNullOrEmpty(contact.Email) != true)
                {
                    contactInDB.Email = contact.Email;
                }
                if(String.IsNullOrEmpty(contact.PhoneType) != true)
                {
                    contactInDB.PhoneType = contact.PhoneType;
                }
                if (String.IsNullOrEmpty(contact.PhoneNumber) != true)
                {
                    contactInDB.PhoneNumber = cm.PopulatePhoneNumber(contact.PhoneNumber);
                    
                }
                if (String.IsNullOrEmpty(contact.AltPhoneNumber) != true)
                 contactInDB.AltPhoneNumber = cm.PopulatePhoneNumber(contact.AltPhoneNumber);
                  
                    if (String.IsNullOrEmpty(Convert.ToString(contact.Description.Anniversary)) != true)
                {
                    contactInDB.Description.Anniversary = contact.Description.Anniversary;
                }
                if (String.IsNullOrEmpty(Convert.ToString(contact.Description.BirthDate)) != true)
                {
                    contactInDB.Description.BirthDate = contact.Description.BirthDate;
                }
                contactInDB.InContact = contact.InContact;
                contactInDB.Perpetual = contact.Perpetual;
                //if(String.IsNullOrEmpty(contact.Description.Gender) != true)
                //{
                //    contactInDB.Description.Gender = contact.Description.Gender;
                //}
                //if(String.IsNullOrEmpty(contact.Description.Category) != true)
                //{
                //    contactInDB.Description.Category = contact.Description.Category;
                //}
                //if(String.IsNullOrEmpty(contact.Description.Relationship) != true)
                //{
                //    contactInDB.Description.Relationship = contact.Description.Relationship;
                //}
                //if(String.IsNullOrEmpty(contact.Description.Notes) != true)
                //{
                //    contactInDB.Description.Notes = contact.Description.Notes;
                //}
                //contactInDB.DescriptionId = contact.DescriptionId;
                //contactInDB.Description = contact.Description;
                //contactInDB.Description.Id = contact.Description.Id;
                //contactInDB.AddressId = contact.AddressId.Value;
                if(contact.Address != null)
                {
                    contactInDB.Address = contact.Address;
                }

                //contactInDB.DescriptionId = db.Contacts.Where(c => c.Id == contact.Id).Select(c=>c.DescriptionId).SingleOrDefault();
                //contactInDB.AddressId = db.Contacts.Where(c => c.Id == contact.Id).Select(c => c.AddressId).SingleOrDefault();
                db.Entry(contactInDB).State = EntityState.Modified;
                db.SaveChanges();

                //****use AJAX instead

                //if(save == "yes")
                //{
                //    return RedirectToAction("Details", "Contacts", new { id = contact.Id });
                //}
                return RedirectToAction("Details", "Contacts", new { id = contact.Id });
            }
            return RedirectToAction("Error", "Contacts");
        }
        public ActionResult Null()
        {
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
        private IQueryable<Contact> ChooseSortOrder(string sortOrder, Networker networker, IQueryable <Contact> contacts, List <DateTime> interactionMoments)
        {
            switch (sortOrder)
            {
                case "givenName_desc":
                    contacts = db.Contacts.Where(c => c.NetworkerId == networker.Id).OrderByDescending(c => c.GivenName);
                    break;
                case "givenName":
                    contacts = db.Contacts.Where(c => c.NetworkerId == networker.Id).OrderBy(c => c.GivenName);
                    break;
                case "familyName_desc":
                    contacts = db.Contacts.Where(c => c.NetworkerId == networker.Id).OrderByDescending(c => c.FamilyName);
                    break;
                case "date":
                    interactionMoments = interactionMoments.OrderBy(i => i.Date).ToList();
                    contacts = db.Contacts.Where(c => c.NetworkerId == networker.Id).OrderBy(c => c.LastUpdated);
                    break;
                case "date_desc":
                    interactionMoments = interactionMoments.OrderByDescending(i => i.Date).ToList();
                    contacts = db.Contacts.Where(c => c.NetworkerId == networker.Id).OrderByDescending(c => c.LastUpdated);
                    break;
                case "inTouch":
                    contacts = db.Contacts.Where(c => c.NetworkerId == networker.Id).OrderBy(c => c.InContact);
                    break;
                case "inTouch_desc":
                    contacts = db.Contacts.Where(c => c.NetworkerId == networker.Id).OrderByDescending(c => c.InContact);
                    break;
                default:
                    contacts = db.Contacts.Where(c => c.NetworkerId == networker.Id).OrderBy(c => c.FamilyName);
                    break;
            }
            return contacts;
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
        [HttpGet]
        public ActionResult Error()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        private int GetAmountOfContacts(int networkerid)
        {
          List<Contact> amountofContacts = new List<Contact>();
          amountofContacts =  db.Contacts.Where(c => c.NetworkerId == networkerid).ToList();
        
            return amountofContacts.Count();
        }
        private string ConcatContactAmount(int count)
        {
            string result;
            if (count == 0)
            {
                result = "You currently have no contacts added.";
            }
            if (count == 1)
            {
                result = count + " Contact";
            }
            else
            {
                result = count + " Contacts";
            }
            return result;
        }
    }
}
