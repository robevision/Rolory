using Microsoft.AspNet.Identity;
using Rolory.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace Rolory.Controllers
{
    public class MessageController : Controller
    {
        private ApplicationDbContext db;
        // GET: Message
       public MessageController()
        {
            db = new ApplicationDbContext();
        }
        //BuildEmail(5)
        public ActionResult BuildEmail(int id, string subject, string body, string cc = null, string bcc = null)
        {
            string userId = User.Identity.GetUserId();
            var networker = db.Networkers.Where(n => n.Id == id).SingleOrDefault();
            var user = db.Users.Where(u => u.Id == userId).SingleOrDefault();
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
                return RedirectToAction("SendEmail", "Message", new { builtMessage = message });
            }
            else
            {
                message.IsActive = true;
                message.IsEmail = false;
                message.Postmark = DateTime.Now;
                return RedirectToAction("SendMessage", "Message", new { builtMessage = message });
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
                ViewBag.Status = "Email Sent Successfully.";
                db.Messages.Add(builtMessage);
                db.SaveChanges();
            }
            catch (Exception)
            {
                ViewBag.Status = "Problem while sending email, Please check details.";

            }
        }
        public void SendMessage(Message builtMessage)
        {
            db.Messages.Add(builtMessage);
            db.SaveChanges();
        }
    }
}