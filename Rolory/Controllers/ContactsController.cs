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
        public List<SelectListItem> stateList = new List<SelectListItem>();
        public List<SelectListItem> typeList = new List<SelectListItem>();
        public List<SelectListItem> genderList = new List<SelectListItem>();
        public List<SelectListItem> relationshipList = new List<SelectListItem>();
        public List<SelectListItem> categoryList = new List<SelectListItem>();
        public List<SelectListItem> prefixList = new List<SelectListItem>();

        public ContactsController()
        {
            db = new ApplicationDbContext();
            GetStateSelection();
            GetTypeSelection();
            GetGenderSelection();
            GetRelationshipSelection();
            GetCategorySelection();
            GetPrefixSelection();
        }
        private void GetRelationshipSelection()
        {
            relationshipList.Add(new SelectListItem() { Text = "Distant", Value = "Distant" });
            relationshipList.Add(new SelectListItem() { Text = "Familiar", Value = "Familiar" });
            relationshipList.Add(new SelectListItem() { Text = "Friend", Value = "Friend" });
            relationshipList.Add(new SelectListItem() { Text = "Close", Value = "Close" });
            relationshipList.Add(new SelectListItem() { Text = "Business Superior", Value = "Business Superior" });
            relationshipList.Add(new SelectListItem() { Text = "Business Equal", Value = "Business Equal" });
            relationshipList.Add(new SelectListItem() { Text = "Teacher", Value = "Teacher" });
            relationshipList.Add(new SelectListItem() { Text = "Classmate", Value = "Classmate" });
            
            
        }
        private void GetCategorySelection()
        {
            categoryList.Add(new SelectListItem() { Text = "School", Value = "School" });
            categoryList.Add(new SelectListItem() { Text = "Family", Value = "Family" });
            categoryList.Add(new SelectListItem() { Text = "Social", Value = "Social" });
            categoryList.Add(new SelectListItem() { Text = "Work", Value = "Work" });
            //Be able to add custom category in the future
            categoryList.Add(new SelectListItem() { Text = "Other", Value = "Other" });
        }
        private void GetGenderSelection()
        {
            genderList.Add(new SelectListItem() { Text = "Male", Value = "Male" });
            genderList.Add(new SelectListItem() { Text = "Female", Value = "Female" });
        }
        private void GetTypeSelection()
        {
            typeList.Add(new SelectListItem() { Text = "Home", Value = "Home"});
            typeList.Add(new SelectListItem() { Text = "Work", Value = "Work" });
            typeList.Add(new SelectListItem() { Text = "Other", Value = "Other" });
        }
        private void GetStateSelection()
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
        private void GetPrefixSelection()
        {
            prefixList.Add(new SelectListItem() { Text = "Dr.", Value = "Dr." });
            prefixList.Add(new SelectListItem() { Text = "Mr.", Value = "Mr." });
            prefixList.Add(new SelectListItem() { Text = "Mrs.", Value = "Mrs." });
            prefixList.Add(new SelectListItem() { Text = "Ms.", Value = "Ms." });
            prefixList.Add(new SelectListItem() { Text = "Miss", Value = "Miss" });
            prefixList.Add(new SelectListItem() { Text = "Master", Value = "Master" });
        }

        // GET: Contacts
        public ActionResult Index(string sortOrder, string searchString)
        {
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
            string userId = User.Identity.GetUserId();
            var networker = db.Networkers.Where(n => n.UserId == userId).Select(n => n).SingleOrDefault();
            var networkerNullCheck = db.Networkers.Where(n => n.UserId == userId).Any();
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
            var contacts = db.Contacts.Where(c=>c.NetworkerId == networker.Id).Include(c => c.Address).Include(c => c.AlternateAddress).Include(c => c.Description).Include(c => c.Networker);
            
            switch (sortOrder)
            {
                case "name_desc":
                    contacts = db.Contacts.Where(c => c.NetworkerId == networker.Id).OrderByDescending(c => c.FamilyName);
                    break;
                case "Date":
                    contacts = db.Contacts.Where(c => c.NetworkerId == networker.Id).OrderBy(c => c.LastUpdated);
                    break;
                case "date_desc":
                    contacts = db.Contacts.Where(c => c.NetworkerId == networker.Id).OrderByDescending(c => c.LastUpdated);
                    break;
                default:
                    contacts = db.Contacts.Where(c => c.NetworkerId == networker.Id).OrderBy(c => c.FamilyName);
                    break;
            }
            if (!String.IsNullOrEmpty(searchString))
            {
                contacts = db.Contacts.Where(c => c.NetworkerId == networker.Id).Where(c => c.FamilyName.Contains(searchString));
            }

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
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                var interactions = db.Interactions.Where(i => i.ContactId == id).Select(i => i);
                var interactionMomentsBool = interactions.Select(i => i.Moment >= lastWeek).Any();
                DateTime interactionMoment = interactionMoment = db.Interactions.Where(i => i.ContactId == id).OrderByDescending(i => i.Moment).Select(i => i.Moment).FirstOrDefault();
                if (interactionMomentsBool == true)
                {
                    if(interactionMoment.Date == nullDateTime)
                    {
                        interactionMoment = db.Interactions.Where(i => i.ContactId == id).OrderByDescending(i => i.Moment).Select(i => i.Moment).FirstOrDefault();
                    }
                    else
                    {
                        var dayAmount = DateTime.Today - interactionMoment.Date;
                        ViewBag.Moment = dayAmount.Days.ToString();
                    }

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
            ViewBag.Prefix = prefixList;
            ViewBag.States = stateList;
            ViewBag.Types = typeList;
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
                string userId = User.Identity.GetUserId();
                var networker = db.Networkers.Where(n => n.UserId == userId).SingleOrDefault();
                contact.NetworkerId = networker.Id;
                contact.LastUpdated = DateTime.Now;
                contact.PhoneType = Request.Form["Phone Type"].ToString();
                contact.AltPhoneNumberType = Request.Form["Alternate Phone Type"].ToString();
                var PhoneType = db.Contacts.Where(c => c.Id == contact.Id).Select(c => c.PhoneType).SingleOrDefault();
                PhoneType = contact.PhoneType;
                if (String.IsNullOrEmpty(contact.PhoneNumber) != true)
                {
                    List<string> phoneNumberResult = new List<string>();
                    List<char> areaCode = new List<char>();
                    List<char> body = new List<char>();
                    var phoneNumber = contact.PhoneNumber;
                    for (int i = 0; i < phoneNumber.Count(); i++)
                    {
                        if (Char.IsNumber(phoneNumber[i]) == true)
                        {
                            if (i == 0)
                            {
                                areaCode.Add(Convert.ToChar("("));
                                areaCode.Add(phoneNumber[i]);
                            }
                            else if (i == 1)
                            {
                                areaCode.Add(phoneNumber[i]);
                            }
                            else if (i == 2)
                            {
                                areaCode.Add(phoneNumber[i]);
                                areaCode.Add(Convert.ToChar(")"));
                                areaCode.Add(Convert.ToChar(" "));
                            }
                            else if (i > 2)
                            {
                                if (body.LongCount() == 3)
                                {
                                    body.Add(Convert.ToChar("-"));
                                }
                                if (body.LongCount() < 8)
                                {
                                    body.Add(phoneNumber[i]);
                                }

                            }

                        }
                    }
                    foreach (char index in areaCode)
                    {
                        phoneNumberResult.Add(Convert.ToString(index));
                    }
                    foreach (char index in body)
                    {
                        phoneNumberResult.Add(Convert.ToString(index));
                    }
                    contact.PhoneNumber = String.Join("", phoneNumberResult.ToArray());
                }
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
            ViewBag.States = stateList;
            ViewBag.Types = typeList;
            ViewBag.Prefix = prefixList;
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
                if (String.IsNullOrEmpty(contact.PhoneNumber) != true)
                {
                    List<string> phoneNumberResult = new List<string>();
                    List<char> areaCode = new List<char>();
                    List<char> body = new List<char>();
                    var phoneNumber = contact.PhoneNumber;
                    for (int i = 0; i < phoneNumber.Count(); i++)
                    {
                        if (Char.IsNumber(phoneNumber[i]) == true)
                        {
                            if (i == 0)
                            {
                                areaCode.Add(Convert.ToChar("("));
                                areaCode.Add(phoneNumber[i]);
                            }
                            else if (i == 1)
                            {
                                areaCode.Add(phoneNumber[i]);
                            }
                            else if (i == 2)
                            {
                                areaCode.Add(phoneNumber[i]);
                                areaCode.Add(Convert.ToChar(")"));
                                areaCode.Add(Convert.ToChar(" "));
                            }
                            else if (i > 2)
                            {
                                if (body.LongCount() == 3)
                                {
                                    body.Add(Convert.ToChar("-"));
                                }
                                if (body.LongCount() < 8)
                                {
                                    body.Add(phoneNumber[i]);
                                }

                            }

                        }
                    }
                    foreach (char index in areaCode)
                    {
                        phoneNumberResult.Add(Convert.ToString(index));
                    }
                    foreach (char index in body)
                    {
                        phoneNumberResult.Add(Convert.ToString(index));
                    }
                    contact.PhoneNumber = String.Join("", phoneNumberResult.ToArray());
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
        [HttpGet]
        public ActionResult Build(int? id)
        {
            ViewBag.Gender = genderList;
            ViewBag.Category = categoryList;
            ViewBag.Relationship = relationshipList;
            ViewBag.Prefix = prefixList;
            var contact = db.Contacts.Where(c => c.Id == id).Select(c => c).SingleOrDefault();
            contact.Description = db.Descriptions.Where(d => d.Id == contact.DescriptionId).Select(d => d).SingleOrDefault();
            contact.Description.Id = db.Descriptions.Where(d => d.Id == contact.DescriptionId).Select(d=>d.Id).SingleOrDefault();
            if (contact == null)
            {
                return RedirectToAction("Index", "Home");
            }
            if(contact.Description.Gender != null && contact.Description.Category != null && contact.Description.Relationship != null && contact.Description.BirthDate != null)
            {
                return RedirectToAction("Expand", "Contacts", new { passedId = id });
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
        public ActionResult BuildAddress(int? id)
        {
            ViewBag.Gender = genderList;
            ViewBag.Category = categoryList;
            ViewBag.Relationship = relationshipList;
            ViewBag.States = stateList;
            ViewBag.Types = typeList;

            var contact = db.Contacts.Where(c => c.Id == id).Select(c => c).SingleOrDefault();
            return View(contact);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult BuildAddress([Bind(Include = "Id,Image,Email,Prefix,GivenName,FamilyName,PhoneType,PhoneNumber,Organization,WorkTitle,AltPhoneNumberType,AltPhoneNumber,LastUpdated,InContact,AddressId,AltAddressId,DescriptionId,NetworkerId")] Contact contact)
        {
            if (ModelState.IsValid)
            {
                string userId = User.Identity.GetUserId();
                var networker = db.Networkers.Where(n => n.UserId == userId).SingleOrDefault();
                contact.NetworkerId = networker.Id;
                contact.LastUpdated = DateTime.Now;
                db.Addresses.Add(contact.Address);
                db.SaveChanges();
                db.Entry(contact).State = EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("Index", "Home");

        }
        [HttpGet]
        public ActionResult Expand(int? passedId)
        {
            if (passedId != null)
            {

                Contact contact = db.Contacts.Where(c => c.Id == passedId).Select(c => c).SingleOrDefault();
                ViewBag.Gender = genderList;
                ViewBag.Category = categoryList;
                ViewBag.Relationship = relationshipList;
                ViewBag.States = stateList;
                ViewBag.Types = typeList;
                ViewBag.Prefix = prefixList;
                contact.Description = db.Descriptions.Where(d => d.Id == contact.DescriptionId).Select(d => d).SingleOrDefault();
                contact.Address = db.Addresses.Where(a => a.Id == contact.AddressId).Select(a => a).SingleOrDefault();
                return View(contact);
               
            }

            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        public ActionResult Expand(Contact contact)
        {
            contact.Description.Id = contact.DescriptionId;
            contact.Description = db.Descriptions.Where(d => d.Id == contact.DescriptionId).Select(d => d).SingleOrDefault();
            db.SaveChanges();
            if (ModelState.IsValid)
            {
                //    var description = contact.Description;
                //    db.Entry(description).State = EntityState.Modified;
                //db.SaveChanges();
                Contact contactInDB = db.Contacts.Where(c => c.Id == contact.Id).FirstOrDefault();
                //WeatherManagement weath = new WeatherManagement();
                //weath.SetLatLong(contact.Address);  
                if(String.IsNullOrEmpty(contact.Prefix) != true)
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
                if(String.IsNullOrEmpty(contact.PhoneNumber) != true)
                {
                    List <string> phoneNumberResult = new List <string>();
                    List <char> areaCode = new List<char>();
                    List<char> body = new List<char>();
                    var phoneNumber = contact.PhoneNumber;
                    for (int i = 0; i < phoneNumber.Count(); i++)
                    {
                        if(Char.IsNumber(phoneNumber[i]) == true)
                        {
                            if (i == 0)
                            {
                                areaCode.Add(Convert.ToChar("("));
                                areaCode.Add(phoneNumber[i]);
                            }
                            else if (i == 1)
                            {
                                areaCode.Add(phoneNumber[i]);
                            }
                            else if (i == 2)
                            {
                                areaCode.Add(phoneNumber[i]);
                                areaCode.Add(Convert.ToChar(")"));
                                areaCode.Add(Convert.ToChar(" "));
                            }
                            else if (i > 2)
                            {
                                if (body.LongCount() == 3)
                                {
                                    body.Add(Convert.ToChar("-"));
                                }
                                if (body.LongCount() < 8)
                                {
                                    body.Add(phoneNumber[i]);
                                }

                            }

                        }
                    }
                foreach(char index in areaCode)
                    {
                        phoneNumberResult.Add(Convert.ToString(index));
                    }
                foreach(char index in body)
                    {
                        phoneNumberResult.Add(Convert.ToString(index));
                    }
              contactInDB.PhoneNumber = String.Join("", phoneNumberResult.ToArray());
                }
                contactInDB.DescriptionId = contact.DescriptionId;
                contactInDB.Description = contact.Description;
                contactInDB.Description.Id = contact.Description.Id;
                //contactInDB.AddressId = contact.AddressId.Value;
                if(contact.Address != null)
                {
                    contactInDB.Address = contact.Address;
                }

                //contactInDB.DescriptionId = db.Contacts.Where(c => c.Id == contact.Id).Select(c=>c.DescriptionId).SingleOrDefault();
                //contactInDB.AddressId = db.Contacts.Where(c => c.Id == contact.Id).Select(c => c.AddressId).SingleOrDefault();
                db.Entry(contactInDB).State = EntityState.Modified;
                db.SaveChanges();
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
    }
}
