using Rolory.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Rolory.Controllers
{
    public class RandomManagement
    {
        ApplicationDbContext db;
        WeatherManagement weath;
        public RandomManagement()
        {
            db = new ApplicationDbContext();
            weath = new WeatherManagement();
        }
        public List<Contact> GetContactsByBirthDate(List<Contact> filteredContactList)
        {
            var nextWeek = DateTime.Today.AddDays(6);
            List<Contact> pushedContactsByBirthDate = new List<Contact>();

            foreach (Contact contact in filteredContactList)
            {
                if(contact.DescriptionId != null)
                {
                    contact.Description = db.Contacts.Where(c => c.DescriptionId == contact.DescriptionId).Select(c => c.Description).SingleOrDefault();            
                }
                //contact.Description = db.Contacts.Where(c => c.DescriptionId == contact.DescriptionId).Select(c => c.Description).SingleOrDefault();
                //contact.Description.Id = db.Contacts.Where(c => c.DescriptionId == contact.DescriptionId.Value).Select(c => c.DescriptionId.Value).SingleOrDefault();
                if (contact.Description != null)
                {
                    DateTime? birthDateNullTest = contact.Description.BirthDate;
                    if (birthDateNullTest == null)
                    {
                        contact.Description.BirthDate = new DateTime(1801, 12, 25);
                    }
                    DateTime contactBirthDate = contact.Description.BirthDate.Value;
                    int contactBirthMonth = contactBirthDate.Month;

                    if (!contactBirthDate.ToString().Contains("12/25/1801"))
                    {
                        if (contactBirthMonth == DateTime.Today.Month || contactBirthDate <= nextWeek)
                        {
                            pushedContactsByBirthDate.Add(contact);
                        }
                    }
                }
              
                //db.Entry(contact).State = EntityState.Unchanged;
            }
            return pushedContactsByBirthDate;
        }
        public List<Contact> GetContactsByAnniversary(List<Contact> filteredContactList)
        {
            var nextWeek = DateTime.Today.AddDays(6);
            List<Contact> pushedContactsByAnniversaryDate = new List<Contact>();
            foreach(Contact contact in filteredContactList)
            {
                //contact.Description = db.Contacts.Where(c => c.DescriptionId == contact.DescriptionId).Select(c => c.Description).SingleOrDefault();
                //contact.Description.Id = db.Contacts.Where(c => c.DescriptionId == contact.DescriptionId.Value).Select(c => c.DescriptionId.Value).SingleOrDefault();
                if(contact.Description != null)
                {
                    DateTime? anniversaryDateNullTest = contact.Description.Anniversary;

                    if (anniversaryDateNullTest == null)
                    {
                        contact.Description.Anniversary = new DateTime(1801, 12, 25);
                    }

                    DateTime contactAnniversary = contact.Description.Anniversary.Value;
                    int contactAnniversaryMonth = contactAnniversary.Month;
                    if (!contactAnniversary.ToString().Contains("12/25/1801"))
                    {
                        if (contactAnniversaryMonth == DateTime.Today.Month || contactAnniversary <= nextWeek)
                        {
                            pushedContactsByAnniversaryDate.Add(contact);
                        }
                    }
                }
               
                //db.Entry(contact).State = EntityState.Unchanged;
            }
            return pushedContactsByAnniversaryDate;
        }
        public List<Contact> GetContactsByWorkTitle(List<Contact> filteredContactList)
        {
            List<Contact> pushedContactsByProfession = new List<Contact>();
            int networkerId = filteredContactList.Select(f => f.NetworkerId).FirstOrDefault();
            var networker = db.Networkers.Where(n => n.Id == networkerId).SingleOrDefault();
            foreach (Contact contact in filteredContactList)
            {
                //contact.Description = db.Contacts.Where(c => c.DescriptionId == contact.DescriptionId).Select(c => c.Description).SingleOrDefault();
                //contact.Description.Id = db.Contacts.Where(c => c.DescriptionId == contact.DescriptionId.Value).Select(c => c.DescriptionId.Value).SingleOrDefault();
                var contactWorkTitle = contact.WorkTitle;

                if (contactWorkTitle != null && networker.WorkTitle != null)
                {
                    if (contactWorkTitle.Contains(networker.WorkTitle))
                    {
                        pushedContactsByProfession.Add(contact);
                    }
                }
                //db.Entry(contact).State = EntityState.Unchanged;
            }
            return pushedContactsByProfession;
         
        }
        public List<Contact> GetContactsByRelation(List<Contact> filteredContactList)
        {
            List<Contact> pushedContactsByRelationship = new List<Contact>();

            foreach (Contact contact in filteredContactList)
            {
                //contact.Description = db.Contacts.Where(c => c.DescriptionId == contact.DescriptionId).Select(c => c.Description).SingleOrDefault();
                //contact.Description.Id = db.Contacts.Where(c => c.DescriptionId == contact.DescriptionId.Value).Select(c => c.DescriptionId.Value).SingleOrDefault();
                if(contact.Description != null)
                {
                    string contactRelation = contact.Description.Relationship;
                    if (contactRelation != null)
                    {
                        if (contactRelation == "Friend" || contactRelation == "Family")
                        {
                            pushedContactsByRelationship.Add(contact);
                        }
                    }
                }
               
            }
            return pushedContactsByRelationship;
        }
        public List<Contact> GetContactsBySharedActivities(List<Contact> filteredContactList)
        {
            List<Contact> pushedContactsBySharedActivities = new List<Contact>();
            int networkerId = filteredContactList.Select(f => f.NetworkerId).SingleOrDefault();
            var networker = db.Networkers.Where(n => n.Id == networkerId).SingleOrDefault();
            var contactListWithDescriptions = filteredContactList.Where(c => c.Description != null).Select(c => c).ToList();
            var descriptionIdsFromContactList = contactListWithDescriptions.Select(c => c.Description.Id).ToList();
            var sharedActivitiesDescriptionIds = db.SharedActivities.Select(s => s.DescriptionId).ToList();
            foreach (int descriptId1 in descriptionIdsFromContactList)
            {
                foreach (int descriptId2 in sharedActivitiesDescriptionIds)
                {
                    if (descriptId1 == descriptId2)
                    {
                        pushedContactsBySharedActivities.Add(db.Contacts.Where(c => c.DescriptionId == descriptId1).SingleOrDefault());
                    }
                }
            }
            return pushedContactsBySharedActivities;
        }
        public List<Contact> CheckSharedActivitiesWithSeason(List<Contact> filteredContactList)
        {
            List<Contact> pushedContactsBySharedActivities = new List<Contact>();
            List<SharedActivity> sharedActivities = new List<SharedActivity>();
            List<string> activeActivities = new List<string>();
            //List<string> winterActivities = new List<string>() { "snowboarding", "down hill", "ice skating", "sledding","snowmobiling","cross country" };
            //List<string> allAroundActivities = new List<string>() { "hiking","basketball"};
            List<int> descriptionIds = filteredContactList.Select(f => f.DescriptionId).ToList();
            List<int> sharedActivitiesDescriptionIds = db.SharedActivities.Select(s => s.DescriptionId).ToList();
           
            foreach (int descriptId in descriptionIds)
            {
               sharedActivities.Add(db.SharedActivities.Where(s => s.DescriptionId == descriptId).SingleOrDefault());
            }
            var allSeasons = sharedActivities.Select(s => s.Season).ToList();
            if (sharedActivities != null)
            {
                foreach(SharedActivity activity in sharedActivities)
                {
                    if (activity.Season == 0)
                    {
                        activeActivities.Add(activity.Activity);
                    }
                }
                
            }
            return pushedContactsBySharedActivities;
        }
        public void CheckContactCoolDown()
        {
            DateTime endCoolDownTimer = DateTime.Now;
            List<Contact> contactsBackInPool = new List<Contact>();
            var contactsWithCoolDown = db.Contacts.Where(c => c.CoolDown == true).ToList();

            foreach (Contact contact in contactsWithCoolDown)
            {
                if (contact.CoolDownTime.Value.AddHours(1) <= endCoolDownTimer)
                {
                    contactsBackInPool.Add(contact);
                }
            }

            List<bool> coolDownProperty = contactsBackInPool.Where(c => c.CoolDown == true).Select(c => c.CoolDown).ToList();
            for (int i = 0; i < coolDownProperty.Count(); i++)
            {
                coolDownProperty[i] = false;
            }
            foreach (Contact contact in contactsBackInPool)
            {
                foreach (bool coolDownBool in coolDownProperty)
                {
                    contact.CoolDown = coolDownBool;
                    db.Entry(contact).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
        }
    }
}