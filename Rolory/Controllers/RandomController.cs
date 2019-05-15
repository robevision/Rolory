using Microsoft.AspNet.Identity;
using Rolory.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Rolory.Controllers
{
    public class RandomController : Controller
    {
        private ApplicationDbContext db;
        private readonly MessageManagement msg;
        private RandomManagement rndmngmnt;
        private ContactsManagement cm;
        Random random;
        public RandomController()
        {
            db = new ApplicationDbContext();
            random = new Random();
            msg = new MessageManagement();
            rndmngmnt = new RandomManagement();
            cm = new ContactsManagement();

        }
        // GET: Random
        [Authorize]
        [HttpGet]
        public ActionResult Index(int? id = null)
        {
            if (id == null || id == 0)
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
                var contactList = db.Contacts.Where(c => c.NetworkerId == networker.Id).Where(c => c.InContact == false).Where(c => c.CoolDown == false).Where(c => c.Description.DeathDate == null).ToList();
                var filteredContactList = contactList.Where(c => c.Perpetual == false).ToList();

                pushedContactsByBirthDate = rndmngmnt.GetContactsByBirthDate(filteredContactList);
                pushedContactsByAnniversaryDate = rndmngmnt.GetContactsByAnniversary(filteredContactList);
                pushedContactsByProfession = rndmngmnt.GetContactsByWorkTitle(filteredContactList);
                pushedContactsByRelationship = rndmngmnt.GetContactsByRelation(filteredContactList);

                //filteredContactsBySharedActivities = rndmngmnt.GetContactsBySharedActivities(filteredContactList);
                //rndmngmnt.CheckSharedActivitiesWithSeason(filteredContactsBySharedActivities);
                if (pushedContactsByBirthDate != null)
                {
                    foreach (Contact contact in pushedContactsByBirthDate)
                    {
                        pushedContacts.Add(contact);
                    }
                }
                if (pushedContactsByAnniversaryDate != null)
                {
                    foreach (Contact contact in pushedContactsByAnniversaryDate)
                    {
                        pushedContacts.Add(contact);
                    }
                }
                if (pushedContactsByProfession != null)
                {
                    foreach (Contact contact in pushedContactsByProfession)
                    {
                        pushedContacts.Add(contact);
                    }
                }
                if (pushedContactsByRelationship != null)
                {
                    foreach (Contact contact in pushedContactsByRelationship)
                    {
                        pushedContacts.Add(contact);
                    }
                }
                pushedContacts.Add(pushedContactsBySharedActivities.SingleOrDefault());
                foreach (Contact contact in pushedContacts)
                {
                    if (contact != null)
                    {
                        newPushedContacts.Add(contact);
                    }
                }
                if (newPushedContacts.Count == 0)
                {
                    if (filteredContactList.Count != 0)
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
                if (pushedContacts.Count != 0)
                {
                    do
                    {
                        int r = random.Next(pushedContacts.Count);
                        if (pushedContacts.Count == 1)
                        {
                            filteredContact = pushedContacts.SingleOrDefault();
                            if (filteredContact == null)
                            {
                                break;
                            }
                        }
                        filteredContact = pushedContacts[r];
                    }
                    while (filteredContact == null);
                }

                if (pushedContactsByBirthDate.Select(p => p.Id).Contains(filteredContact.Id))
                {
                    DateTime thisBirthDate = db.Descriptions.Where(d => d.Id == filteredContact.DescriptionId).Select(d => d.BirthDate).SingleOrDefault().Value;
                    int age = DateTime.Today.Year - thisBirthDate.Year;
                    if (thisBirthDate.Day > DateTime.Today.Day)
                    {
                        ViewBag.Message = $"It is {filteredContact.GivenName}'s birthday soon. Why not reach out?";
                    }
                    if (age / 10 == Convert.ToInt32(age.ToString().IndexOf("0")))
                    {
                        ViewBag.Message = $"{filteredContact.GivenName} is turning {age}. Why not reach out?";
                    }
                    if (thisBirthDate.Day == DateTime.Today.Day)
                    {
                        ViewBag.Message = $"It is {filteredContact.GivenName}'s birthday today!";
                    }
                    if (thisBirthDate.Day < DateTime.Today.Day)
                    {
                        ViewBag.Message = $"{filteredContact.GivenName} had their birthday recently. You should check in.";
                    }

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
            else
            {
                Contact contact = db.Contacts.Where(c => c.Id == id).Select(c => c).SingleOrDefault();
                return View(contact);
            }
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
        public ActionResult GetInTouch(int? id, string message = null)
        {
            string userId = User.Identity.GetUserId();
            var networker = db.Networkers.Where(n => n.UserId == userId).SingleOrDefault();
            var nullDateTime = new DateTime();
            var today = DateTime.Now.Day;
            var week = DateTime.Now.AddDays(6);
            DateTime weekAgo = DateTime.Now.AddDays(-6);
            var contact = db.Contacts.Where(c => c.Id == id).Select(c => c).SingleOrDefault();
            var description = db.Descriptions.Where(d => d.Id == contact.DescriptionId).Select(d => d).SingleOrDefault();
            string firstPronoun = null;
            string secondPronoun = null;
            random = new Random();
            var chance = random.Next(2);
            var secondChance = random.Next(2);
            if (contact.Prefix != null)
            {
                if (contact.Prefix == "Mr." || contact.Prefix == "Master")
                {
                    firstPronoun = "Him";
                    secondPronoun = "He";
                }
                if (contact.Prefix == "Mrs.")
                {
                    firstPronoun = "Her";
                    secondPronoun = "She";
                }
                else
                {
                    firstPronoun = "Them";
                    secondPronoun = "They";
                }
            }


            if (contact.WorkTitle != null && networker.WorkTitle != null)
            {
                if (contact.WorkTitle == networker.WorkTitle || contact.WorkTitle.Contains(networker.WorkTitle))
                {
                    string[] workTitleMessages = new string[]
                {
                    $"You and {contact.GivenName} may have a similar occupation. Why not set up a coffee to share your experiences?",
                    $"'Hey {contact.GivenName}, I heard you are working at {contact.Organization}. How has that been?'", $"'Hi {contact.GivenName}, how has it been since...'",
                    "'You came up on my feed because of...we should catch up!'", "'I hope you are having a great day!'",
                    "'How about, 'Hey, It's been a while since we last spoke, what have you been up to?'",
                    $"'Hey there {contact.GivenName}. Had a memory recently of when... How have you been?'",
                    "'What are you up to these days? We should grab coffee to catch up!'"
                };
                    random = new Random();
                    var newRandom = random.Next(8);
                    if (workTitleMessages[newRandom] != message)
                    {
                        ViewBag.Message = workTitleMessages[newRandom];
                        return View(contact);
                    }

                }
            }
            if (description.Relationship != null || Convert.ToString(description.Relationship) != String.Empty)
            {
                if (description.Relationship == "Friend")
                {
                    ViewBag.Message = "friend";
                }
                if (description.Relationship == "Family")
                {
                    ViewBag.Message = "family";
                }
                if (contact.Description.Relationship == "Classmate")
                {
                    ViewBag.Message = "classmate";
                }
            }
            if (description.Anniversary != null && description.Anniversary.ToString() != String.Empty && description.Anniversary != nullDateTime)
            {
                random = new Random();
                DateTime contactAnniversary = description.Anniversary.Value;
                if (contactAnniversary.Month == DateTime.Now.Month && secondChance == 1)
                {
                    if (description.Anniversary.Value.Day == today)
                    {
                        ViewBag.Message = $"It is {contact.GivenName}'s wedding Anniversary today! Just a simple 'Happy Anniversary' can get a conversation going about how both of you have been!";
                    }
                    else if (today - description.Anniversary.Value.Day < 7 && today - description.Anniversary.Value.Day > 0 /*&& lateBirthDayMessages.Where(w => w == message).Any() != true*/)
                    {
                        ViewBag.Message = $"It was {contact.GivenName}'s wedding Anniversary on {contact.Description.Anniversary.Value.DayOfWeek}! Just a simple 'Happy Anniversary' shows you're thinking of them.";
                    }
                    else if (today - description.Anniversary.Value.Day > -7 && today - description.Anniversary.Value.Day < 0)
                    {
                        ViewBag.Message = $"{contact.GivenName}'s Anniversary is coming up! It's on {description.BirthDate.Value.DayOfWeek}! Reaching out with a 'Happy Anniversary' shows you're thinking of them.";
                    }
                }
            }
            if (description.BirthDate != null && description.BirthDate.Value.ToString() != String.Empty && description.BirthDate != nullDateTime /*&& messageCooldown.Select(m => m.Contains("Birth")).Any() != true*/)
            {
                random = new Random();
                DateTime contactBirthDate = description.BirthDate.Value;

                if (contactBirthDate.Month == DateTime.Now.Month && chance == 1)
                {
                    string[] birthDayMessages = new string[]
                     {
                             $"It is {contact.GivenName}'s Birthday today! Just a simple 'Happy Birthday' can get a conversation going about how both of you have been!"
                     };
                    string[] lateBirthDayMessages = new string[]
                        {
                            "test",
                            $"It was {contact.GivenName}'s Birthday on {description.BirthDate.Value.DayOfWeek} the {description.BirthDate.Value.Day}! Just a simple 'Happy Belated Birthday' shows you're thinking of {firstPronoun.ToLower()}.",
                            $"{contact.GivenName} {contact.FamilyName} just had their birthday. Not to worry. It was just the other day. Send your best wishes to let {firstPronoun.ToLower()} know that {secondPronoun.ToLower()} is on your mind."
                        }; //suffix variable needed for after number
                    string[] earlyBirthDayMessages = new string[]
                    {
                            $"{contact.GivenName}'s Birthday is coming up! It's on {description.BirthDate.Value.DayOfWeek}! Reaching out with a 'Happy Birthday' shows you're thinking of them."
                    };

                    if (contactBirthDate.Day == today && birthDayMessages.Where(w => w == message).Any() != true)
                    {
                       
                        random = new Random();
                        var newRandom = random.Next(birthDayMessages.Count());
                        ViewBag.Message = birthDayMessages[newRandom];

                    }
                    else if (today - contactBirthDate.Day < 7 && today - contactBirthDate.Day > 0 && lateBirthDayMessages.Where(w => w == message).Any() != true)
                    {
                        random = new Random();
                        var newRandom = random.Next(lateBirthDayMessages.Count());
                        ViewBag.Message = lateBirthDayMessages[newRandom];
                    }
                    else if (today - description.BirthDate.Value.Day > -7 && today - description.BirthDate.Value.Day < 0 && earlyBirthDayMessages.Where(w => w == message).Any() != true)
                    {
                        random = new Random();
                        var newRandom = random.Next(earlyBirthDayMessages.Count());
                        ViewBag.Message = earlyBirthDayMessages[newRandom];
                    }
                }
                else if (contactBirthDate.Month - DateTime.Now.Month == 1 || DateTime.Now.Month - contactBirthDate.Month == 11)
                {
                    if (today - description.BirthDate.Value.Day < 7 && today - description.BirthDate.Value.Day > 0)
                    {
                        ViewBag.Message = $"It was {contact.GivenName}'s Birthday on {description.BirthDate.Value.DayOfWeek} the {description.BirthDate.Value.Day}! Just a simple 'Happy Belated Birthday' shows you're thinking of them."; //suffix variable needed for after number
                    }
                    else if (today - description.BirthDate.Value.Day > -7 && today - description.BirthDate.Value.Day < 0)
                    {
                        ViewBag.Message = $"{contact.GivenName}'s Birthday is coming up! It's on {description.BirthDate.Value.DayOfWeek}! Reaching out with a 'Happy Birthday' shows you're thinking of them.";
                    }
                }
            }

            {
                WeatherManagement weath = new WeatherManagement();
                var season = weath.GetCurrentSeason();

                if (ViewBag.Message == null)
                {
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
                    var newRandom = random.Next(genericMessages.Count());
                    if (genericMessages[newRandom] != message)
                    {
                        ViewBag.Message = genericMessages[newRandom];
                    }
                    else
                    {
                        random = new Random();
                        newRandom = random.Next(genericMessages.Count());
                        ViewBag.Message = genericMessages[newRandom];
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
            List<PropertyInfo> nullPropertiesList = new List<PropertyInfo>();
            Contact contact = db.Contacts.Where(c => c.Id == id).Select(c => c).SingleOrDefault();
            Description description = db.Descriptions.Where(d => d.Id == contact.DescriptionId).Select(d => d).SingleOrDefault();
            List<SharedActivity> sharedActivity = new List<SharedActivity>();
            //I want to query all shared activities from every contact that the networker has
            sharedActivity = db.SharedActivities.Where(s => s.DescriptionId == contact.Id).ToList();
            string userId = User.Identity.GetUserId();
            Networker networker = db.Networkers.Where(n => n.UserId == userId).SingleOrDefault();
            Address address = db.Addresses.Where(a => a.Id == contact.AddressId).Select(a => a).SingleOrDefault();
    
            //Update Info. Will be recursive in adding info about the person to log. Have a button for when they want to go to the next person.
            if (contact!= null)
            {
                if (answer == null)
                {
                    //if(sharedActivity != 0)
                    //{
                    //if(weather == summer ...)
                    //}
                    foreach (PropertyInfo property in contact.GetType().GetProperties())
                {
                    if(property.Name != "ImageTitle" && property.Name != "ImagePath" && property.Name != "Id" && property.Name != "InContract" && property.Name != "LastUpdated" && property.Name != "PhoneType" && property.Name != "AltPhoneType" && property.Name != "Description" && property.Name != "DescriptionId" && property.Name != "NetworkerId" && property.Name != "Networker" && property.Name != "AddressId" && property.Name != "AltAddressId" && property.Name != "InContact" && property.Name != "InContactCountDown" && property.Name != "CoolDown" && property.Name != "Reminder")
                    {
                        if (property.GetValue(contact) == null && property.Name != question)
                        {
                            nullPropertiesList.Add(property);
                        }
                            else if (Convert.ToString(property.GetValue(contact)) == String.Empty && property.Name != question)
                            {
                                nullPropertiesList.Add(property);
                            }
                        }
                   
                }
                foreach(PropertyInfo property in description.GetType().GetProperties())
                {
                    if(property.Name != "ContactId" && property.Name != "Id" && property.Name != "DeathDate" && property.Name != "Notes")
                    {
                            //if (property.PropertyType.Name.Contains("Int") && property.GetValue(description) == 0)
                            //{
                            //    nullPropertiesList.Add(property);
                            //}
                            if (property.GetValue(description) == null && property.Name != question)
                        {
                            nullPropertiesList.Add(property);
                        }
                            else if(Convert.ToString(property.GetValue(description)) == String.Empty && property.Name != question)
                            {
                                nullPropertiesList.Add(property);
                            }
                    }
                }
                foreach(PropertyInfo property in address.GetType().GetProperties())
                    {
                        if(property.Name != "Id" && property.Name != "Latitude" && property.Name != "Longitude" && property.Name != "Unit")
                        {
                            if (property.GetValue(address) == null && property.Name != question)
                            {
                                nullPropertiesList.Add(property);
                            }
                            else if (Convert.ToString(property.GetValue(address)) == String.Empty && property.Name != question)
                            {
                                nullPropertiesList.Add(property);
                            }
                        }
                    }
                random = new Random();
                int r = random.Next(nullPropertiesList.Count);
                var nullProperty = nullPropertiesList[r];
                    if(nullProperty != null)
                    {
                        StringBuilder formattedQuestion = new StringBuilder(nullProperty.Name.Length * 2);
                        for (int i = 0; i < nullProperty.Name.Length; i++)
                        {
                            if (i != 0 && i != nullProperty.Name.Length && Char.IsUpper(nullProperty.Name[i]) == true)
                            {
                                formattedQuestion.Append(' ');
                            }
                            formattedQuestion.Append(nullProperty.Name[i]);
                        }
                        string sentQuestion = formattedQuestion.ToString().ToLower();
                        ViewBag.RawQuestion = nullProperty.Name.ToString();
                        ViewBag.Question = sentQuestion;
                        ViewBag.Message = $"Do you know {contact.GivenName}'s {sentQuestion}?";
                        ViewBag.IsQuestion = "false";
                        if(nullProperty.Name.ToString() == "AltPhoneNumber")
                        {
                            ViewBag.Message = $"Does {contact.GivenName} have a secondary phone number?";
                        }
                        else if(nullProperty.Name.ToString() == "AltAddress")
                        {
                            ViewBag.Message = $"Does {contact.GivenName} have another address?";
                        }
                        else if(nullProperty.Name.ToString() == "Organization")
                        {
                            ViewBag.Message = $"Do you know the company that {contact.GivenName} works for?";
                        }
                        else if(nullProperty.Name.ToString() == "WorkTitle")
                        {
                            ViewBag.Message = $"Do you know what {contact.GivenName}'s position is?";
                        }
                        else if(nullProperty.Name.ToString() == "Relationship")
                        {
                            ViewBag.Message = $"Do you know how close you are to {contact.GivenName}?";
                            var relationship = cm.relationshipList;
                            ViewBag.DropDown = relationship;
                        }
                        else if(nullProperty.Name.ToString() == "Category")
                        {
                            ViewBag.Message = $"Do you remember how you met {contact.GivenName}?";
                           
                        }
                        else if(nullProperty.Name.ToString() == "Gender")
                        {
                            var gender = cm.genderList;
                            ViewBag.DropDown = gender;
                        }
                        else if(nullProperty.Name.ToString() == "Prefix")
                        {
                            var prefix = cm.prefixList;
                            ViewBag.DropDown = prefix;
                        }
                    }
                    if (address != null && address.ZipCode != 0 && address.ZipCode != null && networker.AddressId != null)
                    {
                        networker.Address = db.Addresses.Where(a => a.Id == networker.AddressId).SingleOrDefault();
                        if (networker.Address != null && networker.Address.ZipCode == null || networker.Address.ZipCode == 0)
                        {
                            ViewBag.Message = $"Does {contact.GivenName} live near you?";
                            question = "zipBind";
                        }
                        else if(networker.Address == null)
                        {
                            Address networkerAddress = new Address();
                            networker.Address = networkerAddress;
                            ViewBag.Message = $"Does {contact.GivenName} live near you?";
                            question = "zipBind";
                        }
                    }

                }
                else if(answer == "Submit")
                {
                    if (question != null && question != "zipBind")
                    {
                        StringBuilder formattedQuestion = new StringBuilder(question.Length * 2);
                        if (question == "Address" || question == "ZipCode" || question == "StreetAddress" || question == "AddressType" || question == "CountryName" || question == "Region" || question == "Locality")
                        {
                            return RedirectToAction("BuildAddress", "Contacts", new { passedId = id });
                        }
                        for (int i = 0; i < question.Length; i++)
                        {
                            if (i != 0 && i != question.Length && Char.IsUpper(question[i]) == true)
                            {
                                formattedQuestion.Append(' ');
                            }
                            formattedQuestion.Append(question[i]);
                        }
                        string sentQuestion = formattedQuestion.ToString().ToLower();
                        ViewBag.Message = $"What is {contact.GivenName}'s {sentQuestion}?";
                        ViewBag.IsQuestion = "true";
                        ViewBag.RawQuestion = question;
                        if (question == "AltPhoneNumber")
                        {
                            ViewBag.Message = $"What is {contact.GivenName}'s alternate phone number?";
                        }
                        if(question == "Category")
                        {
                            ViewBag.Message = $"How did you meet {contact.GivenName}?";
                            var category = cm.categoryList;
                            ViewBag.DropDown = category;
                        }
                        if (question == "Relationship")
                        {
                            ViewBag.Message = $"How well do you know {contact.GivenName}?";
                            var relationship = cm.relationshipList;
                            ViewBag.DropDown = relationship;
                        }
                       
                    }
                    else if(question == "zipBind")
                    {
                        if(networker.Address != null && contact.Address != null)
                        {
                            networker.Address.ZipCode = contact.Address.ZipCode;
                            db.Entry(networker).State = EntityState.Modified;
                            db.SaveChanges();
                            question = null;
                            answer = null;
                        }
                        else if(networker.AddressId != null)
                        {
                            networker.Address = db.Addresses.Where(a => a.Id == networker.AddressId).SingleOrDefault();
                            if(contact.AddressId != 0)
                            {
                                contact.Address = db.Addresses.Where(a => a.Id == contact.AddressId).SingleOrDefault();
                                networker.Address.ZipCode = contact.Address.ZipCode;
                                db.Entry(networker).State = EntityState.Modified;
                                db.SaveChanges();
                                question = null;
                                answer = null;
                            }
                        }
                        
                        return RedirectToAction("UpdateInfo", new { id = contact.Id });
                    }
                }
                else
                {
                    bool found = false;
                    foreach (PropertyInfo property in contact.GetType().GetProperties())
                    {
                        if (property.Name.ToString() == question)
                        {
                            if (question == "AltPhoneNumber" || question == "PhoneNumber")
                            {
                                List<string> phoneNumberResult = new List<string>();
                                List<char> areaCode = new List<char>();
                                List<char> body = new List<char>();
                                var phoneNumber = contact.PhoneNumber;
                                List<int> intPhoneNumber = new List<int>();
                                for (int i = 0; i < phoneNumber.Count(); i++)
                                {
                                    if (Char.IsNumber(phoneNumber[i]) == true)
                                    {
                                        intPhoneNumber.Add(phoneNumber[i]);
                                    }
                                }

                                for (int i = 0; i < intPhoneNumber.Count(); i++)
                                {
                                    if (i == 0)
                                    {
                                        areaCode.Add(Convert.ToChar("("));
                                        areaCode.Add(Convert.ToChar(intPhoneNumber[i]));
                                    }
                                    else if (i == 1)
                                    {
                                        areaCode.Add(Convert.ToChar(intPhoneNumber[i]));
                                    }
                                    else if (i == 2)
                                    {
                                        areaCode.Add(Convert.ToChar(intPhoneNumber[i]));
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
                                            body.Add(Convert.ToChar(intPhoneNumber[i]));
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
                                if (question == "PhoneNumber")
                                {
                                    contact.PhoneNumber = String.Join("", phoneNumberResult.ToArray());
                                    answer = null;
                                    question = null;
                                }
                                else if (question == "AltPhoneNumber")
                                {
                                    contact.AltPhoneNumber = String.Join("", phoneNumberResult.ToArray());
                                    answer = null;
                                    question = null;
                                }

                            }
                            else
                            {
                                var passedProperty = "{" + contact + "." + question + "}";
                                passedProperty = answer;
                                found = true;
                                property.SetValue(contact, answer);
                                db.Entry(contact).State = EntityState.Modified;
                                db.SaveChanges();
                                answer = null;
                                question = null;

                            }
                        }
                    }
                    if(found == false)
                    {
                        foreach (PropertyInfo property in description.GetType().GetProperties())
                        {
                            if (property.Name.ToString() == question)
                            {
                                if(question == "Anniversary" || question == "BirthDate")
                                {
                                    DateTime dateAnswer = new DateTime();
                                    dateAnswer = Convert.ToDateTime(answer);
                                    property.SetValue(description, dateAnswer);
                                    db.Entry(description).State = EntityState.Modified;
                                    db.SaveChanges();
                                    answer = null;
                                    question = null;
                                }
                                else
                                {
                                    var passedProperty = "{" + contact + "." + description + "." + question + "}";
                                    passedProperty = answer;
                                    found = true;
                                    property.SetValue(description, answer);
                                    db.Entry(description).State = EntityState.Modified;
                                    db.SaveChanges();
                                    answer = null;
                                    question = null;
                                }
                          
                            }
                        }
                        if(found == false)
                        {
                            foreach (PropertyInfo property in address.GetType().GetProperties())
                            {
                                if (property.Name.ToString() == question)
                                {
                                    if (property.PropertyType.ToString().Contains("Int"))
                                    {
                                        var newAnswer = Convert.ToInt32(answer);
                                        var passedProperty = "{" + contact + "." + address + "." + question + "}";
                                        passedProperty = answer;
                                        found = true;
                                        property.SetValue(address, newAnswer);
                                        db.Entry(address).State = EntityState.Modified;
                                        db.SaveChanges();
                                        answer = null;
                                        question = null;
                                    }
                                    else
                                    {
                                        var passedProperty = "{" + contact + "." + address + "." + question + "}";
                                        passedProperty = answer;
                                        found = true;
                                        property.SetValue(address, answer);
                                        db.Entry(address).State = EntityState.Modified;
                                        db.SaveChanges();
                                        answer = null;
                                        question = null;
                                    }
                               
                                }
                            }
                           
                        }
                        
                    }
                    return RedirectToAction("UpdateInfo", id = contact.Id);
                }
               
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
