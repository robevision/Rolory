using Rolory.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Helpers;

namespace Rolory.Controllers
{
    public class MessageManagement
    {
        private ApplicationDbContext db;
        private Message errorMessage;
        private Random random;
        // GET: Message
        public MessageManagement()
        {
            db = new ApplicationDbContext();
            random = new Random();
        }
        public void BuildMessage(int id, string subject, string body, DateTime? postmark = null)
        {
            var networker = db.Networkers.Where(n => n.Id == id).SingleOrDefault();
            var user = db.Users.Where(u => u.Id == networker.UserId).SingleOrDefault();
            Message message = new Message();
            message.NetworkerId = id;
            message.Subject = subject;
            message.Body = body;
            if(postmark == null)
            {
                postmark = DateTime.Now;
            }
            message.Postmark = postmark.Value;
            if(message.Postmark <= DateTime.Now && message.Postmark.TimeOfDay <= DateTime.Now.TimeOfDay)
            {
                message.IsActive = true;
                SendMessage(message);
            }
            else
            {
                HoldMessage(message);
            }
            
        }
        public void BuildEmail(int id, string subject, string body, string cc = null, string bcc = null)
        {
            var networker = db.Networkers.Where(n => n.Id == id).SingleOrDefault();
            var user = db.Users.Where(u => u.Id == networker.UserId).SingleOrDefault();
            Message message = new Message();
            message.NetworkerId = id;
            var emailAddressNullCheck = user.Email;
            message.Subject = subject;
            message.Body = body;
            message.EmailCC = cc;
            message.EmailBCC = bcc;

            if (emailAddressNullCheck != null && networker.ReceiveEmails == true)
            {
                if (networker.EmailFrequency > 0)
                {
                    networker.EmailFrequency--;
                    db.Entry(networker).State = EntityState.Modified;
                    db.SaveChanges();
                    message.ToEmail = user.Email;
                    message.IsEmail = true;
                    message.Postmark = DateTime.Now;
                    SendEmail(message);
                }
               
            }

            else
            {
                message.IsActive = true;
                message.IsEmail = false;
                message.Postmark = DateTime.Now;
                SendMessage(message);
            }
        }

