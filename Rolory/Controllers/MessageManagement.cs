﻿using Rolory.Models;
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
        // GET: Message
        public MessageManagement()
        {
            db = new ApplicationDbContext();
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
                message.ToEmail = user.Email;
                message.IsEmail = true;
                message.Postmark = DateTime.Now;
                SendEmail(message);
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
            var messageList = db.Messages.Where(m => m.Postmark == DateTime.Now).Where(m => m.IsActive == null).Select(m => m).ToList();
            var isActiveList = db.Messages.Where(m => m.Postmark == DateTime.Now).Where(m => m.IsActive == null).Select(m => m.IsActive).ToList();
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
    }
}