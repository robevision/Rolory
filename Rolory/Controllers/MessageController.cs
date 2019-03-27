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
        ApplicationDbContext db;
        // GET: Message
       public MessageController()
        {
            db = new ApplicationDbContext();
        }
        public ActionResult SendEmail()
        {
            
            return View();
        }
        [HttpPost]
        public ActionResult SendEmail(Message message)
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
                WebMail.Send(to: message.ToEmail, subject: message.Subject, body: message.Body, cc: message.EmailCC, bcc: message.EmailBCC, isBodyHtml: true);
                ViewBag.Status = "Email Sent Successfully.";
            }
            catch (Exception)
            {
                ViewBag.Status = "Problem while sending email, Please check details.";

            }
            return View();
        }
    }
}