        public void SendEmail(Message builtMessage)
        {
            try
            {
                //Configuring webMail class to send emails  
                //gmail smtp server  
                WebMail.SmtpServer = "smtp.gmail.com";
                //gmail port to send emails  
                WebMail.SmtpPort = 587;
                WebMail.SmtpUseDefaultCredentials = true;
                //sending emails with secure protocol  
                WebMail.EnableSsl = true;
                //EmailId used to send emails from application  
                WebMail.UserName = "rolorycontactmanager@gmail.com";
                WebMail.Password = "HelloWorld1234!";

                //Sender email address.  
                WebMail.From = "rolorycontactmanager@gmail.com";

                //Send email  
                WebMail.Send(to: builtMessage.ToEmail, subject: builtMessage.Subject, body: builtMessage.Body, cc: builtMessage.EmailCC, bcc: builtMessage.EmailBCC, isBodyHtml: true);
                db.Messages.Add(builtMessage);
                db.SaveChanges();
            }
            catch (Exception)
            {
                errorMessage = new Message();
                errorMessage.Subject = "Error";
                errorMessage.Body = "Unsuccessful email";
                errorMessage.NetworkerId = builtMessage.NetworkerId;
                errorMessage.Postmark = DateTime.Now;
                SendMessage(builtMessage);
                db.Messages.Add(errorMessage);
                db.SaveChanges();
            }
        }
        public void SendMessage(Message builtMessage)
        {
            db.Messages.Add(builtMessage);
            db.SaveChanges();
        }
        public void HoldMessage(Message builtMessage)
        {
            db.Messages.Add(builtMessage);
            db.SaveChanges();
            CycleMessages();
        }
        public void CycleMessages()
        {
            var messageList = db.Messages.Where(m => m.Postmark >= DateTime.Now).Where(m => m.IsActive == null).Select(m => m).ToList();
            var isActiveList = db.Messages.Where(m => m.Postmark >= DateTime.Now).Where(m => m.IsActive == null).Select(m => m.IsActive).ToList();
            for (int i = 0; i < isActiveList.Count(); i++)
            {
                isActiveList[i] = true;
            }
            foreach (Message message in messageList)
            {
                foreach (bool? isActive in isActiveList)
                {
                    message.IsActive = isActive;
                    db.Entry(message).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
        }
        public void GenerateEmail(int? id)
        {
            var nextWeek = DateTime.Today.AddDays(7);
            List<Contact> pushedContacts = new List<Contact>();
            List<Contact> pushedContactsByBirthDate = new List<Contact>();
            List<Contact> pushedContactsByAnniversaryDate = new List<Contact>();
            List<Contact> pushedContactsByProfession = new List<Contact>();
            List<Contact> pushedContactsByRelationship = new List<Contact>();
            string subject;
            string body;
            var networker = db.Networkers.Where(n => n.Id == id).Select(n => n).SingleOrDefault();
            var contactsList = db.Contacts.Where(c => c.NetworkerId == id).Where(c => c.InContact == false).Where(c => c.Description.DeathDate == null).Select(c => c).ToList();
            Contact filteredContact = null;
            var filteredContactList = contactsList.Where(c => c.Perpetual == false).ToList();
            foreach (Contact contact in filteredContactList)
            {
                contact.Description = db.Contacts.Where(c => c.DescriptionId == contact.DescriptionId).Select(c => c.Description).SingleOrDefault();
                DateTime? birthDateNullTest = contact.Description.BirthDate;
                DateTime? anniversaryDateNullTest = contact.Description.Anniversary;
                if (birthDateNullTest == null)
                {
                    contact.Description.BirthDate = new DateTime(1801, 12, 25);
                }
                DateTime contactBirthDate = contact.Description.BirthDate.Value;
                int contactBirthMonth = contactBirthDate.Month;
                var contactWorkTitle = contact.WorkTitle;
                if (anniversaryDateNullTest == null)
                {
                    contact.Description.Anniversary = new DateTime(1801, 12, 25);
                }
                DateTime contactAnniversary = contact.Description.Anniversary.Value;
                int contactAnniversaryMonth = contactAnniversary.Month;
                var contactRelation = contact.Description.Relationship;
                if (!contactBirthDate.ToString().Contains("12/25/1801"))
                {
                    if (contactBirthMonth == DateTime.Today.Month || contactBirthDate <= nextWeek)
                    {
                        pushedContactsByBirthDate.Add(contact);
                    }
                }
                if (contactWorkTitle != null && networker.WorkTitle != null)
                {
                    if (contactWorkTitle.Contains(networker.WorkTitle))
                    {
                        pushedContactsByProfession.Add(contact);
                    }
                }
                if (!contactAnniversary.ToString().Contains("12/25/1801"))
                {
                    if (contactAnniversaryMonth == DateTime.Today.Month || contactAnniversary <= nextWeek)
                    {
                        pushedContactsByAnniversaryDate.Add(contact);
                    }
                }
                if (contactRelation != null)
                {
                    if (contactRelation == "Friend" || contactRelation == "Family")
                    {
                        pushedContactsByRelationship.Add(contact);
                    }
                }
            }
            pushedContacts.Add(pushedContactsByBirthDate.SingleOrDefault());
            pushedContacts.Add(pushedContactsByAnniversaryDate.SingleOrDefault());
            pushedContacts.Add(pushedContactsByProfession.SingleOrDefault());
            pushedContacts.Add(pushedContactsByRelationship.SingleOrDefault());
            //pushedContacts.Add(pushedContactsBySharedActivities.SingleOrDefault());
            do
            {
                int r = random.Next(pushedContacts.Count);
                filteredContact = pushedContacts[r];
            }
            while (filteredContact == null);
            if (filteredContact.Id == pushedContactsByBirthDate.Select(p => p.Id).SingleOrDefault())
            {
                subject = $"{filteredContact.GivenName}'s Birthday".ToString();
                body = $"It is {filteredContact.GivenName}'s birthday soon. Why not reach out?".ToString();
                if (filteredContact != null && networker.EmailFrequency > 0)
                {
                    BuildEmail(networker.Id, subject, body);
                }
            }
            else if (filteredContact.Id == pushedContactsByAnniversaryDate.Select(p => p.Id).SingleOrDefault())
            {
                subject = $"{filteredContact.GivenName}'s Anniversary".ToString();
                body = $"It is {filteredContact.GivenName}'s anniversary soon. Why not reach out?".ToString();
                if (filteredContact != null && networker.EmailFrequency > 0)
                {
                    BuildEmail(networker.Id, subject, body);
                }
            }
            else if (filteredContact.Id == pushedContactsByProfession.Select(p => p.Id).SingleOrDefault())
            {
                subject = $"{filteredContact.GivenName}'s Job Is Similar To Yours".ToString();
                body = $"{filteredContact.GivenName} and you share a career. Why not reach out?".ToString();
                if (filteredContact != null && networker.EmailFrequency > 0)
                {
                    BuildEmail(networker.Id, subject, body);
                }
            }
            else if (filteredContact.Id == pushedContactsByRelationship.Select(p => p.Id).SingleOrDefault())
            {
                subject = $"Remember {filteredContact.GivenName} {filteredContact.FamilyName}?".ToString();
                body = $"You should get back in touch with {filteredContact.GivenName}.It's been a while.".ToString();
                if (filteredContact != null && networker.EmailFrequency > 0)
                {
                    BuildEmail(networker.Id, subject, body);
                }
            }

        }
    }
}